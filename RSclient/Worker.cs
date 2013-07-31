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
                mainData.isDomainsLoad += mainData_isDomainsLoad;
                mainData.isLocationsLoad += mainData_isLocationsLoad;
                mainData.isNebulasLoad += mainData_isNebulasLoad;
                mainData.isPlanetsLoad += mainData_isPlanetsLoad;
                mainData.isItemsLoad += mainData_isItemsLoad;
                user.isLoadingComplite += user_isLoadingComplite;
                client.sendLogin(user.login, user.handler);
                user.isLogin = true;
                Thread receiver = new Thread(new ParameterizedThreadStart(new Receiver().doWork));
                ReceiverParams rp = new ReceiverParams(user, mainData);
                receiver.Name = @"Receiver: " + user.login;
                receiver.Start(rp);
                receiver.IsBackground = true;
                receiver.Join();
                bool state = receiver.IsAlive;
            }
        }

        void user_isLoadingComplite(object sender, EventArgs e)
        {
            client.sendCommand(new Command(Command.CList.loadComplite), user.handler);
            user.updateUserShip();
        }

        void mainData_isItemsLoad(object sender, EventArgs e)
        {
            client.sendCommand(new Command(Command.CList.getPlayerData), user.handler);
        }

        void mainData_isPlanetsLoad(object sender, EventArgs e)
        {
            if (!mainData.isLoaded&&!mainData.isItems&&!mainData.loadingItems)
            {
                mainData.loadingItems = true;
                client.sendCommand(new Command(Command.CList.getItems), user.handler);
            }
        }

        void user_isPasswordComplite(object sender, EventArgs e)
        {
            if (!mainData.isLoaded && !mainData.isDomains && !mainData.loadingDomains)
            {
                mainData.loadingDomains = true;
                client.sendCommand(new Command(Command.CList.getDomains), user.handler);
            }
        }

        private void user_isLoginComplite(object sender, EventArgs e)
        {
            client.sendPassword(user.password, user.handler);
        }

        private void mainData_isDomainsLoad(object sender, EventArgs e)
        {
            if (!mainData.isLoaded && !mainData.isLocations&&!mainData.loadingLocations)
            {
                mainData.loadingLocations = true;
                client.sendCommand(new Command(Command.CList.getLocations), user.handler);
            }
        }

        private void mainData_isLocationsLoad(object sender, EventArgs e)
        {
            if (!mainData.isLoaded && !mainData.isNebulas&&!mainData.loadingNebulas)
            {
                mainData.loadingNebulas = true;
                client.sendCommand(new Command(Command.CList.getNebulas), user.handler);
            }
        }

        private void mainData_isNebulasLoad(object sender, EventArgs e)
        {
            if (!mainData.isLoaded && !mainData.isPlanets&&!mainData.loadingPlanets)
            {
                mainData.loadingPlanets = true;
                for (int i = 0; i < mainData.locations.Count; i++)
                {
                    List<byte> rawData = new List<byte>();
                    rawData.AddRange(client.intToByteArray(mainData.locations.ElementAt(i).Value.id));
                    client.sendCommand(new Command(Command.CList.getPlanets, rawData.ToArray()), user.handler);
                }
                mainData.isPlanets = true;
            }
        }
    }
}
