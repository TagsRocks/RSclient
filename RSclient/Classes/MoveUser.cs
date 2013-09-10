using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class MoveUser
    {
        public enum MoveStatus : int
        {
            stop = 0,
            move = 1,
            dead = 2
        }
        public int userId;
        public int x;
        public int y;
        public int targetX;
        public int targetY;
        public int dX;
        public int dY;
        public double startMove;
        public double flyTime;
        public double fuel;
        public double startEnergy;
        public MoveStatus mStatus = MoveStatus.stop;
    }
}
