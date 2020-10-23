using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
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
    public partial class TermsConditionPopup : PopupPage
    {
        public bool isAccepted { get; set; }
        public TermsConditionPopup()
        {
            InitializeComponent();
        }
        protected override bool OnBackgroundClicked()
        {
            return false;
        }
        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            isAccepted = true;
            await Navigation.PopPopupAsync();
        }
    }
}