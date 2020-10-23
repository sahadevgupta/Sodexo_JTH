
using Sodexo_JTH.Helpers;
using Sodexo_JTH.Interfaces;
using Sodexo_JTH.Models;
using Sodexo_JTH.Resources;
using Sodexo_JTH.ViewModels;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Sodexo_JTH.Views
{
    public partial class LoginPage : ContentPage
    {
        LoginPageViewModel _viewModel;
        public LoginPage()
        {
            InitializeComponent();
            
            _viewModel = BindingContext as LoginPageViewModel;
            _viewModel.navigation = Navigation;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //languagePicker.SelectedItem = "English";
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
           // this.txtusername.EntryUnfocused -= Txtusername_EntryUnfocused;

        }

        private async void Txtusername_EntryUnfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(_viewModel.UserName))
            {
                var result = await _viewModel.BindRole();
                if (result == null)
                {
                    this.Txtusername.Focus();
                    return;
                }
                else if (result.Any())
                {
                    _viewModel.EnableSubmitButton = true;
                    if (result.Count == 1)
                    {
                        _viewModel.SelectedRole = result.First();
                        DependencyService.Get<INotify>().ShowToast($"You are logging in as {_viewModel.SelectedRole}");
                    }
                }
                else
                {
                    await App.pageDialog.DisplayAlertAsync("Alert.!", AppResources.ResourceManager.GetString("chckuser", AppResources.Culture), "OK");
                    this.Txtusername.Focus();
                    _viewModel.EnableSubmitButton = false;
                }
            }

        }
        private void Txtusername_EntryFocused(object sender, FocusEventArgs e)
        {
            _viewModel.EnableSubmitButton = false;
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            var btn = ((Button)sender).CommandParameter.ToString();
            if (btn == "LDAP")
            {
                await _viewModel.Login(true);
            }
            else
                await _viewModel.Login();

            //this.txtusername.Unfocus();

        }
     

        private void LangPicker_SelectionChanged(object sender, Events.EventArgs<string> e)
        {
            if (string.IsNullOrEmpty(e.Value))
            {
                return;
            }
            Library.KEY_USER_LANGUAGE = e.Value;

           var language = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList().First(element => element.EnglishName.Contains(e.Value));


            AppResources.Culture = language;
            App.Current.MainPage = new LoginPage();
        }
    }
}
