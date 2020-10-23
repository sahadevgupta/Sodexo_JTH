using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Rg.Plugins.Popup.Extensions;
using Sodexo_JTH.Controls;
using Sodexo_JTH.Helpers;
using Sodexo_JTH.Interfaces;
using Sodexo_JTH.Models;
using Sodexo_JTH.Repos;
using Sodexo_JTH.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using DependencyService = Xamarin.Forms.DependencyService;

namespace Sodexo_JTH.ViewModels
{
    public class DailyOrderDetailPageViewModel : ViewModelBase
    {
        private bool _dataStackVisibility;
        public bool DataStackVisibility
        {
            get { return _dataStackVisibility; }
            set { SetProperty(ref _dataStackVisibility, value); }
        }
        private DateTime _selectedDate = DateTime.UtcNow;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }
        private mstr_mealdelivered _SelectedOrderDetail;
        public mstr_mealdelivered SelectedOrderDetail
        {
            get { return _SelectedOrderDetail; }
            set { SetProperty(ref _SelectedOrderDetail, value); }
        }
        
        private mstr_ward_details _selectedWard;
        public mstr_ward_details SelectedWard
        {
            get { return _selectedWard; }
            set
            {
                SetProperty(ref _selectedWard, value);
                if (value != null)
                {
                    PopulateBedData(value);
                }



            }
        }

        private mstr_meal_time _selectedMealTime;
        public mstr_meal_time SelectedMealTime
        {
            get { return _selectedMealTime; }
            set
            {

                SetProperty(ref _selectedMealTime, value);

            }
        }
        private ObservableCollection<mstr_ward_details> _wardData;
        public ObservableCollection<mstr_ward_details> WardData
        {
            get { return _wardData; }
            set { SetProperty(ref _wardData, value); }
        }
        private ObservableCollection<mstr_meal_time> _MealTimeList;
        public ObservableCollection<mstr_meal_time> MealTimeList
        {
            get { return _MealTimeList; }
            set { SetProperty(ref _MealTimeList, value); }
        }
        private ObservableCollection<mstr_mealdelivered> _mealdeliveredCollection;
        public ObservableCollection<mstr_mealdelivered> MealDeliveredCollection
        {
            get { return _mealdeliveredCollection; }
            set { SetProperty(ref _mealdeliveredCollection, value); }
        }
        private ObservableCollection<mstr_bed_details> _bedDetails;

        public ObservableCollection<mstr_bed_details> BedDetails
        {
            get { return this._bedDetails; }
            set { SetProperty(ref _bedDetails, value); }
        }

        private bool _isCareGiver;
        public bool IsCareGiver
        {
            get { return _isCareGiver; }
            set { SetProperty(ref _isCareGiver, value); }
        }
        private mstr_bed_details _selectedBed;

        public mstr_bed_details SelectedBed
        {
            get { return this._selectedBed; }
            set { SetProperty(ref _selectedBed, value); }
        }
        private ObservableCollection<mstr_caregiver_mealorder_details> _caregiver_details;
        public ObservableCollection<mstr_caregiver_mealorder_details> caregiver_details
        {
            get { return _caregiver_details; }
            set { SetProperty(ref _caregiver_details, value); }
        }
        
