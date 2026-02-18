using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace HardwareMonitor.Utils
{
    public class ExportService
    {
        public void ExportToTxt(string path, object data)
        {
            File.WriteAllText(path, data.ToString(), Encoding.UTF8);
        }

        public void ExportToJson(string path, object data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(path, json, Encoding.UTF8);
        }
    }
}


