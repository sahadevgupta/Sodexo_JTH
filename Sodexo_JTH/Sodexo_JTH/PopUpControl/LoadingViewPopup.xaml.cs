using Rg.Plugins.Popup.Pages;
using Sodexo_JTH.Views;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sodexo_JTH.PopUpControl
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingViewPopup : PopupPage
    {
        private float _progress;

        public float Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                prog.Progress = value;
                Debug.WriteLine(value);
            }
            
        }


        public LoadingViewPopup()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var a = this.Parent as HomeMasterDetailPage;
            (a.Detail as NavigationPage).CurrentPage.Opacity = 0.5;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var a = this.Parent as HomeMasterDetailPage;
            (a.Detail as NavigationPage).CurrentPage.Opacity = 1;
        }
        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}