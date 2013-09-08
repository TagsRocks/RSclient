using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace RSclient
{
    public class Client
    {
        public int protocolVersion = 12;

        public Command waitCommand(Socket handler)
        {
            byte[] bytesCom = new byte[4];
            byte[] bytesLen = new byte[4];
            byte[] controlCRClen = new byte[4];
            Command.CList com = Command.CList.none;
            int len = 0;
            handler.Receive(bytesCom);
            handler.Receive(controlCRClen);
            handler.Receive(bytesLen);
            com = (Command.CList)byteArrayToInt(bytesCom);
            int controlCRC = byteArrayToInt(controlCRClen);
            len = byteArrayToInt(bytesLen);
            int buffCount = (len / 1024) + 1;
            if (buffCount > 1024) { throw new OutOfMemoryException(); }
            List<ArraySegment<byte>> recvBuffers = new List<ArraySegment<byte>>(buffCount);
            for (int i = 0; i < buffCount; i++)
            {
                byte[] bigBuffer;
                if ((buffCount - 1) == i)
                {
                    bigBuffer = new byte[len - (buffCount - 1) * 1024];
                }
                else
                {
                    bigBuffer = new byte[1024];
                }
                recvBuffers.Add(new ArraySegment<byte>(bigBuffer));
            }
            if (len != 0) { handler.Receive(recvBuffers); }
            byte[] data = ConvertToByteArray(recvBuffers);
            Command cmd = new Command(com, data);
            cmd.controlCRC = controlCRC;
            return cmd;
        }
        public void sendCommand(Command command, Socket handler)
        {
            try
            {
                command.length = command.data.Length;
                handler.Send(intToByteArray((int)command.idCommand));
                handler.Send(intToByteArray(command.controlCRC));
                handler.Send(intToByteArray(command.length));
                handler.Send(command.data);
            }
            catch
            {
            }
        }

        #region Converters bytes<->Int
        public int byteArrayToInt(byte[] b)
        {
            return b[3] & 0xFF | (b[2] & 0xFF) << 8 | (b[1] & 0xFF) << 16 | (b[0] & 0xFF) << 24;
        }
        public byte[] intToByteArray(int a)
        {
            return new byte[] { (byte)((a >> 24) & 0xFF), (byte)((a >> 16) & 0xFF), (byte)((a >> 8) & 0xFF), (byte)(a & 0xFF) };
        }
        public byte[] ConvertToByteArray(IList<ArraySegment<byte>> list)
        {
            var bytes = new byte[list.Sum(asb => asb.Count)];
            int pos = 0;

            foreach (var asb in list)
            {
                Buffer.BlockCopy(asb.Array, asb.Offset, bytes, pos, asb.Count);
                pos += asb.Count;
            }

            return bytes;
        }
        #endregion

        #region SendCommands
        public void sendLogin(string login, Socket handler)
        {
            List<byte> rawData = new List<byte>();
            rawData.AddRange(intToByteArray(protocolVersion));
            byte[] loginArr = Encoding.Unicode.GetBytes(login);
            rawData.AddRange(intToByteArray(loginArr.Length));
            rawData.AddRange(loginArr);
            Command cmd = new Command(Command.CList.LoginUser, rawData.ToArray());
            sendCommand(cmd, handler);
        }
        public void sendPassword(string password, Socket handler)
        {
            List<byte> rawData = new List<byte>();
            byte[] passwordArr = Encoding.Unicode.GetBytes(password);
            rawData.AddRange(intToByteArray(passwordArr.Length));
            rawData.AddRange(passwordArr);
            Command cmd = new Command(Command.CList.PasswordUser, rawData.ToArray());
            sendCommand(cmd, handler);
        }
        public void sendDisconnect(Socket handler)
        {
            Command cmd = new Command(Command.CList.Disconnect);
            sendCommand(cmd, handler);
        }
        #endregion
    }
}

