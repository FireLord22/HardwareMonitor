using System;
using HardwareMonitor.Models;
using System.Management;

namespace HardwareMonitor.Services
{
    public class DiskMonitor
    {
        public DiskInfo GetDiskInfo()
        {
            var diskInfo = new DiskInfo();

            using (var searcher = new ManagementObjectSearcher(
                "SELECT Model, Size, MediaType FROM Win32_DiskDrive"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    diskInfo.PhysicalDisks.Add(new PhysicalDiskInfo
                    {
                        Model = obj["Model"]?.ToString(),
                        Manufacturer = obj["Model"]?.ToString(),
                        SizeBytes = Convert.ToInt64(obj["Size"] ?? 0),
                        MediaType = obj["MediaType"]?.ToString()
                    });
                }
            }

            using (var searcher = new ManagementObjectSearcher(
                "SELECT DeviceID, Size, FreeSpace, FileSystem FROM Win32_LogicalDisk WHERE DriveType=3"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    diskInfo.LogicalDisks.Add(new LogicalDiskInfo
                    {
                        DriveLetter = obj["DeviceID"]?.ToString(),
                        TotalSizeBytes = Convert.ToInt64(obj["Size"] ?? 0),
                        FreeSizeBytes = Convert.ToInt64(obj["FreeSpace"] ?? 0),
                        FileSystem = obj["FileSystem"]?.ToString()
                    });
                }
            }

            return diskInfo;
        }
    }
}

