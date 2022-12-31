using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShaolanTech.App.Hosts
{
    public static class ExecutableHostManager
    {
        static Dictionary<string, ExecutableHost> execItems = new Dictionary<string, ExecutableHost>();
        /// <summary>
        /// Load from json config
        /// </summary>
        /// <param name="configFile"></param>
        public static void LoadFromEnv()
        {

            var file = Environment.GetEnvironmentVariable("START_PROCESS_LIST");
            if (file.IsNotNullOrEmpty())
            {
                LoadFromFile(file);
            }
        }

        public static async Task WaitProcessStart(string displayname)
        {
            while (execItems.ContainsKey(displayname)==false)
            {
                await Task.Delay(1000);
            }
            await execItems[displayname].WaitStart();
        }
        public static void StopProcess(string displayName)
        {
            if(execItems.ContainsKey(displayName) == false)
            {
                return;
            }
            else
            {
                execItems[displayName].Dispose();
            }
        }
        public static void LoadFromFile(string file)
        {
            if (File.Exists(file))
            {
                Task.Run(async () =>
                { 
                    using (var sr = new StreamReader(file))
                    {
                        var items = sr.ReadToEnd().FromJsonString<List<ExecItem>>();
                        foreach (var item in items)
                        {
                            if (item.StartOrder.HasValue==false)
                            {
                                item.StartOrder = 0;
                            }
                            if (item.StartDelaySeconds.HasValue==false)
                            {
                                item.StartDelaySeconds = 0;
                            }
                        }
                        foreach (var item in items.OrderBy(i=>i.StartOrder))
                        {
                            if (item.StartDelaySeconds!=0)
                            {
                                await Task.Delay(TimeSpan.FromSeconds(item.StartDelaySeconds.Value));
                            }
                            var p = new ExecutableHost(item.FilePath, item.Arguments, item.UserName, item.WorkingDir,item.DisplayName);
                            p.Start();
                            execItems.Add(item.DisplayName,p);
                        }
                    } 
                });
            }
        }
    }
}
