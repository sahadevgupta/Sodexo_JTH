using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace Sodexo_JTH.Converters
{
    public class BytesToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource imgSource = null;

            if (value is string img)
            { 
                
            }
            byte[] FileName = value as byte[];
            if (FileName != null)
            {


                var stream1 = new MemoryStream(FileName);
                imgSource = ImageSource.FromStream(() => stream1);
                return imgSource;
            }
            else
            {

                imgSource = (FileImageSource)PlatFromImage.GetImage("no_image.png");
                return imgSource;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
