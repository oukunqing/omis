using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMIS.Server.Common
{
    public class ServerPort
    {
        public int Port { get; set; }
        public string Server { get; set; }

        public ServerPort()
        {
            this.Port = 0;
            this.Server = string.Empty;
        }

    }
}