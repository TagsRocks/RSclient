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
                                MoveUser moveUser = Loader.getTouchUser(cmdReader, user);
                                if (moveUser.userId == user.id)
                                {
                                    user.moveUser = moveUser;
                                    user.moveUser.mStatus = MoveUser.MoveStatus.move;
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
                                if (!MainData.users.ContainsKey(userAdd.id))
                                {
                                    lock (MainData.users)
                                    {
                                        MainData.users.Add(userAdd.id, userAdd);
                                    }
                                }
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
    }
}
