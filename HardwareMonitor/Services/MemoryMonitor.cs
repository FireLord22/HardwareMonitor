using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwareMonitor.Models;
using System;
using System.Management;

namespace HardwareMonitor.Services
{
    public class MemoryMonitor
    {
        public MemoryInfo GetMemoryInfo()
        {
            var memory = new MemoryInfo();

            using (var searcher = new ManagementObjectSearcher(
                "SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    long totalKb = Convert.ToInt64(obj["TotalVisibleMemorySize"]);
                    long freeKb = Convert.ToInt64(obj["FreePhysicalMemory"]);

                    memory.TotalMemoryBytes = totalKb * 1024;
                    memory.AvailableMemoryBytes = freeKb * 1024;
                    memory.UsagePercentage =
                        100.0 * (memory.TotalMemoryBytes - memory.AvailableMemoryBytes) / memory.TotalMemoryBytes;
                }
            }

            using (var searcher = new ManagementObjectSearcher(
                "SELECT Manufacturer, Capacity, Speed FROM Win32_PhysicalMemory"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    memory.Modules.Add(new MemoryModuleInfo
                    {
                        Manufacturer = obj["Manufacturer"]?.ToString(),
                        CapacityBytes = Convert.ToInt64(obj["Capacity"]),
                        Speed = Convert.ToInt32(obj["Speed"])
                    });
                }
            }

            return memory;
        }
    }
}

