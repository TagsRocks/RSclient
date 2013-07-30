using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class Device : Item
    {
        public enum DeviceType : int
        {
            None = 0,
            Body = 1,
            Engine = 2,
            Fuelbag = 3,
            Droid = 4,
            Shield = 5,
            Hyper = 6,
            Radar = 7,
            Weapon = 8
        }
        public Planet vendor;
        public DeviceType deviceType;
        public string vendorStr;
        public int durability;
    }
}
