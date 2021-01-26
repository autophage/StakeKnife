using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StakeKnife.CommandLine
{
    sealed internal class Utils
    {
        public static void CallCommand(string command, Dictionary<string, Action> mappings)
        {
            if (mappings.ContainsKey(command.ToLower()))
            {
                var action = mappings.GetValueOrDefault(command.ToLower());

                action.Invoke();
            } else
            {
                Console.WriteLine("Invalid command: " + command);
            }
        }
    }
}
