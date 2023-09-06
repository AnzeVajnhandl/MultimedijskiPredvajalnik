using MultimedijskiPredvajalnik.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MultimedijskiPredvajalnik
{
    class UnknownConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "Unknown";
            }

            string genre = value.ToString();
            IEnumerable<string> zvrsti = Settings.Default.Zvrsti.Cast<string>();

            if (zvrsti.Contains(genre))
            {
                return genre;
            }
            else
            {
                return "Unknown";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
