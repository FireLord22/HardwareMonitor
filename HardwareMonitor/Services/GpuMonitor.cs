using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwareMonitor.Models;
using System.Management;

namespace HardwareMonitor.Services
{
    public class GpuMonitor
    {
        public GpuInfo GetGpuInfo()
        {
            var gpu = new GpuInfo();

            using (var searcher = new ManagementObjectSearcher(
                "SELECT Name, AdapterRAM FROM Win32_VideoController"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    gpu.Name = obj["Name"]?.ToString();
                    gpu.MemoryBytes = Convert.ToInt64(obj["AdapterRAM"] ?? 0);
                    break;
                }
            }

            gpu.LoadPercentage = GetGpuLoad();
            return gpu;
        }

        private double GetGpuLoad()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher(
                    "SELECT UtilizationPercentage FROM Win32_PerfFormattedData_GPUPerformanceCounters_GPUEngine"))
                {
                    double total = 0;
                    int count = 0;

                    foreach (ManagementObject obj in searcher.Get())
                    {
                        total += Convert.ToDouble(obj["UtilizationPercentage"] ?? 0);
                        count++;
                    }

                    return count > 0 ? total / count : 0;
                }
            }
            catch { return 0; }
        }
    }
}

