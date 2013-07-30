using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class Loader
    {
        public Dictionary<int, Domain> getDomains(CommandReader cr)
        {
            Dictionary<int, Domain> res = new Dictionary<int,Domain>();
            int domainCount = cr.GetIntValue();
            for (int i = 0; i < domainCount; i++)
            {
                Domain dom = new Domain();
                dom.id = cr.GetIntValue();
                dom.color = cr.GetIntValue();
                dom.description = cr.GetStringValue(cr.GetIntValue());
                dom.x = cr.GetIntValue();
                dom.y = cr.GetIntValue();
                res.Add(dom.id, dom);
            }
            return res;
        }
    }
}
