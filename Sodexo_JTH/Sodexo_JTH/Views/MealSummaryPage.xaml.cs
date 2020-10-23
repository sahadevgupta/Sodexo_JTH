using Xamarin.Forms;

namespace Sodexo_JTH.Views
{
    public partial class MealSummaryPage : ContentPage
    {
        public MealSummaryPage()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                NavigationPage.SetHasBackButton(this, true);
            }
            else if (Device.RuntimePlatform == Device.Android)
                NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, true);
        }
    }
}
