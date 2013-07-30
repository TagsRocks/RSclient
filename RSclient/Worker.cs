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
        Client client = new Client();
        User user = new User();
        MainData mainData = null;

        public void doWork(object param)
        {
            if (param is ThreadParams)
            {
                ThreadParams p = (ThreadParams)param;
                user.login = p.login;
                user.password = p.password;
                mainData = p.mainData;
            }
            if (user.login != "")
            {

                user.handler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                user.handler.Connect(config.ipAddr, config.portListener);
                user.isLogin = false;
                user.isLoginComplite += user_isLoginComplite;
                user.isPasswordComplite += user_isPasswordComplite;
                client.sendLogin(user.login, user.handler);
                Thread receiver = new Thread(new ParameterizedThreadStart(new Receiver().doWork));
                ReceiverParams rp = new ReceiverParams(user, mainData);
                receiver.Name = @"Receiver: " + user.login;
                receiver.Start(rp);
                receiver.IsBackground = true;
                
                while (true) { }
            }
        }

        void user_isPasswordComplite(object sender, EventArgs e)
        {
            if (!mainData.isLoaded)
            {
                if (!mainData.isDomains)
                {
                    mainData.isDomainsLoad += mainData_isDomainsLoad;
                    client.sendCommand(new Command(Command.CList.getDomains), user.handler);
                }
            }
        }

        private void user_isLoginComplite(object sender, EventArgs e)
        {
            client.sendPassword(user.password, user.handler);
        }

        private void mainData_isDomainsLoad(object sender, EventArgs e)
        {
            if (!mainData.isLoaded)
            {
                if (!mainData.isLocations)
                {
                    mainData.isLocationsLoad += mainData_isLocationsLoad;
                    client.sendCommand(new Command(Command.CList.getLocations), user.handler);
                }
            }
        }

        private void mainData_isLocationsLoad(object sender, EventArgs e)
        {
            if (!mainData.isLoaded)
            {
                if (!mainData.isNebulas)
                {
                    mainData.isNebulasLoad += mainData_isNebulasLoad;
                    client.sendCommand(new Command(Command.CList.getNebulas), user.handler);
                    client.sendCommand(new Command(Command.CList.getItems), user.handler);
                }
            }
        }

        private void mainData_isNebulasLoad(object sender, EventArgs e)
        {
            if (!mainData.isLoaded)
            {
                if (!mainData.isPlanets)
                {
                    for (int i = 0; i < mainData.locations.Count; i++)
                    {
                        List<byte> rawData = new List<byte>();
                        rawData.AddRange(client.intToByteArray(mainData.locations[i].id));
                        client.sendCommand(new Command(Command.CList.getPlanets, rawData.ToArray()), user.handler);
                    }
                    mainData.isPlanets = true;
                }
            }
        }

        private void mainData_isItemsLoad(object sender, EventArgs e)
        {
            client.sendCommand(new Command(Command.CList.getPlayerData), user.handler);
        }
    }
}
