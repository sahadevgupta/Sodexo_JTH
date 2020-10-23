using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sodexo_JTH.PopUpControl
{
	//[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingScreen : ContentView
	{
        public static readonly BindableProperty LoadingTextProperty = BindableProperty.Create(
        nameof(LoadingText),
        typeof(string),
        typeof(LoadingScreen),
        null,
        BindingMode.TwoWay);

        public string LoadingText
        {
            get => (string)GetValue(LoadingTextProperty);
            set => SetValue(LoadingTextProperty, value);
        }

        public static readonly BindableProperty ProgressProperty = BindableProperty.Create(
        nameof(Progress),
        typeof(double),
        typeof(LoadingScreen),
        0.0,
        BindingMode.TwoWay);

        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }
        public LoadingScreen ()
		{
			InitializeComponent ();
            ProgressB.SetBinding(ProgressBar.ProgressProperty, new Binding(nameof(Progress), BindingMode.TwoWay, source: this));
            label.SetBinding(Label.TextProperty, new Binding(nameof(LoadingText), BindingMode.TwoWay, source: this));
        }
	}
}