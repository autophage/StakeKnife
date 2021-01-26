using StakeKnife.CommandLine.CommandProcessors;
using System;
using System.Collections.Generic;

namespace StakeKnife.CommandLine
{
    class Program
    {
        private static Repository repository;
        private static ICommandProcessor createProcessor;

        static void Main(string[] args)
        {
            var repository = new Repository(@"/StakeKnifeData/");

            InitializeCommandProcessors(repository);

            ProcessInput(args);

            repository.Save();
        }

        private static void InitializeCommandProcessors(Repository repository)
        {
            createProcessor = new CreateProcessor(repository);
        }

        private static void ProcessInput(string[] args)
        {
            Dictionary<string, Action> commandMappings = new Dictionary<string, Action>();

            commandMappings.Add("create", () => { createProcessor.Process(args); });

            Utils.CallCommand(args[0], commandMappings);
        }
    }
}
