using System;
using System.Globalization;
using System.Windows.Data;

namespace HardwareMonitor.Utils
{
    public class BytesToGbConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "0 GB";

            double bytes = System.Convert.ToDouble(value);
            double gb = bytes / 1024 / 1024 / 1024;
            return $"{gb:F2} GB";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}