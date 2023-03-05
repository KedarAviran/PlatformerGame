using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidProject
{
    public class ByteBuffer : IDisposable
    {
        private List<byte> buff;
        private byte[] readBuff;
        private int readPos;
        private bool buffUpdated = false;
        public ByteBuffer()
        {
            buff = new List<byte>();
            readPos = 0;
        }
        public int getReadPos()
        {
            return readPos;
        }
        public byte[] ToArray()
        {
            return buff.ToArray();
        }
        public int count()
        {
            return buff.Count;
        }
        public int lenght()
        {
            return count() - readPos;
        }
        public void clear()
        {
            buff.Clear();
            readPos = 0;
        }
        public void writeByte(byte input)
        {
            buff.Add(input);
            buffUpdated = true;
        }
        public void writeBytes(byte[] input)
        {
            buff.AddRange(input);
            buffUpdated = true;
        } 
        public void writeShort(short input)
        {
            buff.AddRange(BitConverter.GetBytes(input));
            buffUpdated = true;
        }
        public void writeInteger(int input)
        {
            buff.AddRange(BitConverter.GetBytes(input));
            buffUpdated = true;
        }
        public void writeLong(long input)
        {
            buff.AddRange(BitConverter.GetBytes(input));
            buffUpdated = true;
        }
        public void writeFloat(float input)
        {
            buff.AddRange(BitConverter.GetBytes(input));
            buffUpdated = true;
        }
        public void writeBool(bool input)
        {
            buff.AddRange(BitConverter.GetBytes(input));
            buffUpdated = true;
        }
        public void writeString(string input)
        {
            buff.AddRange(BitConverter.GetBytes(input.Length));
            buff.AddRange(Encoding.ASCII.GetBytes(input));
            buffUpdated = true;
        }

        public byte readByte(bool peek=true)
        {
            if(buff.Count>readPos)
            {
                if(buffUpdated)
                {
                    readBuff = buff.ToArray();
                    buffUpdated = false;
                }
                byte value = readBuff[readPos];
                if(peek&buff.Count>readPos)
                    readPos += 1;
                return value;
            }
            else
                throw new Exception("You are not trying to read out a byte");
        }
        public byte[]  readBytes(int length,bool peek = true)
        {
            if (buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = buff.ToArray();
                    buffUpdated = false;
                }
                byte[] value = buff.GetRange(readPos, length).ToArray();
                if (peek)
                    readPos += 1;
                return value;
            }
            else
                throw new Exception("You are not trying to read out a bytes");
        }
        public short readShort(bool peek = true)
        {
            if (buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = buff.ToArray();
                    buffUpdated = false;
                }
                short value = BitConverter.ToInt16(readBuff, readPos);
                if (peek & buff.Count>readPos)
                    readPos += 2;
                return value;
            }
            else
                throw new Exception("You are not trying to read out a short");
        }
        public int readInteger(bool peek = true)
        {
            if (buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = buff.ToArray();
                    buffUpdated = false;
                }
                int value = BitConverter.ToInt32(readBuff, readPos);
                if (peek & buff.Count > readPos)
                    readPos += 4;
                return value;
            }
            else
                throw new Exception("You are not trying to read out a int");
        }
        public long readLong(bool peek = true)
        {
            if (buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = buff.ToArray();
                    buffUpdated = false;
                }
                long value = BitConverter.ToInt64 (readBuff, readPos);
                if (peek & buff.Count > readPos)
                    readPos += 4;
                return value;
            }
            else
                throw new Exception("You are not trying to read out a long");
        }
        public float readFloat(bool peek = true)
        {
            if (buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = buff.ToArray();
                    buffUpdated = false;
                }
                float value = BitConverter.ToSingle(readBuff, readPos);
                if (peek & buff.Count > readPos)
                    readPos += 4;
                return value;
            }
            else
                throw new Exception("You are not trying to read out a float");
        }
        public bool readBool(bool peek = true)
        {
            if (buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = buff.ToArray();
                    buffUpdated = false;
                }
                bool value = BitConverter.ToBoolean(readBuff, readPos);
                if (peek & buff.Count > readPos)
                    readPos += 1;
                return value;
            }
            else
                throw new Exception("You are not trying to read out a bool");
        }
        public string readString(bool peek = true)
        {
            try
            {
                int length = readInteger(true);
                if (buffUpdated)
                {
                    readBuff = buff.ToArray();
                    buffUpdated = false;
                }
                string value = Encoding.ASCII.GetString(readBuff, readPos, length);
                if (peek & buff.Count > readPos)
                    if (value.Length > 0)
                        readPos += length; ;
                return value; 
            }
            catch(Exception)
            {
                throw new Exception("you are not trying to read out a string");
            }
        }
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                disposedValue = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }    
}
