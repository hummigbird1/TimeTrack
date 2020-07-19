using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;

namespace TimeTrack.Console
{
    public class CommandlineArgumentParser : ICommandlineArgumentParser
    {
        private readonly ICommandOptionProvider _commandOptionProvider;
        private readonly IOutputWriter _outputWriter;

        public CommandlineArgumentParser(ICommandOptionProvider commandOptionProvider, IOutputWriter outputWriter)
        {
            _commandOptionProvider = commandOptionProvider;
            _outputWriter = outputWriter;
        }

        public bool TryParseArguments(IEnumerable<string> arguments, out ICommandOption commandOption)
        {
            commandOption = null;

            var parser = CreateParser();
            var result = parser.ParseArguments(arguments, _commandOptionProvider.OptionTypes);
            if (result.Tag == ParserResultType.Parsed)
            {
                commandOption = (ICommandOption)((Parsed<object>)result).Value;
                return true;
            }
            var notParsedResult = (NotParsed<object>)result;
            if (IsBadVerbParsingError(notParsedResult, out var badVerb))
            {
                var aliasOption = ParseAsAliasOption(arguments, badVerb);
                if (aliasOption != null)
                {
                    commandOption = aliasOption;
                    return true;
                }

                return false;
            }

            RenderHelpText(result);
            return false;
        }

        public void WriteHelpTextToOutput(IEnumerable<string> arguments)
        {
            var parser = CreateParser();
            var result = parser.ParseArguments(arguments, _commandOptionProvider.OptionTypes);
            RenderHelpText(result);
        }

        private static IList<string> CreateArgumensForAliasOption(IEnumerable<string> originalArguments, string verbToken)
        {
            var argumentList = originalArguments.Except(new[] { verbToken }).ToList();
            var actualVerb = typeof(AliasOptions).GetCustomAttribute<VerbAttribute>().Name;
            argumentList.Insert(0, actualVerb);

            var option = typeof(AliasOptions).GetProperty(nameof(AliasOptions.Alias)).GetCustomAttribute<OptionAttribute>().LongName;

            return argumentList.Union(new[] { $"--{option}", verbToken })
                .ToList();
        }

        private Parser CreateParser(StringBuilder helpTextBuilder = null)
        {
            return new Parser(config =>
            {
                config.CaseSensitive = false;
                config.AutoHelp = true;
                config.HelpWriter = helpTextBuilder == null ? new StringWriter() : new StringWriter(helpTextBuilder);
            });
        }

        private bool IsBadVerbParsingError(NotParsed<object> notParsed, out string badVerb)
        {
            badVerb = null;
            var errors = notParsed.Errors.ToList();
            if (errors.Count == 1)
            {
                var badVerbError = errors.SingleOrDefault(x => x.Tag == ErrorType.BadVerbSelectedError);
                if (badVerbError != null)
                {
                    badVerb = ((BadVerbSelectedError)badVerbError).Token;
                    return !badVerb.StartsWith("--");
                }
            }

            return false;
        }

        private AliasOptions ParseAsAliasOption(IEnumerable<string> originalArguments, string verbToken)
        {
            var argumentList = CreateArgumensForAliasOption(originalArguments, verbToken);

            var parser = CreateParser();
            var result = parser.ParseArguments<AliasOptions>(argumentList);
            if (result.Tag == ParserResultType.Parsed)
            {
                return ((Parsed<AliasOptions>)result).Value;
            }

            RenderHelpText(result);
            return null;
        }

        private void RenderHelpText<T>(ParserResult<T> parserResult)
        {
            var helpText = HelpText.AutoBuild(parserResult);
            _outputWriter.WriteLine(helpText);
        }
    }
}