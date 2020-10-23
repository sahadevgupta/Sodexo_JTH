using dotMorten.Xamarin.Forms;
using Prism.Services.Dialogs;
using Rg.Plugins.Popup.Extensions;
using Sodexo_JTH.Effects;
using Sodexo_JTH.Models;
using Sodexo_JTH.PopUpControl;
using Sodexo_JTH.ViewModels;
using System;
using Xamarin.Forms;

namespace Sodexo_JTH.Views
{
    public partial class PatientSearchPage : ContentPage
    {
        PatientSearchPageViewModel _viewModel;
        public PatientSearchPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as PatientSearchPageViewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.navigation = Navigation;

            MessagingCenter.Subscribe<App, string>((App)Xamarin.Forms.Application.Current, "MasterSync", OnSyncMasterTap);
            MessagingCenter.Subscribe<App, string>((App)Xamarin.Forms.Application.Current, "offlineOrderSync", OnOfflineOrderSyncTap);
            MessagingCenter.Subscribe<App, string>((App)Xamarin.Forms.Application.Current, "NewOrder", OnNewOrderReceived);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<App, string>((App)Xamarin.Forms.Application.Current, "MasterSync");
            MessagingCenter.Unsubscribe<App, string>((App)Xamarin.Forms.Application.Current, "offlineOrderSync");
            MessagingCenter.Unsubscribe<App, string>((App)Xamarin.Forms.Application.Current, "NewOrder");
        }

        private void OnNewOrderReceived(App arg1, string arg2)
        {
            _viewModel.OnNewOrderReceived( arg1,  arg2);
        }

        private void OnOfflineOrderSyncTap(App arg1, string arg2)
        {
            _viewModel.OnOfflineOrderSyncTap(arg1, arg2);
        }

        private void OnSyncMasterTap(App arg1, string arg2)
        {
            _viewModel.LoadData();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            _viewModel.Patients = new System.Collections.ObjectModel.ObservableCollection<mstr_patient_info>();
            if ((e as TappedEventArgs).Parameter.Equals("ByWard"))
            {
                _viewModel.IsWardVisible = true;
            }
            else
            {
                _viewModel.IsWardVisible = false;
            }


        }

        

        private void DeleteOrder_Clicked(object sender, EventArgs e)
        {
            _viewModel.DeleteOrder(((ImageButton)sender).BindingContext as mstr_patient_info);
        }
        bool IsBusy;
        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            var imagebtn = ((ImageButton)sender);

            imagebtn.Opacity = 0;
            await imagebtn.FadeTo(1, 250);

            var selectedPatient = imagebtn.BindingContext as mstr_patient_info;
            var mealtype = imagebtn.CommandParameter.ToString();
            if (string.IsNullOrEmpty(selectedPatient.caregiverno))
            {
                await _viewModel.NavigateToMealPopUp(selectedPatient, mealtype);
            }
            imagebtn.Opacity = 1;
            IsBusy = false;
        }

        private async void ItemholdingEffect_ItemLongPressed(object sender, EventArgs e)
        {
            _viewModel.IsPageEnabled = true;

            mstr_patient_info selectedPatient;
            if (Device.RuntimePlatform == Device.iOS)
            {
                selectedPatient = ((Grid)sender).BindingContext as mstr_patient_info;
            }
            else
                selectedPatient = sender as mstr_patient_info;


            if (string.IsNullOrEmpty(selectedPatient.caregiverno))
            {
                var popup = new PatientInfoPopUp
                {
                    BindingContext = selectedPatient
                };

                await Navigation.PushPopupAsync(popup, true);
            }
            _viewModel.IsPageEnabled = false;
        }

        

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            _viewModel.NavigateToInfoPage((e as ItemTappedEventArgs).Item as mstr_patient_info);
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            _viewModel.NavigateToInfoPage(e.Item as mstr_patient_info);
        }

        private void ItemholdingEffect_ItemTapped(object sender, EventArgs e)
        {
            _viewModel.NavigateToInfoPage(sender as mstr_patient_info);
        }

        private void AutoSuggestionBox_TextChanged(object sender, Events.EventArgs<string> e)
        {
            
            _viewModel.GetPatientInfo(e.Value);
        }

        private void AutoSuggestBox_TextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
        {
            if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (!string.IsNullOrEmpty(autosuggestview.Text))
                {
                    _viewModel.IsPageEnabled = true;
                    _viewModel.GetPatientInfo(autosuggestview.Text);

                    _viewModel.IsPageEnabled = false;
                }
               
            }
            else if (e.Reason == AutoSuggestionBoxTextChangeReason.SuggestionChosen)
            {
                _viewModel.PatientName = autosuggestview.Text;
                _viewModel.GetPatientsList("PatientName");
            }
                
        }
    }
}
