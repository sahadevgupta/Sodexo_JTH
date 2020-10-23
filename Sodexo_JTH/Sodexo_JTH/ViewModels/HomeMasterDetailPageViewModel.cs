using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Rg.Plugins.Popup.Extensions;
using Sodexo_JTH.Helpers;
using Sodexo_JTH.Interfaces;
using Sodexo_JTH.Models;
using Sodexo_JTH.PopUpControl;
using Sodexo_JTH.Repos;
using Sodexo_JTH.Resources;
using System;
using System.Dynamic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using DependencyService = Xamarin.Forms.DependencyService;

namespace Sodexo_JTH.ViewModels
{
    public class HomeMasterDetailPageViewModel : ViewModelBase,IDisposable
    {


        private int _orderCount;
        public int OrderCount
        {
            get { return this._orderCount; }
            set { SetProperty(ref _orderCount, value); }
        }

        private float _CurrentProgress;

        public float CurrentProgress
        {
            get { return this._CurrentProgress; }
            set
            {
                SetProperty(ref _CurrentProgress, value);
                ui.Progress = _CurrentProgress;
            }
        }

        private bool _isMasterAvailable;

        public bool IsMasterAvailable
        {
            get { return this._isMasterAvailable; }
            set { SetProperty(ref _isMasterAvailable, value); }
        }

        private bool _isMenuAvailable;

        public bool IsMenuAvailable
        {
            get { return this._isMenuAvailable; }
            set { SetProperty(ref _isMenuAvailable, value); }
        }

        public bool isMstrNotificationAvailable;

        public LoadingViewPopup ui { get; private set; }

        public bool isMenuNotificationAvailable;
        public INavigation navigation { get; set; }

        IGenericRepo<mstr_meal_order_local> _orderlocalRepo;
        IGenericRepo<mstr_meal_time> _mealtimeRepo;
        public HomeMasterDetailPageViewModel(INavigationService navigationService, IGenericRepo<mstr_meal_order_local> orderlocalRepo,
            IGenericRepo<mstr_meal_time> mealtimeRepo, IPageDialogService pageDialog) : base(navigationService, pageDialog)
        {
            _orderlocalRepo = orderlocalRepo;
            _mealtimeRepo = mealtimeRepo;

           
        }

        private void LoadData()
        {
            OfflineOrderCount();

            GetUpdateNotification();
        }
        private void GetUpdateNotification()
        {
            System.Timers.Timer timer = new System.Timers.Timer(10000);
            timer.AutoReset = true; // the key is here so it repeats
            timer.Elapsed += timer_elapsed;
            timer.Start();

        }

