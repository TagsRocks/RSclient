using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class MainData
    {
        public Dictionary<int, Domain> domains = new Dictionary<int,Domain>();
        public Dictionary<int, Location> locations = new Dictionary<int,Location>();
        public Dictionary<int, Nebula> nebulas = new Dictionary<int,Nebula>();
        public Dictionary<int, Planet> planets = new Dictionary<int,Planet>();
        public ItemCollection itemCollect = new ItemCollection();
        public bool isLoaded = false;
    }
}
