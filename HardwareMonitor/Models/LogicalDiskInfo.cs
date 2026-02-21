using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareMonitor.Models
{
    public class LogicalDiskInfo
    {
        public string DriveLetter { get; set; }
        public long TotalSizeBytes { get; set; }
        public long FreeSizeBytes { get; set; }
        public string FileSystem { get; set; }
        public string Name => DriveLetter;
        public long FreeSpaceBytes => FreeSizeBytes;
    }
}

