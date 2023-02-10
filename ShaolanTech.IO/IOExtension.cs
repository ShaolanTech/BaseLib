using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace ShaolanTech.IO
{
    public static class IOExtensions
    {
        public static void WriteTableRow(this StreamWriter writer,params string[] items)
        {
            writer.WriteLine(string.Join("\t",items.Select (i=>string.IsNullOrEmpty(i)?"":i).Select(i=>i.Replace("\t","").Replace("\r","").Replace("\n",""))));
        }
        /// <summary>
        /// 使用Deflate压缩字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] CompressToBytes(this string s)
        {
            byte[] result = null;
            using (var output = new MemoryStream())
            {
                using (var stream = new DeflateStream(output, CompressionMode.Compress))
                {
                    StreamWriter sw = new StreamWriter(stream);
                    sw.Write(s);
                    sw.Flush();
                }
                result = output.ToArray();
            }
            return result;
        }
        /// <summary>
        /// 使用Deflate压缩字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static void CompressToSpan(this string s, out ReadOnlySpan<byte> span)
        {
            byte[] result = null;
            using (var output = new MemoryStream())
            {
                using (var stream = new DeflateStream(output, CompressionMode.Compress))
                {
                    var buffer = Encoding.UTF8.GetBytes(s);
                    stream.Write(buffer, 0, buffer.Length);

                }
                result = output.ToArray();
            }
            span = new ReadOnlySpan<byte>(result);
        }
        /// <summary>
        /// 使用Deflate解压缩字符串
        /// </summary>
        /// <param name="bufer"></param>
        /// <returns></returns>
        public static string DecompressFromBytes(this byte[] buffer)
        {
            string result = null;
            using (var stream = new MemoryStream(buffer))
            {
                using (var deflate = new DeflateStream(stream, CompressionMode.Decompress))
                {
                    StreamReader sr = new StreamReader(deflate);
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }
        /// <summary>
        /// 使用Deflate解压缩字符串
        /// </summary>
        /// <param name="bufer"></param>
        /// <returns></returns>
        public static string DecompressFromBytes(this ref ReadOnlySpan<byte> buffer)
        {
            return buffer.ToArray().DecompressFromBytes();
        }
        /// <summary>
        /// 使用Deflate压缩数据流
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Compress(this byte[] buffer)
        {
            byte[] result = null;
            using (var output = new MemoryStream())
            {
                using (var stream = new DeflateStream(output, CompressionLevel.Optimal))
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
                result = output.ToArray();
            }
            return result;
        }
        /// <summary>
        /// 使用Deflate压缩数据流
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="span">输出结果</param>
        public static void CompressToSpan(this ref ReadOnlySpan<byte> buffer, out ReadOnlySpan<byte> span)
        {
            byte[] result = null;
            using (var output = new MemoryStream())
            {
                using (var stream = new DeflateStream(output, CompressionMode.Compress))
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        stream.WriteByte(buffer[i]);
                    }
                }
                result = output.ToArray();
            }
            span = new ReadOnlySpan<byte>(result);
        }
        /// <summary>
        /// 使用Deflate解压缩数据流
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Decompress(this byte[] buffer)
        {

            byte[] result = null;
            using (var outStream = new MemoryStream())
            {
                using (var inStream = new MemoryStream(buffer))
                using (var deflate = new DeflateStream(inStream, CompressionMode.Decompress))
                {
                    deflate.CopyTo(outStream);
                }
                result = outStream.ToArray();
            }
            return result;
        }


    }
}
