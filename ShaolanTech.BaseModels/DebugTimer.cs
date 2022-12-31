using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ShaolanTech
{
    public class DebugTimer : IDisposable
    {
        public DebugTimer(bool enable = true)
        {
            this.enable = enable;
        }
        DateTime time = DateTime.Now;
        private bool enable;
        public bool UseConsole { get; set; } = false;
        private void WriteLine(string message)
        {
            if (this.UseConsole)
            {
                Console.WriteLine(message);
            }
            else
            {
                Debug.WriteLine(message);
            }
        }
        public void CheckSpan(string title = "",bool reset=false)
        {
            if (this.enable)
            {
                WriteLine($"{title}:{(DateTime.Now - this.time).TotalMilliseconds}");
            }
            if (reset)
            {
                this.Reset();
            }
        }
        public void Reset()
        {
            this.time = DateTime.Now;
        }
        public void Dispose()
        {
            if (this.enable)
            {
                //Debug.WriteLine($"finished time span:{(DateTime.Now - this.time).TotalMilliseconds}");

            }
        }
    }
}
