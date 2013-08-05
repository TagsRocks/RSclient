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
        User user = new User();
        public void doWork(object param)
        {
            if (param is ThreadParams)
            {
                ThreadParams p = (ThreadParams)param;
                user.login = p.login;
                user.password = p.password;
                user.mainData = p.mainData;
            }
            if (user.login != "")
            {
                user.handler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                user.handler.Connect(config.ipAddr, config.portListener);
                user.isLogin = false;
                user.isLoginComplite += user_isLoginComplite;
                user.isPasswordComplite += user_isPasswordComplite;
                user.mainData.isDomainsLoad += mainData_isDomainsLoad;
                user.mainData.isNebulasLoad += mainData_isNebulasLoad;
                user.mainData.isItemsLoad += mainData_isItemsLoad;
                user.isLoadingComplite += user_isLoadingComplite;
                user.client.sendLogin(user.login, user.handler);
                user.isLogin = true;
                Thread receiver = new Thread(new ParameterizedThreadStart(new Receiver().doWork));
                ReceiverParams rp = new ReceiverParams(user);
                receiver.Name = @"Receiver: " + user.login;
                receiver.Start(rp);
                receiver.IsBackground = true;
            }
        }

        private void user_isLoadingComplite(object sender, EventArgs e)
        {
            user.client.sendCommand(new Command(Command.CList.loadComplite), user.handler);
            user.updateUserShip();
        }

        private void mainData_isItemsLoad(object sender, EventArgs e)
        {
            user.client.sendCommand(new Command(Command.CList.getPlayerData), user.handler);
        }

        private void user_isPasswordComplite(object sender, EventArgs e)
        {
            
        }

        private void user_isLoginComplite(object sender, EventArgs e)
        {
            user.client.sendPassword(user.password, user.handler);
        }

        private void mainData_isDomainsLoad(object sender, EventArgs e)
        {
            if (!user.mainData.isLoaded && !user.mainData.isNebulas && !user.mainData.loadingNebulas)
            {
                user.mainData.loadingNebulas = true;
                user.client.sendCommand(new Command(Command.CList.getNebulas), user.handler);
            }
        }

        private void mainData_isNebulasLoad(object sender, EventArgs e)
        {
            if (!user.mainData.isLoaded && !user.mainData.isItems && !user.mainData.loadingItems)
            {
                user.mainData.loadingItems = true;
                user.client.sendCommand(new Command(Command.CList.getItems), user.handler);
            }
        }
    }
}
