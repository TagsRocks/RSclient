using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class Weapon : Device
    {
        public enum WeaponType : int
        {
            Laser = 0,
            Rocket = 1,
            BFG = 2
        }
        public WeaponType weaponType;
        public int radius;
        public int power;
        public int time_start;
        public int time_reload;
    }
}
