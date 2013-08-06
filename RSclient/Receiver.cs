using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RSclient
{
    public class Receiver
    {
        private User user = null;
        private bool isWork = true;

        public void doWork(object param)
        {
            if (param is ReceiverParams)
            {
                ReceiverParams p = (ReceiverParams)param;
                user = p.user;
            }
            
            user.timer.AutoReset = true;
            user.timer.Elapsed += (sender, e) => approxCoordUser(sender, e, user);
            if (user != null)
            {
                while (isWork)
                {
                    Command command = user.client.waitCommand(user.handler);
                    CommandReader cmdReader = new CommandReader(command);
                    switch (command.idCommand)
                    {
                        case Command.CList.LoginUser:
                            {
                                user.serverTime = cmdReader.getInt();
                                if (!MainData.isLoaded && !MainData.isDomains && !MainData.loadingDomains)
                                {
                                    MainData.loadingDomains = true;
                                    user.client.sendCommand(new Command(Command.CList.getDomains), user.handler);
                                }
                                break;
                            }
                        case Command.CList.FailLogin:
                            {
                                user.error = User.ErrorList.Password;
                                isWork = false;
                                break;
                            }
                        case Command.CList.FailVersion:
                            {
                                user.error = User.ErrorList.Protocol;
                                isWork = false;
                                break;
                            }
                        case Command.CList.getDomains:
                            {
                                MainData.domains = Loader.getDomains(cmdReader);
                                MainData.isDomains = true;
                                MainData.loadingDomains = false;
                                break;
                            }
                        case Command.CList.getNebulas:
                            {
                                MainData.nebulas = Loader.getNebulas(cmdReader);
                                MainData.isNebulas = true;
                                MainData.loadingNebulas = false;
                                break;
                            }
                        case Command.CList.getLocations:
                            {
                                Loader.getLocations(cmdReader, user);
                                for (int i = 0; i < user.locations.Count; i++)
                                {
                                    user.log += "LoadLocation: " + user.locations.ElementAt(i).Value.starName + "\r\n";
                                    if (!user.locations.ElementAt(i).Value.isLoadPlanet)
                                    {
                                        List<byte> rawData = new List<byte>();
                                        rawData.AddRange(user.client.intToByteArray(user.locations.ElementAt(i).Value.id));
                                        user.client.sendCommand(new Command(Command.CList.getPlanets, rawData.ToArray()), user.handler);
                                    }
                                }
                                break;
                            }
                        case Command.CList.getPlanets:
                            {
                                Loader.getPlanets(cmdReader, user);
                                break;
                            }
                        case Command.CList.touchUser:
                            {
                                user.timer.Stop();
                                user.moveUser = Loader.getTouchUser(cmdReader, user);
                                if (user.moveUser != null)
                                {
                                    if (user.usersClose.ContainsKey(user.moveUser.userId))
                                    {
                                        user.timer.Start();
                                    }
                                }
                                break;
                            }
                        case Command.CList.getItems:
                            {
                                MainData.itemCollect = Loader.getItems(cmdReader, user);
                                MainData.isItems = true;
                                MainData.loadingItems = false;
                                break;
                            }
                        case Command.CList.getPlayerData:
                            {
                                user = Loader.getUserData(cmdReader, user);
                                user.isLoadComplite = true;
                                Action action = user.ai.start;
                                action.BeginInvoke(null, null);
                                break;
                            }
                        case Command.CList.addUser:
                            {
                                User userAdd = Loader.getAddUser(cmdReader, user);
                                user.usersClose.Add(userAdd.id, userAdd);
                                Action action = user.ai.newObject;
                                action.BeginInvoke(null, null);
                                break;
                            }
                        case Command.CList.removeUser:
                            {
                                user.usersClose.Remove(Loader.getRemoveUser(cmdReader));
                                Action action = user.ai.removeObject;
                                action.BeginInvoke(null, null);
                                break;
                            }
                        case Command.CList.Disconnect:
                            {
                                isWork = false;
                                break;
                            }
                        case Command.CList.useEquips:
                            {
                                Loader.getUseEquip(cmdReader, user);
                                Action action = user.ai.newAction;
                                action.BeginInvoke(null, null);
                                break;
                            }
                    }
                }
            }
        }

        public void approxCoordUser(object sender, EventArgs e, User user)
        {
            User usr = user.usersClose[user.moveUser.userId];
            double unic_epox = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            double speed = usr.userShip.maxSpeed;
            int startX = user.moveUser.x;
            int startY = user.moveUser.y;
            int endX = user.moveUser.targetX;
            int endY = user.moveUser.targetY;
            double flyLength = Math.Sqrt(Math.Pow(endX - startX, 2) + Math.Pow(endY - startY, 2));
            double timeLenght = flyLength / speed;
            double dX = endX - startX;
            double dY = endY - startY;
            double dT = user.moveUser.startMove + timeLenght;
            double timeLeft = unic_epox - user.moveUser.startMove;
            if (timeLeft > dT)
            {
                usr.x = user.moveUser.targetX;
                usr.y = user.moveUser.targetY;
                Timer timer = (Timer)sender;
                timer.Stop();
                timer.Dispose();
            }
            else
            {
                double ddX = dX * (timeLeft / dT);
                double ddY = dY * (timeLeft / dT);
                usr.x = user.moveUser.x + (int)ddX;
                usr.y = user.moveUser.y + (int)ddY;
            }
        }
    }
}
