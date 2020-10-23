using System;
using System.Globalization;
using Xamarin.Forms;

namespace Sodexo_JTH.Converters
{
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value.ToString().ToLower();
            string returnvalue = "";

            if (input == "true" || input == null)
                returnvalue = "False";
            else
                returnvalue = "True";

            return returnvalue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
