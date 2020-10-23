using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Services;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Sodexo_JTH.Helpers;
using Sodexo_JTH.Interfaces;
using Sodexo_JTH.Models;
using Sodexo_JTH.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DependencyService = Xamarin.Forms.DependencyService;

namespace Sodexo_JTH.PopUpControl
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CancelOrderPopup : PopupPage
    {
        public IPageDialogService PageDialog { get; private set; }

        List<mstr_meal_history> PatientMealHistoryList;
        public bool IsChanged { get; set; }
        public CancelOrderPopup(List<mstr_meal_history> _PatientMealHistoryList, IPageDialogService pageDialog)
        {
            InitializeComponent();

            PageDialog = pageDialog;
            PatientMealHistoryList = new List<mstr_meal_history>(_PatientMealHistoryList);
            HistoryList.ItemsSource = PatientMealHistoryList;
            //Search clist in T2O PatientSearch page
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var a = this.BindingContext;
        }
        private async void Titlelbl_Close(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        private async void CancelBtn_Clicked(object sender, EventArgs e)
        {
            var selectedRecord = (sender as Button).BindingContext as mstr_meal_history;
            if (string.IsNullOrEmpty(selectedRecord.remarks))
            {
                var msg = Library.KEY_USER_LANGUAGE == "Thai" ? "กรุณาใส่ข้อสังเกต" : "Please Enter Remarks";
                await PageDialog.DisplayAlertAsync("Error!!", msg, "OK");
                return;
            }
            else
            {
                dynamic p = new JObject();
                p.Id = selectedRecord.Id;//id;
                p.createdby = selectedRecord.createdby;
                p.meal_detail_id = selectedRecord.meal_detail_id;
                p.mealtimeid = selectedRecord.mealtimeid;
                p.mealtimename = selectedRecord.mealtimename;
                p.orderdate = selectedRecord.orderdate;
                p.remark = selectedRecord.remarks;
                p.ward_bed = "";
                p.wardid = "";
                p.work_station_IP = DependencyService.Get<ILocalize>().GetIpAddress();
                p.system_module = DependencyService.Get<ILocalize>().GetDeviceName();//GetMachineNameFromIPAddress(p.work_station_IP);

                string json = JsonConvert.SerializeObject(p);

                var httpClient = new HttpClient();

                var result = await httpClient.PostAsync($"{Library.URL}/OrderCanceled", new StringContent(json, Encoding.UTF8, "application/json"));
                var contents = await result.Content.ReadAsStringAsync();


                if (contents == "true")
                {
                    await PageDialog.DisplayAlertAsync("Alertt!!", AppResources.ResourceManager.GetString("ml1",AppResources.Culture), "OK");
                    HistoryList.ItemsSource = new List<mstr_meal_history>();
                    PatientMealHistoryList.Remove(selectedRecord);
                    HistoryList.ItemsSource = PatientMealHistoryList;
                    IsChanged = true;
                }
                else
                    await DisplayAlert("", AppResources.ResourceManager.GetString("ml12", AppResources.Culture), "OK");

                if (!PatientMealHistoryList.Any())
                {
                    await Navigation.PopAllPopupAsync();
                }
            }
        }
    }
}