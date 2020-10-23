using System;
using System.Globalization;
using Xamarin.Forms;

namespace Sodexo_JTH.Converters
{
    public class BoolToObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;


            if (parameter != null)
            {
                switch (parameter)
                {
                    case "VegNVeg":
                        {
                            if (System.Convert.ToBoolean(value))
                            {
                                return "Veg";
                            }
                            else
                                return "Non-Veg";

                        }

                    case "HNH":
                        {
                            if (System.Convert.ToBoolean(value))
                            {
                                return "Halal";
                            }
                            else
                                return "Non-Halal";
                        }

                    case "Disposable":
                        {
                            if (System.Convert.ToBoolean(value))
                            {
                                return "Yes";
                            }
                            else
                                return "No";
                        }
                }
            }

            if (value.ToString() == "True")
            {
                return "H";
            }
            else
            {
                return "NH";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();
            if (value.ToString() == "Non-Veg" || value.ToString() == "Non-Halal" || value.ToString() == "No")
                return false;
            else
                return true;
        }
    }
}
