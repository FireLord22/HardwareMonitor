using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace HardwareMonitor.Models
{
    public class MemoryInfo
    {
        public long TotalMemoryBytes { get; set; }
        public long AvailableMemoryBytes { get; set; }
        public double UsagePercentage { get; set; }

        public ObservableCollection<MemoryModuleInfo> Modules { get; set; }
            = new ObservableCollection<MemoryModuleInfo>();
    }
}


