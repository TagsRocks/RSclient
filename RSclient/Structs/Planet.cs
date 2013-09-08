using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class Planet
    {
        public int id;
        public string planetName;
        public int planetType;
        public Location parent;
        public int parentLocation;
        public int r_speed;
        public int orbit;
        public int radius;
        public int color;
        public int atmosphere;
        public Domain domain;
        public int atmosphere_speedX;
        public int atmosphere_speedY;
        public int price_coef;
        public List<int> services = new List<int>();
    }
}
