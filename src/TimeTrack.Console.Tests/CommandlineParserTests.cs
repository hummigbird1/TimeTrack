using TimeTrack.Console.Options;
using Xunit;

namespace TimeTrack.Console.Tests
{
    public class CommandlineParserTests
    {
        [Fact]
        public void AliasParse_Existing_WithCommonOptions()
        {
            var outputWriter = new MockOutputWriter();
            var parser = new CommandlineArgumentParser(new CommandOptionProvider(), outputWriter);
            var args = new[] { "MyAlias", "--config", "bla bla", "-v" };
            var parseResult = parser.TryParseArguments(args, out var result);
            Assert.True(parseResult);
            var aliasOpt = Assert.IsType<AliasOptions>(result);
            Assert.Equal("MyAlias", aliasOpt.Alias);
            Assert.True(result.Verbose);
            Assert.Equal("bla bla", result.ConfigurationPath);
            Assert.Empty(outputWriter.Lines);
        }

        [Fact]
        public void AliasParse_Existing_WithoutOtherOptions()
        {
            var outputWriter = new MockOutputWriter();
            var parser = new CommandlineArgumentParser(new CommandOptionProvider(), outputWriter);
            var args = new[] { "MyAlias" };
            var parseResult = parser.TryParseArguments(args, out var result);
            Assert.True(parseResult);
            var aliasOpt = Assert.IsType<AliasOptions>(result);
            Assert.Equal("MyAlias", aliasOpt.Alias);
            Assert.False(result.Verbose);
            Assert.Null(result.ConfigurationPath);
            Assert.Empty(outputWriter.Lines);
        }
    }
}