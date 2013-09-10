using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RSclient
{
    public static class CoordTimer
    {
        private static Timer timer = new Timer(100);

        public static void prepareTimer()
        {
            timer.AutoReset = true;
            timer.Elapsed += elapsed;
            timer.Start();
        }

        private static void elapsed(object sender, ElapsedEventArgs e)
        {
            double unic_epox = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            for (int i = 0; i < MainData.users.Count; i++)
            {
                User user = MainData.users.ElementAt(i).Value;
                if (user.moveUser != null)
                {
                    if (user.inPlanet == null && user.moveUser.mStatus == MoveUser.MoveStatus.move)
                    {
                        double endMoveTime = user.moveUser.startMove + user.moveUser.flyTime;
                        if (endMoveTime > unic_epox)
                        {
                            double timeLeft = unic_epox - user.moveUser.startMove;
                            double timeProgress = timeLeft / user.moveUser.flyTime;
                            double ddX = user.moveUser.dX * timeProgress;
                            double ddY = user.moveUser.dY * timeProgress;
                            double ddF = user.moveUser.fuel * timeProgress;
                            user.x = (int)Math.Round(user.moveUser.x + ddX, 0);
                            user.y = (int)Math.Round(user.moveUser.y + ddY, 0);
                            user.userShip.energy = user.moveUser.startEnergy - ddF;
                        }
                        else
                        {
                            user.x = user.moveUser.targetX;
                            user.y = user.moveUser.targetY;
                            user.userShip.energy = user.moveUser.startEnergy - user.moveUser.fuel;
                            user.moveUser.mStatus = MoveUser.MoveStatus.stop;
                        }

                    }
                }
            }
        }
    }
}
