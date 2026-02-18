using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareMonitor.Models
{
    public class NetworkAdapterInfo
    {
        public string Name { get; set; }
        public string MacAddress { get; set; }
        public string Status { get; set; }
        public long Speed { get; set; }
    }
}

