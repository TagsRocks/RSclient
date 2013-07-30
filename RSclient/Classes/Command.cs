using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSclient
{
    public class Command
    {
        public enum CList : int
        {
            none = 0,
            LoginUser = 1,
            FailLogin = 2,
            getDomains = 3,
            Messages = 4,
            Disconnect = 5,
            PasswordUser = 6,
            getPlayerData = 7,
            getLocations = 8,
            getPlanets = 9,
            touchPlayer = 10,
            loadComplite = 11,
            addUser = 12,
            touchUser = 13,
            removeUser = 14,
            getNebulas = 15,
            getItems = 16,
            useEquips = 17,
            FailVersion = 18,
            setTarget = 19,
            userAction = 20
        }
        public CList idCommand = CList.none;
        public int length = 0;
        public byte[] data = new byte[0];
        public int controlCRC = 0;
        public Command(CList com)
        {
            this.idCommand = com;
        }
        public Command(CList com, byte[] data)
        {
            this.controlCRC = CRC(data);
            this.idCommand = com;
            this.length = data.Length;
            this.data = data;
        }
        private int CRC(byte[] data)
        {
            int res = 0;
            for (int i = 0; i < data.Length; i++)
            {
                res = res + (data[i] * (i));
            }
            return res;
        }

    }
}

