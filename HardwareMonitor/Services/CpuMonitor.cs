using System;
using HardwareMonitor.Models;
using System.Management;

namespace HardwareMonitor.Services
{
    public class CpuMonitor
    {
        public CpuInfo GetCpuInfo()
        {
            var cpu = new CpuInfo();

            string query = "SELECT Name, NumberOfCores, NumberOfLogicalProcessors, MaxClockSpeed, Manufacturer, Architecture FROM Win32_Processor";

            using (var searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    cpu.Name = obj["Name"]?.ToString();
                    cpu.CoreCount = Convert.ToInt32(obj["NumberOfCores"]);
                    cpu.ThreadCount = Convert.ToInt32(obj["NumberOfLogicalProcessors"]);
                    cpu.BaseFrequency = Convert.ToInt32(obj["MaxClockSpeed"]);
                    cpu.Manufacturer = obj["Manufacturer"]?.ToString();

                    int arch = Convert.ToInt32(obj["Architecture"]);
                    cpu.Architecture = arch == 9 ? "x64" : "x86";
                    break;
                }
            }

            cpu.LoadPercentage = GetCpuLoad();
            return cpu;
        }

        private double GetCpuLoad()
        {
            using (var searcher = new ManagementObjectSearcher(
                "SELECT PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name='_Total'"))
            {
                foreach (ManagementObject obj in searcher.Get())
                    return Convert.ToDouble(obj["PercentProcessorTime"]);
            }

            return 0;
        }
    }
}


