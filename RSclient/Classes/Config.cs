using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class Config
    {
        public int closeRadius = 3000;
        public int protocolVersion = 9;
        public double GlobalCoolDown = 1.0;
#if !DEBUG
        public IPAddress ipAddr = new IPAddress(new byte[] { 192, 168, 0, 105 });
        public int portListener = 7777;
        public string projectName = "Rania Star server";
#endif
#if DEBUG
        public IPAddress ipAddr = new IPAddress(new byte[] { 127, 0, 0, 1 });
        public int portListener = 7776;
        public string projectName = "Rania Star server DEBUG";
#endif
    }
}
