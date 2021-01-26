using System;
using System.Collections.Generic;
using System.Text;

namespace StakeKnife.CommandLine.CommandProcessors
{
    interface ICommandProcessor
    {
        void Process(string[] args);
    }
}
