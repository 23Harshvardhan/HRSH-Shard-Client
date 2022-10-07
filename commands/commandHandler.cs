using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSH_Shard_Client.commands
{
    public class commandHandler
    {
        List<command> commands;

        public commandHandler()
        {
            this.commands = new List<command>();

            this.commands.Add(new beep("beep"));
            this.commands.Add(new renameLink("renamelink"));
        }

        public string runCommand(string cmd)
        {
            string[] sp = cmd.Split(' ');
            string name = sp.First();
            string[] args = sp.Skip(1).ToArray();

            foreach(command c in this.commands)
                if(c.name.ToLower() == name)
                    return c.execute(args);

            Program.LogEntry("Command " + cmd + " does not exist!");
            return "";
        }
    }
}
