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
                                break;
                            }
                        case Command.CList.FailLogin:
                            {
                                user.isLogin = false;
                                isWork = false;
                                break;
                            }
                        case Command.CList.FailVersion:
                            {
                                user.isLogin = false;
                                isWork = false;
                                break;
                            }
                        case Command.CList.getDomains:
                            {
                                Loader loader = new Loader();
                                mainData.domains = loader.getDomains(cmdReader);
                                break;
                            }
                        case Command.CList.getNebulas:
                            {
                                break;
                            }
                        case Command.CList.getLocations:
                            {
                                break;
                            }
                        case Command.CList.getPlanets:
                            {
                                break;
                            }
                        case Command.CList.getItems:
                            {
                                break;
                            }
                    }
                }
            }
        }
    }
}
