using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareMonitor.Models
{
    public class CpuInfo
    {
        public string Name { get; set; }
        public int CoreCount { get; set; }
        public int ThreadCount { get; set; }
        public int BaseFrequency { get; set; }
        public double LoadPercentage { get; set; }
        public string Manufacturer { get; set; }
        public string Architecture { get; set; }
    }
}

