using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ShaolanTech.App.Hosts
{
    public class LogPipeData
    {
        public string Channel { get; set; }
        public int LogLevel { get; set; }
        public string Message { get; set; }
        public DateTime LogTime { get; set; }
        public string Type { get; set; }
        public byte[] ToBytes()
        {
            byte[] result = null;
            using (var ms=new MemoryStream())
            {
                var bw = new BinaryWriter(ms);
                bw.Write(this.Channel);
                bw.Write(this.LogLevel);
                bw.Write(this.Message);
                bw.Write(this.LogTime.ToString("yyyy-MM-dd HH:mm:ss"));
                bw.Write(this.Type);
                bw.Flush();
                result = ms.ToArray();
            }
            return result;
        }
        public static LogPipeData FromBytes(byte[] buffer)
        {
            LogPipeData result= new LogPipeData();
            using (var ms=new MemoryStream(buffer))
            {
                var br = new BinaryReader(ms);
                result.Channel = br.ReadString();
                result.LogLevel = br.ReadInt32();
                result.Message = br.ReadString();
                result.LogTime = br.ReadString().TryParseDateTime();
                result.Type = br.ReadString();
            }
            return result;
        }
    }
    public class LogPipe
    {
        private static Channel<LogPipeData> channel;
        private LogPipe()
        {
             
        }

        private static LogPipe instance = null;
        public static LogPipe Create()
        {
            if (instance==null)
            {
                instance = new LogPipe();
            }
            return instance;
        }
        static LogPipe()
        {
            channel = Channel.CreateUnbounded<LogPipeData>(); 
        }
        public ChannelReader<LogPipeData> GetReader()
        {
            return channel.Reader;
        }
        public async Task Write(string msgchannel,string msgType, string message)
        {
            await channel.Writer.WriteAsync(new LogPipeData { Channel = msgchannel,Type=msgType, LogLevel = 0, Message = message, LogTime = DateTime.Now });
        }
    }
}
