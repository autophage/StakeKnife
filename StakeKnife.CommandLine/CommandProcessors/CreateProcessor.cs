using StakeKnife.BackEnd;
using System;
using System.Collections.Generic;
using System.Text;
using static StakeKnife.BackEnd.Entities;

namespace StakeKnife.CommandLine.CommandProcessors
{
    class CreateProcessor : ICommandProcessor
    {
        private Repository repository;

        public CreateProcessor(Repository repository)
        {
            this.repository = repository;
        }

        public void Process(string[] args)
        {
            Dictionary<string, Action> commandMappings = new Dictionary<string, Action>();

            commandMappings.Add("address", () => { CreateAddress(args); });

            Utils.CallCommand(args[1], commandMappings);
        }

        private void CreateAddress(string[] args)
        {
            repository.Add(new Address(args[2], args[3], args[4], args[5], args[6]));
        }
    }
}
