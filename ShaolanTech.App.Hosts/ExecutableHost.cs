using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ShaolanTech.App.Hosts
{
    /// <summary>
    /// Process container
    /// </summary>
    public class ExecutableHost : IDisposable
    {
        private string filePath;
        private string arguments;
        private string userName;
        private Process process = null;
        private string workingDir;
        private string displayName = null;
        private bool started = false;
        /// <summary>
        /// ProcessLogger subclass imp
        /// </summary>
        public static LogPipe LogPipe { get; set; } = null;

        public ExecutableHost(string filePath, string arguments = null, string userName = null, string workingDir = null,string displayName=null)
        {
            if (displayName.IsNotNullOrEmpty())
            {
                this.displayName = displayName;
            }
            else
            {
                this.displayName = Path.GetFileNameWithoutExtension(filePath);
            }
            this.filePath = filePath;
            this.arguments = arguments;
            this.userName = userName;
            this.workingDir = workingDir;
        }
        public async Task WaitStart()
        {
            while (this.started==false)
            {
                await Task.Delay(1000);
            }
        }
        
        public void Start()
        {
            Task.Run(async () =>
            {
                try
                {
                    var processName = Path.GetFileNameWithoutExtension(this.filePath);
                    ProcessStartInfo info = new ProcessStartInfo();
                    info.WorkingDirectory = this.workingDir;
                    info.FileName = this.filePath;
                    info.Arguments = arguments;
                    info.UserName = userName;
                    info.UseShellExecute = false;
                    info.RedirectStandardOutput = true;
                    info.RedirectStandardError = true;
                    //info.CreateNoWindow = true;
                    this.process = new Process();
                    this.process.StartInfo = info;
                    //this.process.OutputDataReceived += (s, e) =>
                    //{
                    //    Console.WriteLine($"{processName}--{e.Data}");
                    //    if (LogPipe != null && e.Data.IsNotNullOrEmpty())
                    //    {
                    //        LogPipe.Write(processName, "Info", e.Data).Wait();
                    //    }
                    //};
                    //this.process.ErrorDataReceived += (s, e) =>
                    //{
                    //    Console.WriteLine($"{processName}--{e.Data}");
                    //    if (LogPipe != null && e.Data.IsNotNullOrEmpty())
                    //    {
                    //        LogPipe.Write(processName, "Error", e.Data).Wait();
                    //    }
                    //};
                    //var t = this.RedirectOutput();
                    //Task.Run(() =>
                    //{

                    this.process.Start();
                    this.started = true;
                    //Console.WriteLine(processName + "--started");
                    await this.RedirectOutput();
                    //});


                    //Console.WriteLine($"{this.filePath} started");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(this.filePath + ex.Message);

                }
            });



        }
        public async Task RedirectOutput()
        {
            try
            {
                if (LogPipe != null)
                {
                    //while (this.process == null)
                    //{
                    //    await Task.Delay(1000);
                    //}
                    //var processName = Path.GetFileNameWithoutExtension(this.filePath);
                    var strm = this.process.StandardOutput;
                    var line = strm.ReadLine();
                    while (line != null)
                    {
                        //Console.WriteLine($"{processName}--{line}");
                        if (line.IsNotNullOrEmpty())
                        {
                            await LogPipe.Write(this.displayName, "Info", line);
                            //await Logger.Log(processName, "Info", line);
                        }
                        line = strm.ReadLine();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Dispose()
        {
            if (this.process != null)
            {
                try
                {
                    this.process.Kill();
                    this.process.Dispose();
                }
                catch (Exception ex)
                {


                }

            }
        }



    }
}
