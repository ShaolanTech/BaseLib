using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShaolanTech.App.Hosts
{
    public abstract class ProcessLogger
    {
        public abstract Task Log(string processName, string msgType, string msg);
        
    }
}
