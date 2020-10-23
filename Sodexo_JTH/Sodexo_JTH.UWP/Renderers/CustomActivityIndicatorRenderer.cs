using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Sodexo_JTH.UWP.Renderers;
using SolidColorBrush = Windows.UI.Xaml.Media.SolidColorBrush;

[assembly: ExportRenderer(typeof(ActivityIndicator), typeof(CustomActivityIndicatorRenderer))]
namespace Sodexo_JTH.UWP.Renderers
{
    public class CustomActivityIndicatorRenderer : ViewRenderer<ActivityIndicator, ProgressRing>
    {
        private ProgressRing _progressRing;
        protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicator> e)
        {
            base.OnElementChanged(e);
            if (Control == null && e.NewElement == null) return;
            _progressRing = new ProgressRing
            {
                IsActive = this.Element.IsRunning,
                Height = 40,
                Width= 40,
                Visibility = this.Element.IsRunning ? Visibility.Visible : Visibility.Collapsed,
                Foreground = Color.FromHex("#33446c").ToUwpSolidColorBrush()
            };
            SetNativeControl(_progressRing);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(ActivityIndicator.IsRunning))
            {
                _progressRing.IsActive = this.Element.IsRunning;
                _progressRing.Visibility = this.Element.IsRunning ? Visibility.Visible : Visibility.Collapsed;
                //   Window.Current.CoreWindow.PointerCursor = this.Element.IsRunning ? new CoreCursor(CoreCursorType.Wait, 10) : new CoreCursor(CoreCursorType.Arrow, 10);
            }
            
        }

    }
    public static class Extensions
    {
        public static Windows.UI.Color ToUwPColor(this Xamarin.Forms.Color color)
        {
            return Windows.UI.Color.FromArgb((byte)(color.A * 255.0), (byte)(color.R * 255.0), (byte)(color.G * 255.0), (byte)(color.B * 255.0));

        }

        public static SolidColorBrush ToUwpSolidColorBrush(this Xamarin.Forms.Color color)
        {
            return new SolidColorBrush(color.ToUwPColor());
        }
    }
}