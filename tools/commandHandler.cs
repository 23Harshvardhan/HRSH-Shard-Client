using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSH_Shard_Client.tools
{
    internal class commandHandler
    {
        static List<string> cmd = new List<string>();
        
        public static void loadCommands()
        {
            cmd.Add("ping");
        }
    }
}
