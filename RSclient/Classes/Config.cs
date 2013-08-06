using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public static class Config
    {
        public static int closeRadius = 3000;
        public static int protocolVersion = 9;
        public static double GlobalCoolDown = 1.0;
#if !DEBUG
        public static IPAddress ipAddr = new IPAddress(new byte[] { 192, 168, 0, 105 });
        public static int portListener = 7777;
        public static string projectName = "Rania Star server";
#endif
#if DEBUG
        public static IPAddress ipAddr = new IPAddress(new byte[] { 127, 0, 0, 1 });
        public static int portListener = 7776;
        public static string projectName = "Rania Star server DEBUG";
#endif
    }
}
