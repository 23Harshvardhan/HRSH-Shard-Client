using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSH_Shard_Client.commands
{
    public class renameLink : command
    {
        public renameLink(string name) : base(name) { }

        public override string execute(string[] args)
        {
            if(args.Length == 1)
            {
                Program.LogEntry(args.First());
            }
            else
            {
                Program.LogEntry("Invalid argument for renameLink. Requires 1 argument(s).");
                return "";
            }

            Program.LogEntry("Renaming link to " + args.First());
            return "";
        }
    }
}
