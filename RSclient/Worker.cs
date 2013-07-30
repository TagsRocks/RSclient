using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RSclient
{
    public class Worker
    {
        Config config = new Config();

        public void doWork(object param)
        {
            Client client = new Client();
            User user = new User();
            MainData mainData = null;
            if (param is ThreadParams)
            {
                ThreadParams p = (ThreadParams)param;
                user.login = p.login;
                user.password = p.password;
                mainData = p.mainData;
            }
            if (user.login != "")
            {
                try
                {
                    user.handler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    user.handler.Connect(config.ipAddr, config.portListener);
                    client.sendLogin(user.login, user.handler);
                    client.sendPassword(user.password, user.handler);
                    
                    
                    Thread receiver = new Thread(new ParameterizedThreadStart(new Receiver().doWork));
                    ReceiverParams rp = new ReceiverParams(user, mainData);
                    receiver.Name = @"Receiver: "+user.login;
                    receiver.Start(rp);
                    receiver.IsBackground = true;
                    while (user.isLogin) { }
                    lock (mainData)
                    {
                        if (!mainData.isLoaded)
                        {
                            client.sendCommand(new Command(Command.CList.getDomains), user.handler);
                            //client.sendCommand(new Command(Command.CList.getNebulas), user.handler);
                            //client.sendCommand(new Command(Command.CList.getLocations), user.handler);
                            //client.sendCommand(new Command(Command.CList.getPlanets), user.handler);
                            //client.sendCommand(new Command(Command.CList.getItems), user.handler);
                        }
                    }
                }
                catch (Exception e) { }
            }
        }
    }
}
