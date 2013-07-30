using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class Domain
    {
        public int id;
        public string description;
        public int color;
        public int x;
        public int y;
        public List<int> enemy = new List<int>();
    }
}
