using System;
using System.Collections.Generic;
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