        private void timer_elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isMstrNotificationAvailable)
            {
                MasterUpdateNotify();
            }
            if (!isMenuNotificationAvailable)
            {
                MenuUpdateNotify();
            }
        }

        public void OfflineOrderCount()
        {
            var localOrder = _orderlocalRepo.QueryTable();
            OrderCount = localOrder.Count();

        }

        async Task MasterUpdateNotify()
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    dynamic p = new ExpandoObject();
                    p.country_id = Library.KEY_USER_ccode;
                    string dt = Library.last_mastersynctime;
                    p.date = dt;
                    p.region_id = Library.KEY_USER_regcode;
                    //   p.set_menu_id = setid;
                    p.site_id = Library.KEY_USER_siteid;

                    //var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(p));
                    string stringPayload = JsonConvert.SerializeObject(p);
                    // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                    // display a message jason conversion
                    //var message1 = new MessageDialog("Data is converted in json.");
                    //await message1.ShowAsync();

                    using (var httpClient = new System.Net.Http.HttpClient())
                    {
                        var httpResponse = new System.Net.Http.HttpResponseMessage();
                        httpResponse = await httpClient.PostAsync(Library.URL + "/othernotification", httpContent);

                        if (httpResponse.Content != null)
                        {
                            var responseContent = await httpResponse.Content.ReadAsStringAsync();
                            if (responseContent == "true")
                            {
                                isMstrNotificationAvailable = true;
                                IsMasterAvailable = true;
                                ShowToastNotification("T2O", "An Update is Available for All Master!");

                            }
                            else
                            {

                                IsMasterAvailable = false;
                            }

                        }
                    }

                }

            }
            catch (Exception)
            {


            }

        }

        async Task MenuUpdateNotify()
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {

                    dynamic p = new ExpandoObject();
                    p.country_id = Library.KEY_USER_ccode;
                    string dt = Library.last_mealssynctime;
                    p.date = dt;
                    p.region_id = Library.KEY_USER_regcode;
                    //   p.set_menu_id = setid;
                    p.site_id = Library.KEY_USER_siteid;

                    //var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(p));
                    string stringPayload = JsonConvert.SerializeObject(p);
                    // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                    var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                    // display a message jason conversion
                    //var message1 = new MessageDialog("Data is converted in json.");
                    //await message1.ShowAsync();

                    using (var httpClient = new System.Net.Http.HttpClient())
                    {
                        var httpResponse = new System.Net.Http.HttpResponseMessage();


                        httpResponse = await httpClient.PostAsync(Library.URL + "/Menunotification", httpContent);



                        if (httpResponse.Content != null)
                        {
                            var responseContent = await httpResponse.Content.ReadAsStringAsync();
                            if (responseContent == "true")
                            {
                                isMenuNotificationAvailable = true;
                                IsMenuAvailable = true;
                                ShowToastNotification("T2O", "An Update is Available for Menu Master!");

                            }
                            else
                            {
                                IsMenuAvailable = false;
                            }

                        }
                    }
                    
                }
            }
            catch (Exception)
            {


            }

        }

        private void ShowToastNotification(string title, string body)
        {
            if (isMenuNotificationAvailable || isMstrNotificationAvailable)
                DependencyService.Get<INotify>().ShowLocalNotification(title, body);

        }

        public override void Destroy()
        {
            base.Destroy();
        }
        public async void DrawerSelected(string obj)
        {

            switch (obj)
            {
                case "Patient_Search":
                    await NavigationService.NavigateAsync("NavigationPage/PatientSearchPage");
                    break;
                case "Patient":
                    {
                        try
                        {
                            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                            {
                                ui = new LoadingViewPopup();
                                await navigation.PushPopupAsync(ui);

                              
                                await MasterSync.Sync_mstr_patient_info(this);
                               

                                await navigation.PopPopupAsync();


                                await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("msg7",AppResources.Culture), "OK");
                                CurrentProgress = 0f;
                            }
                            else
                                await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("msg10", AppResources.Culture), "OK");
                        }
                        catch (Exception)
                        {
                        }
                    }
                    break;
                case "Master":
                    {
                        IsPageEnabled = true;
                        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        {
                            isMstrNotificationAvailable = true;
                            ui = new LoadingViewPopup();
                            await navigation.PushPopupAsync(ui);

                            
                           
                            await MasterSync.SyncMaster(this);

                            int atime = Convert.ToInt32(Library.KEY_USER_adjusttime);
                            string tm = DateTime.Now.AddMinutes(-atime).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                            Library.last_mastersynctime = tm;

                            MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "MasterSync", "Master");
                            await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("msg0", AppResources.Culture), "OK");
                            await navigation.PopPopupAsync();
                            isMstrNotificationAvailable = false;
                            CurrentProgress = 0f;
                        }
                        else
                            await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("msg10", AppResources.Culture), "OK");
                        IsPageEnabled = false;

                    }
                    break;
                case "Menu":
                    {
                        IsPageEnabled = true;
                        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        {
                            isMenuNotificationAvailable = true;
                            ui = new LoadingViewPopup();
                            await navigation.PushPopupAsync(ui);

                           


                            await MasterSync.SyncMenuMaster(this);

                            //await MasterSync.Sync_mstr_menu_master();
                            //await MasterSync.Sync_mstr_menu_item();

                            int atime = Convert.ToInt32(Library.KEY_USER_adjusttime);
                            string tm = DateTime.Now.AddMinutes(-atime).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                            Library.last_mealssynctime = tm;

                            await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("msg8", AppResources.Culture), "OK");
                            await navigation.PopPopupAsync();
                            isMenuNotificationAvailable = false;
                            CurrentProgress = 0f;
                        }
                        else
                            await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("msg10", AppResources.Culture), "OK");
                        IsPageEnabled = false;
                    }
                    break;
                case "Order":
                    await NavigationService.NavigateAsync("NavigationPage/DailyOrderDetailPage", new NavigationParameters { { "Create", "Create" } });
                    break;
                case "Feed":
                    {
                        if (Library.KEY_USER_feedback_link.Contains("http"))
                            await NavigationService.NavigateAsync("NavigationPage/FeedBackPage");
                        else
                            await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("flna", AppResources.Culture), "OK");
                    }
                    break;
                case "Offline":
                    try
                    {
                        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        {
                            if (OrderCount == 0)
                            {
                                await PageDialog.DisplayAlertAsync("No Data", AppResources.ResourceManager.GetString("syncofforder", AppResources.Culture), "OK");

                                return;
                            }
                            else
                            {
                                var ui = new LoadingViewPopup();
                                await navigation.PushPopupAsync(ui);

                                await ConfirmOrderSync.SyncNow(_orderlocalRepo, _mealtimeRepo, PageDialog);

                                OfflineOrderCount();


                                await navigation.PopPopupAsync();



                                if (OrderCount == 0)
                                {
                                    await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("msg1", AppResources.Culture), "OK");
                                }

                                MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "offlineOrderSync", "offlineOrder");


                            }
                        }
                        else 
                            await PageDialog.DisplayAlertAsync("Error!!", AppResources.ResourceManager.GetString("serv", AppResources.Culture), "OK");
                    }
                    catch (Exception ex)
                    {


                    }

                    break;
                case "LogOut":
                    {
                        var response = await PageDialog.DisplayAlertAsync(AppResources.ResourceManager.GetString("txtlogText", AppResources.Culture), AppResources.ResourceManager.GetString("logsure", AppResources.Culture), AppResources.ResourceManager.GetString("contentyes", AppResources.Culture), AppResources.ResourceManager.GetString("contentno", AppResources.Culture));
                        if (response)
                        {
                            //Library.KEY_USER_LANGUAGE = "English";
                            SessionManager.Instance.EndTrackSession();
                            await NavigationService.NavigateAsync("app:///LoginPage");
                        }
                    }
                    break;
            }
            IsPageEnabled = false;
        }

        public void Dispose()
        {
           
        }

        private void OnlocalOrderReceived(App arg1, string arg2)
        {
            OfflineOrderCount();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            MessagingCenter.Unsubscribe<App, string>((App)Xamarin.Forms.Application.Current, "LocalOrder");
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            MessagingCenter.Subscribe<App, string>((App)Xamarin.Forms.Application.Current, "LocalOrder", OnlocalOrderReceived);
            LoadData();
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
        }
    }
}
