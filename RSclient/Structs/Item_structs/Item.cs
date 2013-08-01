using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public abstract class Item
    {
        public enum ItemType : int
        {
            none = 0,
            device = 1,
            consumable = 2
        }
        public int id;
        public ItemType itemType;
        public string description;
        public int volume;
        public int region;
        public int use_only;
        public int price;
    }
}
