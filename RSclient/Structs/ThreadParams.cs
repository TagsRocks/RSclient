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
        public MainData mainData;

        public ThreadParams(string log, string pass, MainData mainData)
        {
            this.login = log;
            this.password = pass;
            this.mainData = mainData;
        }

    }
}
