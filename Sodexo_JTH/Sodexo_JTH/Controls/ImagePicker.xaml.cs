using Prism.Services;
using Sodexo_JTH.Events;
using Sodexo_JTH.Helpers;
using Sodexo_JTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sodexo_JTH.Controls
{
	//[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ImagePicker : ContentView
	{

        public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
       nameof(TitleText),
       typeof(string),
       typeof(ImagePicker),
       null,
       BindingMode.TwoWay);

        public string TitleText
        {
            get => (string)GetValue(TitleTextProperty);
            set => SetValue(TitleTextProperty, value);
        }
        public ImagePicker ()
		{
			InitializeComponent ();
            lbl.SetBinding(Label.TextProperty, new Binding(nameof(TitleText), BindingMode.TwoWay, source: this));
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var response = await App.pageDialog.DisplayActionSheetAsync("Select Language", "Cancel", null, "English", "Thai");
            if (response == "Cancel")
            {
                return;
            }
            else if (Library.KEY_USER_LANGUAGE != response)
            {
                lbl.Text = response;
                SelectionChanged?.Invoke(this, response);
            }
        }

        public event EventHandler<EventArgs<string>> SelectionChanged;
        
    }
}