using System;
using HardwareMonitor.Models;

namespace HardwareMonitor.Services
{
    public class SystemMonitor
    {
        public SystemInfo GetSystemInfo()
        {
            return new SystemInfo
            {
                OS = Environment.OSVersion.ToString(),
                Architecture = Environment.Is64BitOperatingSystem ? "x64" : "x86",
                MachineName = Environment.MachineName,
                UserName = Environment.UserName
            };
        }
    }
}