        private string _entryText;
        public string RemarkText
        {
            get { return _entryText; }
            set { SetProperty(ref _entryText, value); }
        }
        public string PaymentModeName { get; set; }
        public double TotalAmount { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand UpdateCommand { get; set; }
        IGenericRepo<mstr_ward_details> _mstrWardRepo;
        IGenericRepo<mstr_bed_details> _mstrBedRepo;
        IGenericRepo<mstr_DisplayPaymentModeDetails> _displayPaymentModeDetails;
        public INavigation _iNavigation { get; set; }

        public bool success { get; set; }

        public DailyOrderDetailPageViewModel(INavigationService navigationService, IGenericRepo<mstr_ward_details> mstrWardRepo,
            IGenericRepo<mstr_bed_details> mstrBedRepo, IGenericRepo<mstr_DisplayPaymentModeDetails> displayPaymentModeDetails, IPageDialogService pageDialog) : base(navigationService, pageDialog)
        {
            _mstrWardRepo = mstrWardRepo;
            _mstrBedRepo = mstrBedRepo;
            _displayPaymentModeDetails = displayPaymentModeDetails;
            MealDeliveredCollection = new ObservableCollection<mstr_mealdelivered>();
            SearchCommand = new DelegateCommand(SearchMethod);
            UpdateCommand = new DelegateCommand(UpdateOrder);
        }

        private async void UpdateOrder()
        {
            bool ischecked = false;
            if (IsCareGiver)
            {
                if (MealDeliveredCollection.Where(x => x.is_checked).Count() == 0)
                {
                    await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("yc", CultureInfo.CurrentCulture), "OK");
                    return;
                }

                if (MealDeliveredCollection.Where(x => x.is_checked).Count() > 1)
                {
                    await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("yc", CultureInfo.CurrentCulture), "OK");
                    return;
                }

                SelectedOrderDetail = MealDeliveredCollection.FirstOrDefault(x => x.is_checked);
                SelectedOrderDetail.ward_bed = SelectedOrderDetail.Ward + "-" + SelectedOrderDetail.Bed;

                // displaying patient information on caregiver popup


                if (IsCareGiver)
                {
                    await GetCaregiverData();
                    var ui = new CaregiverODPopup(caregiver_details.FirstOrDefault(), this);
                    ui.BindingContext = SelectedOrderDetail;
                    await _iNavigation.PushPopupAsync(ui, false);
                }
                else
                {
                    await PageDialog.DisplayAlertAsync("Alert", "Please select record first.", "Ok");
                }
            }
            else
            {
                for (int i = 0; i < MealDeliveredCollection.Count; i++)
                {
                    mstr_mealdelivered mealdelivered = MealDeliveredCollection.ElementAt(i);
                    if (mealdelivered.is_checked)
                    {
                        ischecked = true;
                        break;
                    }
                }

                if (ischecked)
                {
                    await SendDelivered_request();
                    if (success)
                    {
                        await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("delivered_message", AppResources.Culture), "OK");

                    }

                    await GetMealDeliveredData();
                    //await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("delivered_message", CultureInfo.CurrentCulture), "OK");

                    //await DisplayAlert(AppResources.ResourceManager.GetString("alert", CultureInfo.CurrentCulture), AppResources.ResourceManager.GetString("delivered_message", CultureInfo.CurrentCulture), "OK");

                }
                else
                {
                    await PageDialog.DisplayAlertAsync("Alert", "Please select record first.", "Ok");
                }

