using System;
using System.Collections.Generic;
using System.Text;

namespace ShaolanTech.IO
{
    public unsafe class BytesBinaryReader
    {
        int pos = 0;
        private byte* array;
        public BytesBinaryReader(ref byte* input)
        {
            this.array = input;
        }
        public void ReadPointer(ref byte* ptr, int count)
        {
            ptr = this.array;
            ptr += this.pos;
            this.pos += count;
        }
        public byte ReadByte()
        {
            byte result = this.array[this.pos];
            this.pos++;
            return result;
        }
        public int ReadInt16()
        {

            short result = (short)(this.array[this.pos] | (this.array[this.pos + 1] << 8));
            this.pos += 2;
            return result;
        }
        public int ReadInt32()
        {
            int result = this.array[this.pos] | (this.array[this.pos + 1] << 8) | (this.array[this.pos + 2] << 16) | (this.array[this.pos + 3] << 24);
            this.pos += 4;
            return result;
        }
        public long ReadInt64()
        {
            uint num = (uint)(this.array[this.pos + 0] | (this.array[this.pos + 1] << 8) | (this.array[this.pos + 2] << 16) | (this.array[this.pos + 3] << 24));
            uint num2 = (uint)(this.array[this.pos + 4] | (this.array[this.pos + 5] << 8) | (this.array[this.pos + 6] << 16) | (this.array[this.pos + 7] << 24));
            long result = (long)(((ulong)num2 << 32) | num);
            this.pos += 8;
            return result;
        }
        public void Read(byte[] buffer, int count)
        {
            for (int i = 0; i < count; i++)
            {
                buffer[i] = this.array[this.pos + i];
            }
            this.pos += count;
        }

    }
}
