using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sodexo_JTH.Converters
{
    public class PlatformImageExtension : IMarkupExtension<string>
    {
        public string SourceImage { get; set; }
        public string ProvideValue(IServiceProvider serviceProvider)
        {
            if (SourceImage == null)
                return null;

            string imagePath;
            switch (Device.RuntimePlatform)
            {
                case Device.UWP:
                    imagePath = "Assets/" + SourceImage;
                    break;
                case Device.Android:
                case Device.iOS:
                    imagePath = SourceImage;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return imagePath;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }
    }

    public static class PlatFromImage
    {
        public static string GetImage(string SourceImage)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                case Device.iOS:
                    break;
                case Device.UWP:
                    SourceImage = "Assets/" + SourceImage;
                    break;
            }
            return SourceImage;
        }
    }

}