                //}

            }
        }
        public async Task GetCaregiverData()
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    //string URL = Library.KEY_http + Library.KEY_SERVER_IP + "/" + Library.KEY_SERVER_LOCATION + "/sodexo.svc";
                    try
                    {
                        //start progessring
                        IsPageEnabled = true;
                        caregiver_details = new ObservableCollection<mstr_caregiver_mealorder_details>();

                        HttpClient httpClient = new System.Net.Http.HttpClient();

                        DateTime dt = SelectedDate;

                        string format_date = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_CAREGIVERMENUITEMS + "/" +
                            SelectedOrderDetail.pateint_id + "/" + SelectedMealTime.ID + "/" + SelectedOrderDetail.OrderedID + "/" + format_date);
                        HttpResponseMessage response = await httpClient.SendAsync(request);
                        var data = await response.Content.ReadAsStringAsync();
                        // JArray jarray = JArray.Parse(data);

                        caregiver_details = JsonConvert.DeserializeObject<ObservableCollection<mstr_caregiver_mealorder_details>>(data);
                        //meal_delivered= JsonConvert.DeserializeObject<mstr_mealdelivered>(data);

                        var paymentmode = 0;

                        foreach (var item in caregiver_details)
                        {
                            TotalAmount = TotalAmount + Convert.ToDouble(item.amount);
                            paymentmode = item.mode_of_payment;
                            item.paymentmodename = _displayPaymentModeDetails.QueryTable().First(x => x.ID == item.mode_of_payment).payment_mode_name;
                        }
                        IsPageEnabled = false;
                    }
                    catch (Exception excp)
                    {
                        // stop progressring
                        IsPageEnabled = false;

                    }
                    IsPageEnabled = false;
                }
                else
                {


                    await PageDialog.DisplayAlertAsync("Alert!!", "Server is not accessible, please check internet connection.", "OK");
                    IsPageEnabled = false;
                }
            }
            catch (Exception excp)
            {
                // stop progressring
                IsPageEnabled = false;
            }
        }

        public async Task SendDelivered_request_caregiver()
        {
            //bool status = false;
            string mealtype = SelectedMealTime.meal_name;
            string meal_time = "";
            if (mealtype.ToLowerInvariant() == "Breakfast".ToLowerInvariant())
            {
                meal_time = "BF";
            }
            else if (mealtype.ToLowerInvariant() == "Lunch".ToLowerInvariant())
            {
                meal_time = "LH";
            }
            else if (mealtype.ToLowerInvariant() == "Dinner".ToLowerInvariant())
            {
                meal_time = "DN";
            }
            else
            {
                meal_time = mealtype;
            }

            try
            {
                string ss = "";
                if (string.IsNullOrEmpty(RemarkText))
                {
                    ss = "NA";
                }
                else
                {
                    ss = RemarkText;
                }
                //--------POST-----------
                dynamic jObj = new JObject();
                jObj.MealTime = meal_time;
                jObj.meal_time_id = SelectedMealTime.ID;
                jObj.mode_of_payment = 0;
                jObj.orderId = SelectedOrderDetail.OrderedID;
                jObj.payment_remark = string.Empty;
                // var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(p));
                string stringPayload = JsonConvert.SerializeObject(jObj);
                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    var httpResponse = new System.Net.Http.HttpResponseMessage();

                    httpResponse = await httpClient.PostAsync(Library.URL + "/" + Library.METHODE_SETDELIVEREDSTATUS, httpContent);

                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();

                    }
                }
            }
            catch (Exception exp)
            {
                await PageDialog.DisplayAlertAsync("Alert!!", exp.Message, "OK");
            }
        }
        public async Task SendDelivered_request()
        {
            //bool status = false;
            try
            {
                for (int i = 0; i < MealDeliveredCollection.Count; i++)
                {
                    mstr_mealdelivered mealdelivered = MealDeliveredCollection.ElementAt(i);
                    if (mealdelivered.is_checked)
                    {
                        string order_id = mealdelivered.OrderedID.ToString();
                        string mealtime = mealdelivered.MealTime;
                        string nullvalue = "";

                        string meatype = mealtime;
                        string meal_time = "";
                        if (meatype.ToLowerInvariant() == "Breakfast".ToLowerInvariant())
                        {
                            meal_time = "BF";
                        }
                        else if (meatype.ToLowerInvariant() == "Lunch".ToLowerInvariant())
                        {
                            meal_time = "LH";
                        }
                        else if (meatype.ToLowerInvariant() == "Dinner".ToLowerInvariant())
                        {
                            meal_time = "DN";
                        }
                        else
                        {
                            meal_time = meatype;
                        }
                        // String url = Gloabal.URL + Gloabal.METHODE_SETDELIVEREDSTATUS + "/" + orderid + "/" + Uri.encode(mealtime) + "/" + mealtimeid + "/" + paymentmode_id + "/" + Uri.encode(payment_remark);

                        //--------POST-----------
                        dynamic p = new JObject();
                        p.MealTime = meal_time;
                        p.meal_time_id = Convert.ToString(SelectedMealTime.ID);
                        p.mode_of_payment = 0;
                        p.orderId = order_id;
                        p.payment_remark = nullvalue;
                        // var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(p));
                        string stringPayload = JsonConvert.SerializeObject(p);
                        // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                        // display a message jason conversion
                        //var message1 = new MessageDialog("Data is converted in json.");
                        //await message1.ShowAsync();

                        using (var httpClient = new System.Net.Http.HttpClient())
                        {
                            var httpResponse = new System.Net.Http.HttpResponseMessage();

                            httpResponse = await httpClient.PostAsync(Library.URL + "/" + Library.METHODE_SETDELIVEREDSTATUS, httpContent);

                            if (httpResponse.Content != null)
                            {
                                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                                success = true;

                            }
                        }

                    }
                }
            }
            catch (Exception exp)
            {
                await PageDialog.DisplayAlertAsync("Alert!!", exp.Message, "OK");
                //await DisplayAlert("", AppResources.ResourceManager.GetString(excp.Message, CultureInfo.CurrentCulture), "OK");
            }
        }
        private void PopulateBedData(mstr_ward_details SelectedWard)
        {
            BedDetails = new ObservableCollection<mstr_bed_details>
                        (
                        _mstrBedRepo.QueryTable().Where(x => x.ward_id == SelectedWard.ID && x.status_id == 1)
                        );
            BedDetails.Insert(0, new mstr_bed_details { bed_no = "All" });
            SelectedBed = BedDetails.First(); ;
        }
        public async void SearchMethod()
        {
            if (SelectedBed == null || SelectedMealTime == null)
            {
                await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("selbedmeal", AppResources.Culture), "OK");
                return;
            }

            await GetMealDeliveredData();
        }
        public async Task GetMealDeliveredData()
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        MealDeliveredCollection = new ObservableCollection<mstr_mealdelivered>();
                        IsPageEnabled = true;

                        HttpClient httpClient = new System.Net.Http.HttpClient();

                        DateTime dt = SelectedDate;

                        string format_date = dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_GETDELIVEREDDATA + "/" + format_date + "/" + SelectedMealTime.ID + "/" + Library.KEY_USER_ccode + "/" + Library.KEY_USER_regcode + "/" + Library.KEY_USER_siteid);
                        HttpResponseMessage response = await httpClient.SendAsync(request);

                        var data = await response.Content.ReadAsStringAsync();
                        var orderData = JsonConvert.DeserializeObject<ObservableCollection<mstr_mealdelivered>>(data);

                        if (!orderData.Any())
                        {
                            IsPageEnabled = false;
                            DependencyService.Get<INotify>().ShowToast("No records found!!");
                            return;
                        }


                        foreach (var item in orderData)
                        {
                            item.istrue = false;
                            if (!SelectedBed.bed_no.Equals("All"))
                            {
                                if (item.Ward == SelectedWard.ward_name && item.Bed == SelectedBed.bed_no && item.MealTime == SelectedMealTime.meal_name)
                                {
                                    if ((IsCareGiver) && (item.Guest.ToLowerInvariant() == "True".ToLowerInvariant()))
                                    {

                                        MealDeliveredCollection.Add(item);

                                    }
                                    if (!IsCareGiver && (item.Guest.ToLowerInvariant() == "False".ToLowerInvariant()))
                                    {

                                        MealDeliveredCollection.Add(item);
                                    }
                                }
                            }
                            else
                            {
                                if (item.Ward == SelectedWard.ward_name && item.MealTime == SelectedMealTime.meal_name)
                                {
                                    if (IsCareGiver && (item.Guest.ToLowerInvariant() == "True".ToLowerInvariant()))
                                    {

                                        MealDeliveredCollection.Add(item);

                                    }
                                    if (!IsCareGiver && (item.Guest.ToLowerInvariant() == "False".ToLowerInvariant()))
                                    {

                                        MealDeliveredCollection.Add(item);
                                        //await DisplayAlert("alert", item.is_verifed.ToString(), "OK");
                                    }
                                }
                                else
                                {
                                    //if ((is_caregiver == true) && (item.Guest == "True"))
                                    //{

                                    //    meal_deliveredNew.Add(item);

                                    //}
                                    //if (!is_caregiver && (item.Guest == "False"))
                                    //{

                                    //    meal_deliveredNew.Add(item);
                                    //}
                                }
                            }



                            // stop
                            IsPageEnabled = false;
                        }

                        if (!MealDeliveredCollection.Any())
                        {
                            IsPageEnabled = false;
                            DependencyService.Get<INotify>().ShowToast("No records found!!");
                            return;
                        }
                        
                    }
                    catch (Exception excp)
                    {
                        // stop progressring
                        IsPageEnabled = false;

                    }
                    IsPageEnabled = false;

                }
                else
                {


                    await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("msg10", AppResources.Culture), "OK");
                    IsPageEnabled = false;
                }
            }
            catch (Exception excp)
            {
                // stop progressring
                IsPageEnabled = false;
            }
        }
        internal async void ScanDeliveredOD(string orderid, string orderedID)
        {
            Boolean status = false;

            if (orderid == orderedID)
            {

                var patientid = MealDeliveredCollection.FirstOrDefault(x => x.OrderedID.ToString() == orderid);
                var mealtime_id = SelectedMealTime.ID;
                HttpClient httpClient = new System.Net.Http.HttpClient();

                DateTime dt = SelectedDate;

                string format_date = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_QRVERIFIED + "/" + patientid.pateint_id + "/" + orderid + "/" + mealtime_id + "/" + format_date);

                //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL + "/" + Library.METHODE_QRVERIFIED + "/" + patientid + "/" + tagid + "/" + mealtime_id + "/" + format_date);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();

                //dynamic data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

                var data = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<Scan_verify>>(data);

                if (list.Count > 0)
                {
                    status = list[0].is_verifed;
                }

                //status = Convert.ToBoolean(data.is_verifed);

                //status = data.is_verifed;

                //JArray jarray = JArray.Parse(data);

                if (status)
                {
                    await GetMealDeliveredData();
                }
            }
            else
            {
                await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("scanned_orderid_not_match", AppResources.Culture), "OK");
            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("Create"))
            {
                FillWard();
                FillMealTime();
            }

        }
        private async void FillWard()
        {
            try
            {
                var db = DependencyService.Get<IDBInterface>().GetConnection();

                WardData = new ObservableCollection<mstr_ward_details>(db.Query<mstr_ward_details>("Select ID,ward_name From mstr_ward_details where ward_type_name not like '%staff%' and status_id ='1' order by ID"));
                //SelectedWard = WardData.FirstOrDefault();
            }
            catch (Exception exp)
            {
                await PageDialog.DisplayAlertAsync("Alert!!", exp.Message, "OK");
            }
        }
        private async void FillMealTime()
        {
            try
            {
                var db = DependencyService.Get<IDBInterface>().GetConnection();
                MealTimeList = new ObservableCollection<mstr_meal_time>(db.Query<mstr_meal_time>("Select * From mstr_meal_time where status_id ='1' order by ID"));
            }
            catch (Exception exp)
            {
                await PageDialog.DisplayAlertAsync("Alert!!", exp.Message, "OK");
            }
        }
    }

}
