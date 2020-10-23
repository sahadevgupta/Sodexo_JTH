using System;
using System.Globalization;
using Xamarin.Forms;

namespace Sodexo_JTH.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (value.ToString().ToLower() == "true")
            {
                //Color aa = (Color)Application.Current.Resources["Red"];
                return Color.Red;
            }
            else
            {
                Color asa = (Color)Application.Current.Resources["My_Header_Color"];
                return asa;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
