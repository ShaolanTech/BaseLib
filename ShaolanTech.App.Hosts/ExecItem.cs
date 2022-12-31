using System;
using System.Collections.Generic;
using System.Text;

namespace ShaolanTech.App.Hosts
{
    public class ExecItem
    {
        public string FilePath { get; set; }
        public string UserName { get; set; }
        public string Arguments { get; set; }
        public string WorkingDir { get; set; }

        public int? StartOrder { get; set; } = 0;

        public int? StartDelaySeconds { get; set; } = 0;
        public string DisplayName { get; set; }
    }
}
