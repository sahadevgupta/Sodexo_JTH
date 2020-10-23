using Sodexo_JTH.Controls;
using Sodexo_JTH.UWP.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]

namespace Sodexo_JTH.UWP.Renderers
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntry ExtendedEntryElement => Element as ExtendedEntry;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    Control.Style = App.Current.Resources["FormTextBoxStyle"] as Windows.UI.Xaml.Style;
                }

                //Control.Loaded -= OnControlLoaded;
               // Control.Loaded += OnControlLoaded;
            }
        }

       

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                //Control.Loaded -= OnControlLoaded;
            }

            base.Dispose(disposing);
        }

        

        
    }
}
