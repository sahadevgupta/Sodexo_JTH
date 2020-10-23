using System;
using System.Globalization;
using Xamarin.Forms;

namespace Sodexo_JTH.Converters
{
    public class AllergytextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "No";
            }

            if (value.ToString() == "" || value.ToString() == "NA" || value.ToString() == "0" || value.ToString() == "NULL")
            {
                return "No";
            }
            else
            {
                return "Yes";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
