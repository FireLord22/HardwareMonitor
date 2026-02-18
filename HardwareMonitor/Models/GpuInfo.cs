using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareMonitor.Models
{
    public class GpuInfo
    {
        public string Name { get; set; }
        public long MemoryBytes { get; set; }
        public double LoadPercentage { get; set; }
    }
}

