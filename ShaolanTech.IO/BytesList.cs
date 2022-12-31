using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShaolanTech.IO
{
    public class BytesList : IDisposable
    {
        private MemoryStream strm = null;
        private BinaryWriter bw = null;
        /// <summary>
        /// Item length of this list
        /// </summary>
        public int Length { get; private set; }
        public void Add(byte[] buffer, bool compress)
        {
            this.Length++;
            if (this.strm == null)
            {
                this.strm = new MemoryStream();
                this.bw = new BinaryWriter(this.strm);
            }
            if (compress)
            {
                var compressedBuffer = buffer.Compress();
                this.bw.Write(compressedBuffer.Length);
                this.bw.Write(compressedBuffer, 0, compressedBuffer.Length);
            }
            else
            {
                this.bw.Write(buffer.Length);
                this.bw.Write(buffer, 0, buffer.Length);
            }

        }
        public byte[] ToBytes()
        {
            byte[] result = null;
            using (var ms = new MemoryStream())
            {
                this.bw.Flush();

                System.IO.BinaryWriter bw1 = new System.IO.BinaryWriter(ms);
                bw1.Write(this.Length);
                bw1.Flush();
                this.strm.Position = 0;
                this.strm.CopyTo(ms);
                result = ms.ToArray();
            }
            return result;
        }
        public static List<byte[]> ToByteList(byte[] buffer, bool compressed)
        {
            var result = new List<byte[]>();
            if (buffer != null && buffer.Length > 0)
            {
                unsafe
                {
                    fixed (byte* p = buffer)
                    {
                        byte* pHead = p;
                        BytesBinaryReader reader = new BytesBinaryReader(ref pHead);
                        int len = reader.ReadInt32();
                        for (int i = 0; i < len; i++)
                        {
                            var itemLen = reader.ReadInt32();
                            byte[] item = new byte[itemLen];
                            reader.Read(item, itemLen);
                            if (!compressed)
                            {
                                result.Add(item);
                            }
                            else
                            {
                                result.Add(item.Decompress());
                            }
                        }
                    }
                }
            }

            return result;
        }
        public static BytesList FromBytes(byte[] buffer, bool compressed)
        {
            BytesList result = new BytesList();

            using (var ms = new MemoryStream(buffer))
            {
                var br = new System.IO.BinaryReader(ms);
                var len = br.ReadInt32();
                result.Length = len;
                result.strm = new MemoryStream();
                result.bw = new BinaryWriter(result.strm);
                ms.CopyTo(result.strm);

            }

            return result;
        }

        public void Dispose()
        {
            if (this.bw != null)
            {
                this.bw.Dispose();
            }
            if (this.strm != null)
            {
                this.strm.Dispose();
            }

        }
    }
}
