using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;

namespace HardwareMonitor.Utils
{
    public static class PerformanceHelper
    {
        private static readonly StringBuilder _log = new StringBuilder();

        public static void RunAllTests()
        {
            _log.Clear();
            WriteLine("=== HARDWARE MONITOR WMI BENCHMARK ===");
            WriteLine($"Дата: {DateTime.Now}");
            WriteLine("");

            // Процессор
            Test("Процессор - Win32_Processor",
                "SELECT Name, NumberOfCores, NumberOfLogicalProcessors, MaxClockSpeed, Manufacturer, Architecture FROM Win32_Processor");

            Test("Процессор - загрузка CPU",
                "SELECT PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name='_Total'");

            // GPU
            Test("ГПУ - Win32_VideoController",
                "SELECT Name, AdapterRAM FROM Win32_VideoController");

            Test("ГПУ - загрузка",
                "SELECT UtilizationPercentage FROM Win32_PerfFormattedData_GPUPerformanceCounters_GPUEngine");

            // Память
            Test("Память - Win32_OperatingSystem",
                "SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");

            Test("Память - модули",
                "SELECT Manufacturer, Capacity, Speed FROM Win32_PhysicalMemory");

            // Диски
            Test("Диски - физические",
                "SELECT Model, Size, MediaType FROM Win32_DiskDrive");

            Test("Диски - логические",
                "SELECT DeviceID, Size, FreeSpace, FileSystem FROM Win32_LogicalDisk WHERE DriveType=3");

            // Сеть
            Test("Сеть - адаптеры",
                "SELECT Name, MACAddress, NetConnectionStatus, Speed FROM Win32_NetworkAdapter WHERE MACAddress IS NOT NULL");

            // Система
            WriteLine("Система - Environment");
            var sw = Stopwatch.StartNew();
            var os = Environment.OSVersion.ToString();
            var arch = Environment.Is64BitOperatingSystem ? "x64" : "x86";
            var name = Environment.MachineName;
            var user = Environment.UserName;
            sw.Stop();

            WriteLine($"OS: {os}");
            WriteLine($"Архитектура: {arch}");
            WriteLine($"Имя машины: {name}");
            WriteLine($"Пользователь: {user}");
            WriteLine($"Замер занял: {sw.ElapsedMilliseconds} ms");

            WriteLine("");
            WriteLine("=== TEST COMPLETE ===");

            SaveToFile();
        }

        private static void Test(string name, string query)
        {
            const int iterations = 5;
            var times = new List<long>();

            // прогрев
            ExecuteQuery(query);

            for (int i = 0; i < iterations; i++)
            {
                var sw = Stopwatch.StartNew();
                ExecuteQuery(query);
                sw.Stop();
                times.Add(sw.ElapsedMilliseconds);
            }

            WriteLine($"{name,-45} Avg: {times.Average():F1} ms | Min: {times.Min()} | Max: {times.Max()}");
        }

        private static void ExecuteQuery(string query)
        {
            using (var searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject _ in searcher.Get()) { }
            }
        }

        private static void WriteLine(string line)
        {
            Console.WriteLine(line);
            _log.AppendLine(line);
        }

        private static void SaveToFile()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WmiBenchmarkLog.txt");
                File.WriteAllText(path, _log.ToString(), Encoding.UTF8);
                Console.WriteLine($"\nЛог сохранён в: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении лога: {ex.Message}");
            }
        }
    }
}
