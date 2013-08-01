﻿using System;
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
        public AI ai;

        public ReceiverParams(User user, MainData mainData, AI ai)
        {
            this.user = user;
            this.mainData = mainData;
            this.ai = ai;
        }
    }
}
