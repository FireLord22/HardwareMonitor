using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwareMonitor.Models;
using System.Management;

namespace HardwareMonitor.Services
{
    public class NetworkMonitor
    {
        public List<NetworkAdapterInfo> GetAdapters()
        {
            var list = new List<NetworkAdapterInfo>();

            using (var searcher = new ManagementObjectSearcher(
                "SELECT Name, MACAddress, NetConnectionStatus, Speed FROM Win32_NetworkAdapter WHERE MACAddress IS NOT NULL"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    list.Add(new NetworkAdapterInfo
                    {
                        Name = obj["Name"]?.ToString(),
                        MacAddress = obj["MACAddress"]?.ToString(),
                        Status = obj["NetConnectionStatus"]?.ToString(),
                        Speed = obj["Speed"] != null ? long.Parse(obj["Speed"].ToString()) : 0
                    });
                }
            }

            return list;
        }
    }
}

