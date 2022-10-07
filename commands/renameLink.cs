using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HRSH_Shard_Client.tools;

namespace HRSH_Shard_Client.commands
{
    public class renameLink : command
    {
        private static readonly string data = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\ssn\data.dat";

        IniFile dataIni = new IniFile(data);

        public renameLink(string name) : base(name) { }

        public override string execute(string[] args)
        {
            string oldName = dataIni.Read("usrName", "current");

            if(args.Length == 1)
            {
                if(oldName== args.First())
                {
                    Program.LogEntry("Command haulted. Old name and new name are same.");
                    return "";
                }
                else
                {
                    dataIni.Write("usrName", args.First(), "current");
                    Program.RenameLinkData(oldName, args.First());

                    Program.LogEntry("Renaming link to " + args.First());
                    return "";
                }
            }
            else
            {
                Program.LogEntry("Invalid argument for renamelink. Requires 1 argument(s).");
                return "";
            }
        }
    }
}
