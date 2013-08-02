using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class Location
    {
        public int id;
        public string starName;
        public int starType;
        public int x;
        public int y;
        public int radius;
        public int domain;
        public Dictionary<int, Planet> planets;
        public bool isLoadPlanet = false;
    }
}
