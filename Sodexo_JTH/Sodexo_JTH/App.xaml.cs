using Prism;
using Prism.Ioc;
using Prism.Services;
using Sodexo_JTH.Helpers;
using Sodexo_JTH.Models;
using Sodexo_JTH.Repos;
using Sodexo_JTH.Resources;
using Sodexo_JTH.Services;
using Sodexo_JTH.ViewModels;
using Sodexo_JTH.Views;
using System;
using System.Globalization;
using System.Threading;
using Xamarin.Forms;

[assembly: ExportFont("Sansation-Regular.ttf", Alias = "sansation")]
namespace Sodexo_JTH
{
    public partial class App
    {
       
        public static IPageDialogService pageDialog;
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            Xamarin.Forms.Device.SetFlags(new string[] { "RadioButton_Experimental"});

            InitializeComponent();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.InstalledUICulture;

            AppResources.Culture = Thread.CurrentThread.CurrentUICulture;
            if (Thread.CurrentThread.CurrentUICulture.EnglishName.Contains("Thai"))
            {
                Library.KEY_USER_LANGUAGE = "Thai";
            }
            else
                Library.KEY_USER_LANGUAGE = "English";
            await NavigationService.NavigateAsync(nameof(LoginPage));
        }

        protected override void OnStart()
        {
            base.OnStart();

            SessionManager.Instance.SessionDuration = TimeSpan.FromMinutes(20);
            SessionManager.Instance.OnSessionExpired = HandleSessionExpired;

            //AppCenter.Start("android=d6e34c5c-4550-484a-b63e-e368c3006ca5;",typeof(Analytics), typeof(Crashes));
        }
        private async void HandleSessionExpired(object sender, EventArgs e)
        {
            SessionManager.Instance.EndTrackSession();

            await pageDialog.DisplayAlertAsync("Alert !!", AppResources.ResourceManager.GetString("session", AppResources.Culture), "OK");

            await NavigationService.NavigateAsync("app:///LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Initialize Services
            containerRegistry.RegisterSingleton<IMealOrderLocalManager, MealOrderLocalManager>();
            containerRegistry.RegisterSingleton<IPatientManager, PatientManager>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_meal_order_local>, GenericRepo<mstr_meal_order_local>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_ward_details>, GenericRepo<mstr_ward_details>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_bed_details>, GenericRepo<mstr_bed_details>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_others_master>, GenericRepo<mstr_others_master>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_allergies_master>, GenericRepo<mstr_allergies_master>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_ingredient>, GenericRepo<mstr_ingredient>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_therapeutic>, GenericRepo<mstr_therapeutic>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_diet_texture>, GenericRepo<mstr_diet_texture>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_meal_type>, GenericRepo<mstr_meal_type>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_meal_time>, GenericRepo<mstr_meal_time>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_menu_item_category>, GenericRepo<mstr_menu_item_category>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_bed_meal_class_mapping>, GenericRepo<mstr_bed_meal_class_mapping>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_mealclass>, GenericRepo<mstr_mealclass>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_meal_option>, GenericRepo<mstr_meal_option>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_diet_type>, GenericRepo<mstr_diet_type>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_remarks>, GenericRepo<mstr_remarks>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_patient_info>, GenericRepo<mstr_patient_info>>();
            containerRegistry.RegisterSingleton<IGenericRepo<mstr_DisplayPaymentModeDetails>, GenericRepo<mstr_DisplayPaymentModeDetails>>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<HomeMasterDetailPage, HomeMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<PatientSearchPage, PatientSearchPageViewModel>();
            containerRegistry.RegisterForNavigation<PatientInformationPage>();
            containerRegistry.RegisterForNavigation<MealOrderPage, MealOrderPageViewModel>();
            containerRegistry.RegisterForNavigation<MealSummaryPage, MealSummaryPageViewModel>();
            containerRegistry.RegisterForNavigation<DailyOrderDetailPage, DailyOrderDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<MealOrderStatusPage>();
            containerRegistry.RegisterForNavigation<FeedBackPage, FeedBackPageViewModel>();
            containerRegistry.RegisterForNavigation<ADFSPage, ADFSPageViewModel>();
        }
    }
}
