using Newtonsoft.Json;

using Prism.Commands;
using Prism.Mvvm;
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
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using DependencyService = Xamarin.Forms.DependencyService;

namespace Sodexo_JTH.ViewModels
{
    public class MealSummaryPageViewModel : ViewModelBase
    {

        private mstr_patient_info _patient;

        public mstr_patient_info Patient
        {
            get { return this._patient; }
            set { SetProperty(ref _patient, value); }
        }

        private mstr_meal_option _mealOption;

        public mstr_meal_option MealOption
        {
            get { return this._mealOption; }
            set { SetProperty(ref _mealOption, value); }
        }
        private mstr_diet_type _dietType;

        public mstr_diet_type DietType
        {
            get { return this._dietType; }
            set { SetProperty(ref _dietType, value); }
        }
        private mstr_meal_time _mealTime;

        public mstr_meal_time MealTime
        {
            get { return this._mealTime; }
            set { SetProperty(ref _mealTime, value); }
        }

        public List<mstr_therapeutic> Therapeutics { get; private set; }
        public List<mstr_ingredient> Ingredients { get; private set; }
        public List<mstr_allergies_master> Allergies { get; private set; }
        
        public List<mstr_meal_type> Cuisines { get; private set; }

        private string _remark;

        public string Remarks
        {
            get { return this._remark; }
            set { SetProperty(ref _remark, value); }
        }



        private ObservableCollection<MealSummary> _mealSummary = new ObservableCollection<MealSummary>();

        public ObservableCollection<MealSummary> MealSummaryDetails
        {
            get { return this._mealSummary; }
            set { SetProperty(ref _mealSummary, value); }
        }

        private string _allergiesName;

        public string AllergiesName
        {
            get { return this._allergiesName; }
            set { SetProperty(ref _allergiesName, value); }
        }
        public DelegateCommand CancelCommand => new DelegateCommand(async () => await CancelOrder());

        public DelegateCommand ConfirmCommand => new DelegateCommand(async () => await ConfirmPlaceOrder());

        public List<Cart> carts { get; private set; }
        public mstr_others_master Others { get; private set; }
        public List<mstr_others_master> OthersList { get; private set; }



        public MealSummaryPageViewModel(INavigationService navigationService, IPageDialogService pageDialog) : base(navigationService, pageDialog)
        {
        }

        private async Task CancelOrder()
        {
            var response = await PageDialog.DisplayAlertAsync(AppResources.ResourceManager.GetString("mo", AppResources.Culture), AppResources.ResourceManager.GetString("canc", AppResources.Culture), "Yes", "No");
            if (response)
            {

                var action = await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("ycn", AppResources.Culture), "Yes", "No");
                if (action)
                    await NavigationService.GoBackAsync();
                else
                    await NavigationService.NavigateAsync("../../../");
            }

        }

        private async Task ConfirmPlaceOrder()
        {
            var action = await PageDialog.DisplayAlertAsync(AppResources.ResourceManager.GetString("mlc", AppResources.Culture), AppResources.ResourceManager.GetString("mlc2", AppResources.Culture), "Yes", "No");
            if (action)
            {
                IsPageEnabled = true;
                await InsertIntoMealOrder();
                IsPageEnabled = false;
            }

        }

