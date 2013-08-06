using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class ThreadParams
    {
        public string login;
        public string password;

        public ThreadParams(string log, string pass)
        {
            this.login = log;
            this.password = pass;
        }

    }
}
