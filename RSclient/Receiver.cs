using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class Receiver
    {
        private User user = null;
        private AI ai = null;
        private bool isWork = true;

        public void doWork(object param)
        {
            if (param is ReceiverParams)
            {
                ReceiverParams p = (ReceiverParams)param;
                user = p.user;
                ai = p.ai;
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
                                Action action = ai.start;
                                action.BeginInvoke(null, null);
                                break;
                            }
                        case Command.CList.addUser:
                            {
                                Loader loader = new Loader();
                                User userAdd = loader.getAddUser(cmdReader, user);
                                user.usersClose.Add(userAdd.id, userAdd);
                                break;
                            }
                        case Command.CList.removeUser:
                            {
                                Loader loader = new Loader();
                                user.usersClose.Remove(loader.getRemoveUser(cmdReader));
                                break;
                            }
                        case Command.CList.Disconnect:
                            {
                                isWork = false;
                                break;
                            }
                    }
                }
            }
        }
    }
}