        public async Task InsertIntoMealOrder()
        {
            try
            {


                int mtype1 = 0;
                int mtype2 = 0;
                int mtype3 = 0;


                int mymeal_time_id = MealTime.ID;


                if (MealTime.meal_name == "Breakfast")
                {
                    if (Others.ID == 1 || Others.ID == 9 || Others.ID == 10)
                    {
                        mtype1 = 3;
                    }
                    else
                    {
                        mtype1 = 1;
                        
                    }
                }
                else if (MealTime.meal_name == "Lunch")
                {
                    if (Others.ID == 1 || Others.ID == 9 || Others.ID == 10)
                    {
                        mtype2 = 3;
                    }
                    else
                    {
                        mtype2 = 1;

                    }
                }
                else if (MealTime.meal_name == "Dinner")
                {
                    if (Others.ID == 1 || Others.ID == 9 || Others.ID == 10)
                    {
                        mtype3 = 3;
                    }
                    else
                    {
                        mtype3 = 1;

                    }
                }
                else
                {
                    mtype1 = 0;
                    mtype2 = 0;
                    mtype3 = 0;
                }


                int _meal_soup_id = 0;
                int _meal_menu_juice_item_id = 0;
                int _meal_entree_id = 0;
                int _meal_beverage_id = 0;
                int _meal_desert_id = 0;



                var entreecount = carts.Where(item => item.mealtimename.ToLower().Contains("entree".ToLower()) || item.mealtimename.ToLower().Contains("entrée".ToLower())).FirstOrDefault();
                var soupcount = carts.Where(item => item.mealtimename.ToLower().Contains("soup".ToLower())).FirstOrDefault();
                var juicecount = carts.Where(item => item.mealtimename.ToLower().Contains("juice".ToLower())).FirstOrDefault();
                var bevcount = carts.Where(item => item.mealtimename.ToLower().Contains("beverage".ToLower())).FirstOrDefault();
                var desscount = carts.Where(item => item.mealtimename.ToLower().Contains("dessert".ToLower())).FirstOrDefault();

                if (soupcount != null)
                    _meal_soup_id = Convert.ToInt32(soupcount.mealitemid.ToString() == null ? "1" : soupcount.mealitemid.ToString());
                if (juicecount != null)
                    _meal_menu_juice_item_id = Convert.ToInt32(juicecount.mealitemid.ToString() == null ? "1" : juicecount.mealitemid.ToString());

                if (entreecount != null)
                    _meal_entree_id = Convert.ToInt32(entreecount.mealitemid.ToString() == null ? "1" : entreecount.mealitemid.ToString());
                if (bevcount != null)
                    _meal_beverage_id = Convert.ToInt32(bevcount.mealitemid.ToString() == null ? "1" : bevcount.mealitemid.ToString());
                if (desscount != null)
                    _meal_desert_id = Convert.ToInt32(desscount.mealitemid.ToString() == null ? "1" : desscount.mealitemid.ToString());


                string _Therapeutic_ids;
                string _ingredeint_ids; //optional
                string _allergies_ids; //optional
                
                string _other_ids = ""; //optional
                string _cusinie_ids = "";

                var theraArray = Therapeutics.Select(x => x.ID).ToArray();
                _Therapeutic_ids = string.Join(",", theraArray);

                var ingridentArray = Ingredients.Select(x => x.ID).ToArray();
                _ingredeint_ids = string.Join(",", ingridentArray);

                var allergyArray = Allergies.Select(x => x.ID).ToArray();
                _allergies_ids = string.Join(",", allergyArray);

                var cusinieArray = Cuisines.Select(x => x.ID).ToArray();
                _cusinie_ids = string.Join(",", cusinieArray);


                var otehrArray = OthersList.Select(x => x.ID).ToArray();
                _other_ids = string.Join(",", otehrArray);

                var p = new mstr_meal_order_local();
                p.isdietician = false;
                p.is_staff = false;
                p.staff_name = "";
                p.dietician_comment = "";

                p.age_id = Convert.ToInt32(Patient.Age_ID);
                p.ward_id = Convert.ToInt32(Patient.Ward_ID);
                p.ward_type_id = Library.KEY_PATIENT_WARD_TYPE_ID;

                p.bed_id = Convert.ToInt32(Patient.Bed_ID);


                p.is_vegitarian = Convert.ToBoolean(Patient.isveg);
                p.is_halal = Convert.ToBoolean(Patient.ishalal);



                p.disposable_tray = Library.IsDisposableEnable;



                p.order_id = Convert.ToInt32(Library.KEY_ORDER_ID);
                p.order_date = Library.KEY_ORDER_DATE;

                p.order_no = "1";



                p.site_code = Patient.Site_Code;

                p.createdby = Convert.ToInt32(Library.KEY_USER_ID);

                p.meal_option_id = MealOption.ID;
                p.meal_diet_id = DietType.ID;
                p.meal_soup_id = _meal_soup_id;
                p.meal_menu_juice_item_id = _meal_menu_juice_item_id;
                p.meal_entree_id = _meal_entree_id;
                p.meal_beverage_id = _meal_beverage_id;
                p.meal_desert_id = _meal_desert_id;


                p.P_id = Patient.ID;
                p.BF = mtype1;
                p.LH = mtype2;
                p.DN = mtype3;

                p.Therapeutic_ids = _Therapeutic_ids;
                p.Meal_Type = _cusinie_ids;
                p.ingredeint_ids = _ingredeint_ids;

                if (_other_ids == "NA" || _other_ids == "0")
                {
                    p.other_ids = "";

                }
                else
                {
                    p.other_ids = _other_ids;

                }
                p.allergies_ids = _allergies_ids;


                if (carts.Any())
                {
                    var data = carts.Where(x => x.addonid != 0).FirstOrDefault();
                    if (data == null)
                        p.meal_addon_id = 0;
                    else
                        p.meal_addon_id = data.addonid;
                }
                else
                    p.meal_addon_id = 0;


                p.meal_time_id = mymeal_time_id;
                p.meal_type_id = 0;
                p.remark_id = 0;

                p.meal_remark = Remarks;


                p.ID = 10;
                p.adult_child = Patient.category;
                p.bedclass_id = Convert.ToInt32(Patient.Bed_Class_ID);
                p.bedclass_name = Patient.Bed_Class_Name;
                p.doctor_comment = Patient.Doctorcomment;
                p.general_comment = Patient.Generalcomment;
                p.patient_age = Patient.Patient_Age;
                p.patient_id = Patient.ID;//23;//Library.KEY_PATIENT_NRIC);// Library.KEY_PATIENT_ID);
                p.patient_name = Patient.Patientname;
                p.patient_race = Patient.patient_race;
                p.preferred_server = Patient.Preferredserver;
                p.ward_bed = Patient.ward_bed;
                p.Is_Late_Ordered = Convert.ToInt32(Library.KEY_IS_LATE_ORDERED);
                p.role = Library.KEY_USER_ROLE.ToString();

                if (Others.ID == 1 )
                {
                    p.orderstatus = "3";
                }
                else if (Others.ID == 9)
                {
                    p.orderstatus = "5";
                }
                else if (Others.ID == 10)
                {
                    p.orderstatus = "6";
                }
                else
                {
                    p.orderstatus = "0";

                }

                p.is_care_giver = false;
                p.mode_of_payment = 0;
                p.payment_remark = "";


                //p.Fluid_Consistency = Library.KEY_FLUID_INFO;
                p.fluid = Patient.FluidInfo == "NA" ? 0 : 1;        // temporary need to fix(20/6/819)
                p.role_Id = Convert.ToInt32(Library.KEY_USER_roleid);
                p.work_station_IP = DependencyService.Get<ILocalize>().GetIpAddress();
                p.system_module = Xamarin.Forms.DependencyService.Get<ILocalize>().GetDeviceName(); //GetMachineNameFromIPAddress(p.work_station_IP);

                //-----------------

                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        // Serialize our concrete class into a JSON String
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
                            // Do the actual request and await the response
                            if (Library.KEY_IS_CARE_GIVER.ToString() == "yes")
                            {
                                httpResponse = await httpClient.PostAsync(Library.URL + "/" + Library.METHODE_SAVEORDER, httpContent);

                            }
                            else if (Convert.ToInt32(Library.KEY_ORDER_ID) > 0)
                            {

                                // httpResponse = new Uri(URL + "/" + Library.METHODE_UPDATE_ORDER); //replace your Url
                                httpResponse = await httpClient.PostAsync(Library.URL + "/" + Library.METHODE_UPDATE_ORDER, httpContent);
                            }
                            else
                            {
                                // httpResponse = new Uri(URL + "/" + Library.METHODE_SAVEORDER); //replace your Url 
                                httpResponse = await httpClient.PostAsync(Library.URL + "/" + Library.METHODE_SAVEORDER, httpContent);
                            }
                            // display a message jason conversion
                            //var message2 = new MessageDialog(httpResponse.ToString());
                            //await message2.ShowAsync();
                            //var httpResponse = await httpClient.PostAsync(URL + "/" + Library.METHODE_SAVEORDER, httpContent);

                            // If the response contains content we want to read it!
                            if (httpResponse.Content != null)
                            {
                                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                                if (responseContent == "true")
                                {
                                    if (p.Is_Late_Ordered == 1)
                                    {
                                        var orderAction = await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("lateorder",AppResources.Culture), "Yes", "No");
                                        await OrderConfirmationMsg(orderAction);
                                    }
                                    else
                                    {
                                        var action = await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("yo", AppResources.Culture), "Yes", "No");
                                        await OrderConfirmationMsg(action);
                                    }
                                }
                                else
                                    await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("yon", AppResources.Culture), "OK");

                            }
                        }
                    }
                    catch (Exception)
                    {
                        await InserOrderLocal(p);
                    }

                }
                else
                    await InserOrderLocal(p);

            }
            catch (Exception excp)
            {
                await PageDialog.DisplayAlertAsync("Alert!!", excp.Message, "OK");
            }

        }

        private async Task OrderConfirmationMsg(bool action)
        {
            if (action)
            {
                MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "NewOrder", "order");

                await NavigationService.GoBackAsync(new NavigationParameters { { "NewOrder", "order" } });
            }
            else
            {
                await NavigationService.NavigateAsync("../../../");

            }
        }

        private async Task InserOrderLocal(mstr_meal_order_local p)
        {
            try
            {
                var db = DependencyService.Get<IDBInterface>().GetConnection();


                db.RunInTransaction(() =>
                {
                    db.Insert(p);
                });
                MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "LocalOrder", "lorder");

                var action = await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("yoff", AppResources.Culture), "Yes", "No");
                if (action)
                    await NavigationService.GoBackAsync(new NavigationParameters { { "NewOrder", "order" } });
                else
                    await NavigationService.NavigateAsync("../../../");


            }
            catch (Exception)
            {


            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            Patient = parameters["Patient"] as mstr_patient_info;
            MealOption = parameters["mealOption"] as mstr_meal_option;
            DietType = parameters["dietType"] as mstr_diet_type;
            Remarks = string.IsNullOrEmpty(parameters["remark"].ToString()) ? string.Empty : parameters["remark"].ToString();
            MealTime = parameters["MealTime"] as mstr_meal_time;

            Therapeutics = parameters["Therapeutics"] as List<mstr_therapeutic>;
            Ingredients = parameters["Ingredients"] as List<mstr_ingredient>;
            Allergies = parameters["Allergies"] as List<mstr_allergies_master>;
            Cuisines = parameters["Cuisines"] as List<mstr_meal_type>;
            carts = parameters["Carts"] as List<Cart>;
            Others = parameters["Others"] as mstr_others_master;
            OthersList = parameters["OthersList"] as List<mstr_others_master>;


            var allergyNames = Allergies.Select(x => x.allergies_name).ToArray();

            if (allergyNames.Count() > 0)
                AllergiesName = string.Join(",", allergyNames);
            else
                AllergiesName = "NA";

            int[] maxCountArray = { Therapeutics.Count, Ingredients.Count, carts.Count, OthersList.Count };
            int maxCount = maxCountArray.Max();


            LoadData(maxCount, Therapeutics, Ingredients, carts, OthersList);
        }

        private void LoadData(int maxCount, List<mstr_therapeutic> _Therapeutics, List<mstr_ingredient> _Ingredients, List<Cart> carts, List<mstr_others_master> others_Masters)
        {
            for (int i = 0; i < maxCount; i++)
            {
                MealSummary obj = new MealSummary();
                if (i < _Therapeutics.Count)
                    obj.Therapeutic = _Therapeutics[i].TH_code;
                else
                    obj.Therapeutic = "--";

                if (i < _Ingredients.Count)
                    obj.Ingrident = _Ingredients[i].ingredient_name;
                else
                    obj.Ingrident = "--";

                if (i < others_Masters.Count)
                {
                    if (!string.IsNullOrEmpty(others_Masters[i].others_name))
                    {
                        obj.Other = others_Masters[i].others_name;
                    }
                    else
                        obj.Other = "--";

                }
                else
                    obj.Other = "--";

                if (i < carts.Count)
                    obj.MealTime = carts[i].mealitemname;
                else
                    obj.MealTime = "--";

                MealSummaryDetails.Add(obj);
            }
        }
    }

    public class MealSummary : BindableBase
    {


        private string _therapeutic;

        public string Therapeutic
        {
            get { return this._therapeutic; }
            set { SetProperty(ref _therapeutic, value); }
        }


        private string _ingrident;

        public string Ingrident
        {
            get { return this._ingrident; }
            set { SetProperty(ref _ingrident, value); }
        }

        private string _other;

        public string Other
        {
            get { return this._other; }
            set { SetProperty(ref _other, value); }
        }


        private string _mealTime;

        public string MealTime
        {
            get { return this._mealTime; }
            set { SetProperty(ref _mealTime, value); }
        }
    }
}
