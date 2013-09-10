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
        User user = new User();
        public void doWork(object param)
        {
            if (param is ThreadParams)
            {
                ThreadParams p = (ThreadParams)param;
                user.login = p.login;
                user.password = p.password;
            }
            if (user.login != "")
            {
                user.handler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                user.handler.Connect(Config.ipAddr, Config.portListener);
                user.isLogin = false;
                user.isLoginComplite += user_isLoginComplite;
                user.isPasswordComplite += user_isPasswordComplite;
                MainData.isDomainsLoad += mainData_isDomainsLoad;
                MainData.isNebulasLoad += mainData_isNebulasLoad;
                MainData.isItemsLoad += mainData_isItemsLoad;
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
            lock (MainData.users)
            {
                MainData.users.Add(user.id, user);
            }
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
            if (!MainData.isLoaded && !MainData.isNebulas && !MainData.loadingNebulas)
            {
                MainData.loadingNebulas = true;
                user.client.sendCommand(new Command(Command.CList.getNebulas), user.handler);
            }
        }

        private void mainData_isNebulasLoad(object sender, EventArgs e)
        {
            if (!MainData.isLoaded && !MainData.isItems && !MainData.loadingItems)
            {
                MainData.loadingItems = true;
                user.client.sendCommand(new Command(Command.CList.getItems), user.handler);
            }
        }
    }
}
