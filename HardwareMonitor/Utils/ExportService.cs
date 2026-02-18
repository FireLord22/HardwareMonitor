using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Reflection;
using HardwareMonitor.ViewModels;

namespace HardwareMonitor.Utils
{
    public class ExportService
    {
        //TXT
        public void ExportToTxt(string path, MainViewModel vm)
        {
            var sb = new StringBuilder();

            WriteSection(sb, "CPU", vm.CpuInfo);
            WriteSection(sb, "GPU", vm.GpuInfo);
            WriteSection(sb, "RAM", vm.MemoryInfo);
            WriteCollection(sb, "RAM Modules", vm.MemoryInfo.Modules);
            WriteSection(sb, "Disks", vm.DiskInfo);
            WriteCollection(sb, "Logical Disks", vm.DiskInfo.LogicalDisks);
            WriteCollection(sb, "Physical Disks", vm.DiskInfo.PhysicalDisks);
            WriteCollection(sb, "Network", vm.NetworkAdapters);
            WriteSection(sb, "System", vm.SystemInfo);

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        //JSON
        public void ExportToJson(string path, MainViewModel vm)
        {
            var data = new
            {
                vm.CpuInfo,
                vm.GpuInfo,
                vm.MemoryInfo,
                vm.DiskInfo,
                vm.NetworkAdapters,
                vm.SystemInfo
            };

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(path, json, Encoding.UTF8);
        }

        //CSV
        public void ExportToCsv(string path, MainViewModel vm)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Section;Property;Value");

            WriteCsvObject(sb, "CPU", vm.CpuInfo);
            WriteCsvObject(sb, "GPU", vm.GpuInfo);
            WriteCsvObject(sb, "RAM", vm.MemoryInfo);
            WriteCsvCollection(sb, "RAM Module", vm.MemoryInfo.Modules);
            WriteCsvCollection(sb, "Logical Disk", vm.DiskInfo.LogicalDisks);
            WriteCsvCollection(sb, "Physical Disk", vm.DiskInfo.PhysicalDisks);
            WriteCsvCollection(sb, "Network", vm.NetworkAdapters);
            WriteCsvObject(sb, "System", vm.SystemInfo);

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        private void WriteSection(StringBuilder sb, string title, object obj)
        {
            sb.AppendLine($"{title.ToUpper()}");
            if (obj == null)
            {
                sb.AppendLine("Нет данных");
                sb.AppendLine();
                return;
            }

            foreach (var prop in obj.GetType().GetProperties())
            {
                var value = prop.GetValue(obj);
                sb.AppendLine($"{prop.Name}: {value}");
            }

            sb.AppendLine();
        }

        private void WriteCollection(StringBuilder sb, string title, System.Collections.IEnumerable collection)
        {
            sb.AppendLine($"{title.ToUpper()}");

            if (collection == null)
            {
                sb.AppendLine("Нет данных\n");
                return;
            }

            foreach (var item in collection)
            {
                foreach (var prop in item.GetType().GetProperties())
                {
                    var value = prop.GetValue(item);
                    sb.AppendLine($"{prop.Name}: {value}");
                }
                sb.AppendLine();
            }
        }

        private void WriteCsvObject(StringBuilder sb, string section, object obj)
        {
            if (obj == null) return;

            foreach (var prop in obj.GetType().GetProperties())
            {
                var value = prop.GetValue(obj);
                sb.AppendLine($"{section};{prop.Name};{value}");
            }
        }

        private void WriteCsvCollection(StringBuilder sb, string section, System.Collections.IEnumerable collection)
        {
            if (collection == null) return;

            foreach (var item in collection)
            {
                foreach (var prop in item.GetType().GetProperties())
                {
                    var value = prop.GetValue(item);
                    sb.AppendLine($"{section};{prop.Name};{value}");
                }
            }
        }
    }
}
