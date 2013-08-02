using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    class ReceiverParams
    {
        public User user;
        public AI ai;

        public ReceiverParams(User user, AI ai)
        {
            this.user = user;
            this.ai = ai;
        }
    }
}
