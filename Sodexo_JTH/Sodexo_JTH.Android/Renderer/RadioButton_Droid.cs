using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Sodexo_JTH.Controls;
using Sodexo_JTH.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Sodexo_JTH.Controls.RadioButton), typeof(RadioButton_Droid))]
namespace Sodexo_JTH.Droid.Renderer
{
    public class RadioButton_Droid : ViewRenderer<Sodexo_JTH.Controls.RadioButton, Android.Widget.RadioButton>
    {
        public RadioButton_Droid(Context context) : base(context)
        {

        }
       
        protected override void OnElementChanged(ElementChangedEventArgs<Sodexo_JTH.Controls.RadioButton> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged += ElementOnPropertyChanged;

            }

            if (this.Control == null)
            {
                var radButton = new Android.Widget.RadioButton(this.Context);
                radButton.CheckedChange += RadButton_CheckedChange;
                this.SetNativeControl(radButton);
            }

            Control.Text = e.NewElement.Text;
            Control.Checked = e.NewElement.Checked;

            Element.PropertyChanged += ElementOnPropertyChanged;
        }

        private void RadButton_CheckedChange(object sender, Android.Widget.CompoundButton.CheckedChangeEventArgs e)
        {
            this.Element.Checked = e.IsChecked;
        }

        void ElementOnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Checked":
                    Control.Checked = Element.Checked;
                    break;
                case "Text":
                    Control.Text = Element.Text;
                    break;
            }
        }
    }
}