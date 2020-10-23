using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Sodexo_JTH.Interfaces;
using Sodexo_JTH.Models;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sodexo_JTH.PopUpControl
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientInfoPopUp : PopupPage
    {
        public PatientInfoPopUp()
        {
            InitializeComponent();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext is mstr_patient_info patient_Info)
            {
                var db = DependencyService.Get<IDBInterface>().GetConnection();

                string _allergies = string.Empty;
                if (!string.IsNullOrEmpty(patient_Info.Allergies))
                {
                    string[] foodallergies = patient_Info.Allergies.Split(',');
                    if (foodallergies.Count() > 0)
                    {
                        int counter = 0;
                        for (int i = 0; i < foodallergies.Count(); i++)
                        {
                            if (counter == 0)
                            {
                                int allergyid = Convert.ToInt32(foodallergies[i]);
                                var ee = db.Query<mstr_allergies_master>("Select * From mstr_allergies_master where ID='" + allergyid + "'").FirstOrDefault();
                                _allergies = ee == null ? "NA" : ee.allergies_name;
                                counter++;
                            }
                            else
                            {
                                int allergyid = Convert.ToInt32(foodallergies[i]);
                                var ee = db.Query<mstr_allergies_master>("Select * From mstr_allergies_master where ID='" + allergyid + "'").FirstOrDefault();
                                _allergies = _allergies + ", " + ee.allergies_name;
                            }

                        }
                    }
                }


                txtallergies.Text = (_allergies != string.Empty) ? _allergies : "NONE";
            }
        }

        private async void PopupTitleView_Close(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}