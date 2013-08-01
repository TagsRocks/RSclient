using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class Consumable : Item
    {
        public enum Type : int
        {
            None = 0,
            FuelID = 1
        }
    }
}
