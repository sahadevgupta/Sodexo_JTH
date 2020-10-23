using Sodexo_JTH.Helpers;
using System;
using Xamarin.Forms;

namespace Sodexo_JTH.Views
{
    public partial class FeedBackPage : ContentPage
    {
        public FeedBackPage()
        {
            InitializeComponent();
            string link = Library.KEY_USER_feedback_link;

            var uriBing = new Uri(link);

            // Launch the URI
            //Device.OpenUri(uriBing);
            Webview.Source = uriBing;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await progress.ProgressTo(0.9, 900, Easing.SpringIn);
        }
        protected void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            progress.IsVisible = true;
        }
        protected void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            progress.IsVisible = false;
        }


        private async void Image_Tapped(object sender, EventArgs e)
        {

            await App.Current.MainPage.Navigation.PopAsync();

           
        }
    }
}
