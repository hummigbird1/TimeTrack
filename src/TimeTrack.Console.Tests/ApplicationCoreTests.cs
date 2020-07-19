using Microsoft.Extensions.Configuration;
using System;
using TimeTrack.Console.Configuration;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;
using Xunit;

namespace TimeTrack.Console.Tests
{
    public class ApplicationCoreTests
    {
        [Fact]
        public void Query()
        {
            Func<ICommandOption, int> assertionHandler = opt =>
            {
                Assert.Equal(Commands.Query, opt.Command);
                var topt = Assert.IsType<QueryOptions>(opt);
                Assert.Null(topt.From);
                Assert.Null(topt.To);
                Assert.Null(topt.RequestType);
                return (int)ExitCodes.Successful;
            };
            var output = new MockOutputWriter();
            var core = CreateApplicationCoreTestInstance(output, assertionHandler);
            var result = core.ExecuteCommandByArguments(new[] { "query" }, out _);
            Assert.Equal((int)ExitCodes.Successful, result);
            Assert.Empty(output.Lines);
        }

        [Fact]
        public void Query_WithFrom()
        {
            Func<ICommandOption, int> assertionHandler = opt =>
           {
               Assert.Equal(Commands.Query, opt.Command);
               var topt = Assert.IsType<QueryOptions>(opt);
               Assert.Equal("today", topt.From);
               Assert.Null(topt.To);
               Assert.Null(topt.RequestType);
               return (int)ExitCodes.Successful;
           };

            var output = new MockOutputWriter();
            var core = CreateApplicationCoreTestInstance(output, assertionHandler);
            var result = core.ExecuteCommandByArguments(new[] { "query", "--From", "today" }, out _);
            Assert.Equal((int)ExitCodes.Successful, result);
            Assert.Empty(output.Lines);
        }

        private ApplicationCore CreateApplicationCoreTestInstance(IOutputWriter outputWriter, Func<ICommandOption, int> mockHandleFunction)
        {
            var commandOptionProvider = new CommandOptionProvider(); // The real thing! Not a Mock!
            var cmdParser = new CommandlineArgumentParser(commandOptionProvider, outputWriter); // The real thing! Not a Mock!

            var configurationManager = new MockConfigurationManager() { ConfigurationPath = "In Memory Dummy Configuration" };
            var logger = new MockLogger();
            var selectTypeServiceProvider = new MockSelectorTypeServiceProvider(new MockCommandHandler(mockHandleFunction));
            return new ApplicationCore(configurationManager, logger, selectTypeServiceProvider, cmdParser);
        }

        private class MockCommandHandler : IHandler<ICommandOption, int>
        {
            private readonly Func<ICommandOption, int> mockHandleFunction;

            public MockCommandHandler(Func<ICommandOption, int> mockHandleFunction)
            {
                this.mockHandleFunction = mockHandleFunction;
            }

            public int Handle(ICommandOption argument)
            {
                return mockHandleFunction(argument);
            }
        }

        private class MockConfigurationManager : IConfigurationManager
        {
            public Configuration.Configuration Configuration { get; set; } = new Configuration.Configuration();

            public string ConfigurationPath { get; set; }

            public IConfiguration ConfigurationRoot => throw new NotImplementedException();

            public Configuration.Configuration LoadConfigurationFile(string configurationFilePath)
            {
                throw new NotImplementedException();
            }

            public void SetConfigurationFilePath(string configurationFilePath)
            {
            }
        }

        private class MockLogger : ILogger
        {
            public bool Enabled { get; set; }

            public void Verbose(string s)
            {
            }
        }

        private class MockSelectorTypeServiceProvider : ISelectorTypeServiceProvider<Commands, IHandler<ICommandOption, int>>
        {
            private readonly IHandler<ICommandOption, int> mockHandler;

            public MockSelectorTypeServiceProvider(IHandler<ICommandOption, int> mockHandler)
            {
                this.mockHandler = mockHandler;
            }

            public IHandler<ICommandOption, int> this[Commands key] => mockHandler;
        }
    }
}