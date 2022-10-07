using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSH_Shard_Client.commands
{
    public class beep : command
    {
        public beep(string name) : base(name) {}

        public override string execute(string[] args)
        {
            Program.LogEntry("Executing command beep.");

            if(args.Length == 0)
            {
                Console.Beep();

                return "";
            }
            else
            {
                if(args.Length == 1)
                {
                    int i = 0;
                    while (i != Int32.Parse(args.First()))
                    {
                        Console.Beep();
                        i++;
                    }

                    return "";
                }
                else
                {
                    Program.LogEntry("Invalid argument for beep. Requires either 0 or 1 argument(s).");
                    return "";
                }
            }
        }
    }
}
