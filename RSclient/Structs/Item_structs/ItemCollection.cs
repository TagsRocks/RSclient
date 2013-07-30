using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class ItemCollection
    {
        public Dictionary<int, Item> bodyes = new Dictionary<int, Item>();
        public Dictionary<int, Item> droids = new Dictionary<int, Item>();
        public Dictionary<int, Item> engines = new Dictionary<int, Item>();
        public Dictionary<int, Item> fuelbags = new Dictionary<int, Item>();
        public Dictionary<int, Item> hypers = new Dictionary<int, Item>();
        public Dictionary<int, Item> radars = new Dictionary<int, Item>();
        public Dictionary<int, Item> shields = new Dictionary<int, Item>();
        public Dictionary<int, Item> weapons = new Dictionary<int, Item>();
        public Dictionary<int, Item> consumables = new Dictionary<int, Item>();

        public int count
        {
            get
            {
                return 9;
            }
        }

        public T get<T>(int id) where T : Item
        {
            T dev = null;
            if (typeof(T) == typeof(Body))
            {
                dev = this.bodyes[id] as T;
            }
            if (typeof(T) == typeof(Droid))
            {
                dev = this.droids[id] as T;
            }
            if (typeof(T) == typeof(Engine))
            {
                dev = this.engines[id] as T;
            }
            if (typeof(T) == typeof(Fuelbag))
            {
                dev = this.fuelbags[id] as T;
            }
            if (typeof(T) == typeof(Hyper))
            {
                dev = this.hypers[id] as T;
            }
            if (typeof(T) == typeof(Radar))
            {
                dev = this.radars[id] as T;
            }
            if (typeof(T) == typeof(Shield))
            {
                dev = this.shields[id] as T;
            }
            if (typeof(T) == typeof(Weapon))
            {
                dev = this.weapons[id] as T;
            }
            if (typeof(T) == typeof(Consumable))
            {
                dev = this.consumables[id] as T;
            }
            return dev;
        }
    }
}
