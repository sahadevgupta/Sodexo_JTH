using Sodexo_JTH.Helpers;
using Sodexo_JTH.ViewModels;
using Xamarin.Forms;

namespace Sodexo_JTH.Views
{
    public partial class PatientInformationPage : ContentPage
    {
        PatientInformationPageViewModel _viewModel;
        public PatientInformationPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as PatientInformationPageViewModel;
            if (Device.RuntimePlatform == Device.iOS)
            {
                NavigationPage.SetHasBackButton(this, true);
            }
           else if (Device.RuntimePlatform == Device.Android)
                NavigationPage.SetHasBackButton(this, false); 

            NavigationPage.SetHasNavigationBar(this, true);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel._Navigation = Navigation;
            //RadioGroup.CheckedChanged += RadioGroup_CheckedChanged;
        }

        private void RadioGroup_CheckedChanged(object sender, int e)
        {
            Library.IsFAGeneralEnable = e == 1 ? false : true;
        }
    }
}
