using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ShaolanTech.Threading
{


    public class ByteLimitDualChannel 
    {

        private TaskCompletionSource<bool> readCompleteSignal;
        private long lengthLimit;
        private Func<object, long> getObjectLength;

        private Channel<object> channel;
        private Func<ChannelReader<object>, TaskCompletionSource<bool>, Task> readCallback;
        private long totalWriteLength = 0;




        public ByteLimitDualChannel(long byteLengthLimit, Func<object, long> getObjectLengthCallback, Func<ChannelReader<object>,   TaskCompletionSource<bool>,Task> readCallback)
        {

            this.lengthLimit = byteLengthLimit;
            this.getObjectLength = getObjectLengthCallback;
            this.channel = Channel.CreateBounded<object>(10);
            this.readCallback = readCallback;
        }
        private void ReinitReader()
        {
            if (this.readCompleteSignal != null)
            {
                Task.WaitAll(this.readCompleteSignal.Task);
            }
            this.readCompleteSignal = new TaskCompletionSource<bool>();
            Task.Run(async () => await this.readCallback(this.channel.Reader, this.readCompleteSignal));
        }
        static int index = 0;
        public async Task StartRead(Func<object> read)
        {
           
            await Task.Run(async () =>
            {
                this.ReinitReader();
                var item = read();
                while (item != null)
                {
                    if (this.totalWriteLength >= this.lengthLimit)
                    {
                        this.channel.Writer.Complete();
                        this.ReinitReader();
                    }
                    var objLength = this.getObjectLength(item);
                    await this.channel.Writer.WriteAsync(item);
                    index++;
                    Console.Write($"\rwrite count:{index}");
                    this.totalWriteLength += objLength;
                    item = read();
                }
                this.channel.Writer.Complete();
                await this.readCompleteSignal.Task;
            });
        }
       
    }
}
