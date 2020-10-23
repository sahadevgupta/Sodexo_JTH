using Newtonsoft.Json;

using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Sodexo_JTH.Helpers;
using Sodexo_JTH.Interfaces;
using Sodexo_JTH.Models;
using Sodexo_JTH.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using DependencyService = Xamarin.Forms.DependencyService;

namespace Sodexo_JTH.ViewModels
{
    public class MealOrderStatusPageViewModel : ViewModelBase
    {
        private mstr_ward_details _selectedWard;
        public mstr_ward_details SelectedWard
        {
            get { return _selectedWard; }
            set { SetProperty(ref _selectedWard, value); }
        }
        private mstr_meal_time _selectedMealTime;
        public mstr_meal_time SelectedMealTime
        {
            get { return _selectedMealTime; }
            set { SetProperty(ref _selectedMealTime, value); }
        }
        private string _selectedStatus;
        public string SelectedMealStatus
        {
            get { return _selectedStatus; }
            set { SetProperty(ref _selectedStatus, value); }
        }
        private List<mstr_ward_details> _wardData;
        public List<mstr_ward_details> WardData
        {
            get { return _wardData; }
            set { SetProperty(ref _wardData, value); }
        }
        private List<mstr_meal_time> _MealTimeList;
        public List<mstr_meal_time> MealTimeList
        {
            get { return _MealTimeList; }
            set { SetProperty(ref _MealTimeList, value); }
        }
        private ObservableCollection<meal_order_status> _mealOrderStatusCollection;
        public ObservableCollection<meal_order_status> MealOrderStatusCollection
        {
            get { return _mealOrderStatusCollection; }
            set { SetProperty(ref _mealOrderStatusCollection, value); }
        }
        private List<string> _statusList;
        public List<string> StatusList
        {
            get { return _statusList; }
            set { SetProperty(ref _statusList, value); }
        }
        private DateTime _selectedDate = DateTime.UtcNow;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }
       
        int mealtime_id = 0;
        int ward_id = 0;

        public DelegateCommand SearchCommand { get; set; }
        public MealOrderStatusPageViewModel(INavigationService navigationService, IPageDialogService pageDialog) : base(navigationService, pageDialog)
        {
            MealOrderStatusCollection = new ObservableCollection<meal_order_status>();
            SearchCommand = new DelegateCommand(SearchMethod);
        }

        public async void SearchMethod()
        {
            IsPageEnabled = true;
            await GetMealOrderStatus();
            IsPageEnabled = false;
        }

        private async Task GetMealOrderStatus()
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        MealOrderStatusCollection = new ObservableCollection<meal_order_status>();
                       

                        HttpClient httpClient = new System.Net.Http.HttpClient();

                        DateTime dt = SelectedDate;

                        string format_date = dt.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        var SelectedMealStatusIndex = StatusList.IndexOf(StatusList.First(x => x == SelectedMealStatus));

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_GETMEALORDERSTATUS + "/" + SelectedWard.ID + "/" + format_date + "/" + SelectedMealTime.ID + "/" + SelectedMealStatusIndex.ToString() + "/" + Library.KEY_USER_SiteCode);
                        HttpResponseMessage response = await httpClient.SendAsync(request);
                        var data = await response.Content.ReadAsStringAsync();

                        MealOrderStatusCollection = JsonConvert.DeserializeObject<ObservableCollection<meal_order_status>>(data);
                        if (!MealOrderStatusCollection.Any())
                        {
                            IsPageEnabled = false;
                            DependencyService.Get<INotify>().ShowToast("No records found!!");
                            return;
                        }

                        int srNo = 1;
                        foreach (var item in MealOrderStatusCollection)
                        {
                            item.SrNo = srNo++;
                        }

                        // stop
                       
                    }
                    catch (Exception excp)
                    {
                        // stop progressring

                    }

                }
                else
                {
                    await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("msg10", AppResources.Culture), "OK");
                }
            }
            catch (Exception excp)
            {
                // stop progressring
            }
        }

        private void FillWard()
        {
            try
            {
                var db = DependencyService.Get<IDBInterface>().GetConnection();

                WardData = new List<mstr_ward_details>(db.Query<mstr_ward_details>("Select ID,ward_name From mstr_ward_details where ward_type_name not like '%staff%' and status_id ='1' order by ID"));
                SelectedWard = WardData.FirstOrDefault();
            }
            catch (Exception exp)
            {
                PageDialog.DisplayAlertAsync("Alert!!", exp.Message, "OK");
            }
        }
        private void FillStatus()
        {
            StatusList = new List<string> { "All", "Ordered", "Not Ordered", "NBM", "Home Leave", "Delivered" };
            SelectedMealStatus = "All";
        }
        private void FillMealTime()
        {
            try
            {
                var db = DependencyService.Get<IDBInterface>().GetConnection();
                MealTimeList = new List<mstr_meal_time>(db.Query<mstr_meal_time>("Select * From mstr_meal_time where status_id ='1' order by ID"));
                SelectedMealTime = MealTimeList.FirstOrDefault();
            }
            catch (Exception exp)
            {
                PageDialog.DisplayAlertAsync("Alert!!", exp.Message, "OK");
            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            FillWard();
            FillStatus();
            FillMealTime();
        }
    }
}