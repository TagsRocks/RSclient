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
        public MainData mainData;

        public ReceiverParams(User user, MainData mainData)
        {
            this.user = user;
            this.mainData = mainData;
        }
    }
}
