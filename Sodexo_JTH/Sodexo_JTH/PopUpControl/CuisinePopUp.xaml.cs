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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CuisinePopUp : PopupPage
    {
        public event EventHandler submit_Clicked;
        public event EventHandler cancel_Clicked;
        public CuisinePopUp()
        {
            InitializeComponent();
        }
        async void ClosePopUp()
        {
            await Navigation.PopPopupAsync();
        }
        private void Cancel_Clicked(object sender, EventArgs e)
        {
            cancel_Clicked?.Invoke(this, e);
            ClosePopUp();
        }

        private void Submit_Clicked(object sender, EventArgs e)
        {
            submit_Clicked?.Invoke(this, e);
            ClosePopUp();
        }
    }
}