using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareMonitor.Models
{
    public class PhysicalDiskInfo
    {
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public long SizeBytes { get; set; }
        public string MediaType { get; set; }
    }
}

