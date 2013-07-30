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
        private MainData mainData = null;
        public bool isWork = true;

        public void doWork(object param)
        {
            if (param is ReceiverParams)
            {
                ReceiverParams p = (ReceiverParams)param;
                user = p.user;
                mainData = p.mainData;
            }
            if (user != null)
            {
                Client client = new Client();
                while (isWork)
                {
                    Command command = client.waitCommand(user.handler);
                    CommandReader cmdReader = new CommandReader(command);
                    switch (command.idCommand)
                    {
                        case Command.CList.LoginUser:
                            {
                                user.isLogin = true;
                                break;
                            }
                        case Command.CList.PasswordUser:
                            {
                                user.serverTime = cmdReader.GetIntValue();
                                user.isPassword = true;
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
                                mainData.domains = loader.getDomains(cmdReader);
                                mainData.isDomains = true;
                                break;
                            }
                        case Command.CList.getNebulas:
                            {
                                Loader loader = new Loader();
                                mainData.nebulas = loader.getNebulas(cmdReader);
                                mainData.isNebulas = true;
                                break;
                            }
                        case Command.CList.getLocations:
                            {
                                Loader loader = new Loader();
                                mainData.locations = loader.getLocations(cmdReader, mainData);
                                mainData.isLocations = true;
                                break;
                            }
                        case Command.CList.getPlanets:
                            {
                                Loader loader = new Loader();
                                mainData.planets = loader.getPlanets(cmdReader, mainData);
                                break;
                            }
                        case Command.CList.getItems:
                            {
                                Loader loader = new Loader();
                                mainData.itemCollect = loader.getItems(cmdReader, mainData);
                                mainData.isItems = true;
                                break;
                            }
                    }
                }
            }
        }
    }
}
