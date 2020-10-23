using System;
using Sodexo_JTH.ViewModels;
using Xamarin.Forms;

namespace Sodexo_JTH.Views
{
    public partial class HomeMasterDetailPage : MasterDetailPage
    {
        HomeMasterDetailPageViewModel _viewModel;
        public HomeMasterDetailPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as HomeMasterDetailPageViewModel;
            _viewModel.navigation = Navigation;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            
        }

        

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            
        }
        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            IsPresented = false;
            _viewModel.DrawerSelected((e as TappedEventArgs).Parameter.ToString());
           
        }
    }
}