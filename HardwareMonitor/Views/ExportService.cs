using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace HardwareMonitor.Utils
{
    public class ExportService
    {
        public void ExportToTxt(string filePath, object data)
        {
            File.WriteAllText(filePath, data.ToString(), Encoding.UTF8);
        }

        public void ExportToJson(string filePath, object data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }

        public void ExportToCsv(string filePath, string csvContent)
        {
            File.WriteAllText(filePath, csvContent, Encoding.UTF8);
        }
    }
}
