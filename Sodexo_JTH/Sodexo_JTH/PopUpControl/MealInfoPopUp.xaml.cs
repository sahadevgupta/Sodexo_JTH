using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Sodexo_JTH.Models;
using Sodexo_JTH.Resources;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms.Xaml;

namespace Sodexo_JTH.PopUpControl
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MealInfoPopUp : PopupPage
    {
        public MealInfoPopUp(ObservableCollection<mstr_meal_history> MealHistory, string MealType)
        {
            InitializeComponent();

            if (MealType == "BF")
            {
                titlelbl.Text = $"{AppResources.ResourceManager.GetString("pmname1",AppResources.Culture)} (BreakFast)";
            }
            else if (MealType == "LH")
            {
                titlelbl.Text = $"{AppResources.ResourceManager.GetString("pmname1", AppResources.Culture)} (Lunch)";
            }
            else
                titlelbl.Text = $"{AppResources.ResourceManager.GetString("pmname1", AppResources.Culture)} (Dinner)";

            meallist.ItemsSource = MealHistory;
            // BindableLayout.SetItemsSource(mealinfostack, MealHistory);
        }

        private async void Titlelbl_Close(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}