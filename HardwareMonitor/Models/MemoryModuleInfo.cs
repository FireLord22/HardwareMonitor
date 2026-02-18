using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareMonitor.Models
{
    public class MemoryModuleInfo
    {
        public string Manufacturer { get; set; }
        public long CapacityBytes { get; set; }
        public int Speed { get; set; }
    }
}

