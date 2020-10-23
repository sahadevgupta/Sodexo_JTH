using Sodexo_JTH.Helpers;
using Sodexo_JTH.UWP.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly:ExportRenderer(typeof(ContentPage), typeof(CustomPageRenderer))]
namespace Sodexo_JTH.UWP.Renderers
{
    public class CustomPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            this.PointerMoved += CustomPageRenderer_PointerMoved;
            this.KeyDown += CustomPageRenderer_KeyDown;
            this.KeyUp += CustomPageRenderer_KeyUp;
        }

        private async void CustomPageRenderer_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (SessionManager.Instance.IsSessionActive)
            {
                SessionManager.Instance.EndTrackSession();
                await SessionManager.Instance.StartTrackSessionAsync();
            }
        }

        private async void CustomPageRenderer_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (SessionManager.Instance.IsSessionActive)
            {
                SessionManager.Instance.EndTrackSession();
                await SessionManager.Instance.StartTrackSessionAsync();
            }
        }

        private async void CustomPageRenderer_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (SessionManager.Instance.IsSessionActive)
            {
                SessionManager.Instance.EndTrackSession();
                await SessionManager.Instance.StartTrackSessionAsync();
            }

           
        }
    }
}
