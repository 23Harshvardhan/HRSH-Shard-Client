using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSH_Shard_Client.commands
{
    public class command
    {
        public string name;

        public command(string name) { this.name = name; }

        public virtual string execute(string[] args) { return ""; }
    }
}
