using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

namespace HardwareMonitor.Models
{
    public class DiskInfo
    {
        public ObservableCollection<PhysicalDiskInfo> PhysicalDisks { get; set; }
            = new ObservableCollection<PhysicalDiskInfo>();

        public ObservableCollection<LogicalDiskInfo> LogicalDisks { get; set; }
            = new ObservableCollection<LogicalDiskInfo>();
    }
}


