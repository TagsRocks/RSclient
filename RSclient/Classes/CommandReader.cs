using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class CommandReader
    {
        public int address;
        public byte[] data;
        public int controlCRC;
        public int crc;
        public bool endOfData;

        public CommandReader()
        {
            this.data = null;
            this.address = 0;
            this.controlCRC = 0;
            this.crc = 0;
            this.endOfData = false;
        }

        public CommandReader(Command cmd)
        {
            this.data = cmd.data;
            this.controlCRC = cmd.controlCRC;
            this.address = 0;
            this.crc = 0;
            this.endOfData = false;
        }

        public void delta(int delta)
        {
            for (int i = 0; i < delta; i++)
            {
                char b = (char)(this.data[this.address + i] & 0xFF);
                this.crc = this.crc + b * (this.address + i);
            }
            this.address += delta;
            if (this.address == this.data.Length)
            {
                this.endOfData = true;
            }
        }
        
        public int GetIntValue()
        {
            int Res = 0;
            byte[] Arr = new byte[4];
            Array.Copy(this.data, this.address, Arr, 0, 4);
            this.delta(4);
            Res = byteArrayToInt(Arr);
            return Res;
        }
        public String GetStringValue(int SL)
        {
            String Res = "";
            byte[] Arr = new byte[SL];
            Array.Copy(this.data, this.address, Arr, 0, SL);
            this.delta(SL);
            Res = Encoding.Unicode.GetString(Arr);
            return Res;
        }
        private int byteArrayToInt(byte[] b)
        {
            return b[3] & 0xFF | (b[2] & 0xFF) << 8 | (b[1] & 0xFF) << 16 | (b[0] & 0xFF) << 24;
        }
    }
}
