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
                                if (!user.mainData.isLoaded && !user.mainData.isDomains && !user.mainData.loadingDomains)
                                {
                                    user.mainData.loadingDomains = true;
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
                                Loader loader = new Loader();
                                user.mainData.domains = loader.getDomains(cmdReader);
                                user.mainData.isDomains = true;
                                user.mainData.loadingDomains = false;
                                break;
                            }
                        case Command.CList.getNebulas:
                            {
                                Loader loader = new Loader();
                                user.mainData.nebulas = loader.getNebulas(cmdReader);
                                user.mainData.isNebulas = true;
                                user.mainData.loadingNebulas = false;
                                break;
                            }
                        case Command.CList.getLocations:
                            {
                                Loader loader = new Loader();
                                loader.getLocations(cmdReader, user);
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
                                Loader loader = new Loader();
                                loader.getPlanets(cmdReader, user);
                                break;
                            }
                        case Command.CList.touchUser:
                            {
                                Loader loader = new Loader();
                                MoveUser moveUser = loader.getTouchUser(cmdReader, user);
                                if (moveUser != null)
                                {
                                    if (user.usersClose.ContainsKey(moveUser.userId))
                                    {
                                        Timer timer = new Timer(100);
                                        timer.AutoReset = true;
                                        timer.Elapsed += (sender, e) => approxCoordUser(sender, e, user.usersClose[moveUser.userId], moveUser);
                                    }
                                }
                                break;
                            }
                        case Command.CList.getItems:
                            {
                                Loader loader = new Loader();
                                user.mainData.itemCollect = loader.getItems(cmdReader, user);
                                user.mainData.isItems = true;
                                user.mainData.loadingItems = false;
                                break;
                            }
                        case Command.CList.getPlayerData:
                            {
                                Loader loader = new Loader();
                                user = loader.getUserData(cmdReader, user);
                                user.isLoadComplite = true;
                                Action action = user.ai.start;
                                action.BeginInvoke(null, null);
                                break;
                            }
                        case Command.CList.addUser:
                            {
                                Loader loader = new Loader();
                                User userAdd = loader.getAddUser(cmdReader, user);
                                user.usersClose.Add(userAdd.id, userAdd);
                                Action action = user.ai.newObject;
                                action.BeginInvoke(null, null);
                                break;
                            }
                        case Command.CList.removeUser:
                            {
                                Loader loader = new Loader();
                                user.usersClose.Remove(loader.getRemoveUser(cmdReader));
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
                                Loader loader = new Loader();
                                loader.getUseEquip(cmdReader, user);
                                Action action = user.ai.newAction;
                                action.BeginInvoke(null, null);
                                break;
                            }
                    }
                }
            }
        }

        public void approxCoordUser(object sender, EventArgs e, User usr, MoveUser moveUser)   // при многократном вызове возможна утечка памяти. проверить под нагрузкой.
        {
            double unic_epox = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            double speed = usr.userShip.maxSpeed;
            int startX = moveUser.x;
            int startY = moveUser.y;
            int endX = moveUser.targetX;
            int endY = moveUser.targetY;
            double flyLength = Math.Sqrt(Math.Pow(endX - startX, 2) + Math.Pow(endY - startY, 2));
            double timeLenght = flyLength / speed;
            double dX = endX - startX;
            double dY = endY - startY;
            double dT = moveUser.startMove + timeLenght;
            double timeLeft = unic_epox - moveUser.startMove;
            if (timeLeft > dT)
            {
                usr.x = moveUser.targetX;
                usr.y = moveUser.targetY;
                Timer timer = (Timer)sender;
                timer.Stop();
                timer.Dispose();
            }
            else
            {
                double ddX = dX * (timeLeft / dT);
                double ddY = dY * (timeLeft / dT);
                usr.x = moveUser.x + (int)ddX;
                usr.y = moveUser.y + (int)ddY;
            }
        }
    }
}
