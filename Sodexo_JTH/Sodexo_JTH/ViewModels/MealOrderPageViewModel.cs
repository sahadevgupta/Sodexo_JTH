using Newtonsoft.Json.Linq;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Sodexo_JTH.Controls;
using Sodexo_JTH.Converters;
using Sodexo_JTH.Helpers;
using Sodexo_JTH.Interfaces;
using Sodexo_JTH.Models;
using Sodexo_JTH.Repos;
using Sodexo_JTH.Resources;
using Sodexo_JTH.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Prism.Ioc;
using DependencyService = Xamarin.Forms.DependencyService;

namespace Sodexo_JTH.ViewModels
{
    public class MealOrderPageViewModel : ViewModelBase
    {


        private ObservableCollection<mstr_meal_time> _mstrMeals;

        public ObservableCollection<mstr_meal_time> MstrMeals
        {
            get { return this._mstrMeals; }
            set { SetProperty(ref _mstrMeals, value); }
        }


        private ObservableCollection<mstr_menu_item_category> _menuCategories;

        public ObservableCollection<mstr_menu_item_category> MenuCategories
        {
            get { return this._menuCategories; }
            set { SetProperty(ref _menuCategories, value); }
        }



        private ObservableCollection<MenuItemClass> _menuItems = new ObservableCollection<MenuItemClass>();

        public ObservableCollection<MenuItemClass> MenuItems
        {
            get { return this._menuItems; }
            set { SetProperty(ref _menuItems, value); }
        }

        public mstr_meal_time SelectedMealTime { get; set; }

        public mstr_menu_item_category SelectedMenuCategory { get; set; }
        
        private bool _isMenuEnable;

        public bool IsMenuEnable
        {
            get { return this._isMenuEnable; }
            set { SetProperty(ref _isMenuEnable, value); }
        }

      

        private ObservableCollection<mstr_meal_option> _mealOptions;

        public ObservableCollection<mstr_meal_option> MealOptions
        {
            get { return this._mealOptions; }
            set { SetProperty(ref _mealOptions, value); }
        }


        private ObservableCollection<mstr_diet_type> _dietTypes;

        public ObservableCollection<mstr_diet_type> DietTypes
        {
            get { return this._dietTypes; }
            set { SetProperty(ref _dietTypes, value); }
        }

        private ObservableCollection<mstr_remarks> _remarks;

        public ObservableCollection<mstr_remarks> Remarks
        {
            get { return this._remarks; }
            set { SetProperty(ref _remarks, value); }
        }


        private mstr_meal_option _selectedMealOption;

        public mstr_meal_option SelectedMealOption
        {
            get { return this._selectedMealOption; }
            set { SetProperty(ref _selectedMealOption, value); }
        }


        private mstr_diet_type _selectedDietType;

        public mstr_diet_type SelectedDietType
        {
            get { return this._selectedDietType; }
            set { SetProperty(ref _selectedDietType, value); }
        }


        private mstr_remarks _selectedRemark;

        public mstr_remarks SelectedRemark
        {
            get { return this._selectedRemark; }
            set
            {
                SetProperty(ref _selectedRemark, value);
                if (value != null)
                {

                    SetOrderRemark(value.Name);
                }
            }
        }


        private string _orderRemarks = string.Empty;

        public string OrderRemarks
        {
            get { return this._orderRemarks; }
            set { SetProperty(ref _orderRemarks, value); }
        }
        Timer timer;

        private string _searchTextt;

        public string SearchText
        {
            get { return this._searchTextt; }
            set
            {
                SetProperty(ref _searchTextt, value);
                if (value != null)
                {
                    SearchMenu(value);
                }
            }
        }


        private string _mealClassName;
        public string MealClassName
        {
            get { return this._mealClassName; }
            set { SetProperty(ref _mealClassName, value); }
        }

        //public bool isallacartebreakfast { get; private set; }

        //public string mealclassName { get; set; }


        private bool _isAlaCarte;

        public bool isAlaCarte
        {
            get { return this._isAlaCarte; }
            set { SetProperty(ref _isAlaCarte, value); }
        }



        private mstr_menu_item_category _SelectedMenu;

        public mstr_menu_item_category SelectedMenu
        {
            get { return this._SelectedMenu; }
            set { SetProperty(ref _SelectedMenu, value); }
        }
       
        internal mstr_patient_info PatientInfo { get; private set; }

        #region SelectedMenu
        public List<mstr_allergies_master> SelectedAllergies { get; private set; }
        public List<mstr_ingredient> SelectedIngredients { get; private set; }
        public List<mstr_therapeutic> SelectedTherapeutics { get; private set; }
       
        public List<mstr_meal_type> SelectedCuisines { get; private set; }


        private mstr_others_master _others;

        public mstr_others_master others
        {
            get { return this._others; }
            set { SetProperty(ref _others, value); }
        }

        private List<mstr_others_master> otherslist;
        #endregion


        public List<Cart> Carts = new List<Cart>();


        private bool _isMenuImageVisible = true;

        public bool IsMenuImageVisible
        {
            get { return this._isMenuImageVisible; }
            set { SetProperty(ref _isMenuImageVisible, value); }
        }


        private bool _isItemAvailable;

        public bool IsItemAvailable
        {
            get { return this._isItemAvailable; }
            set { SetProperty(ref _isItemAvailable, value); }
        }


        private bool _isbtnVisible;

        public bool IsBtnVisible
        {
            get { return this._isbtnVisible; }
            set { SetProperty(ref _isbtnVisible, value); }
        }


        private ObservableRangeCollection<MenuItemClass> _menuItemClasses;

        public ObservableRangeCollection<MenuItemClass> MenuItemClasses
        {
            get { return this._menuItemClasses; }
            set { SetProperty(ref _menuItemClasses, value); }
        }

        private ObservableCollection<Language> _languages;

        public ObservableCollection<Language> Languages
        {
            get { return this._languages; }
            set { SetProperty(ref _languages, value); }
        }

       
        public ObservableCollection<MenuItemClass> TempList { get; private set; }

        public Frame _lastElementSelectedFrame { get; set; }
        public Image _lastElementSelectedImage { get; set; }
        public Label _lastElementSelectedLabel { get; set; }

        
        public Label _lastElementSelectedMealLabel { get; set; }
        public BoxView _lastElementSelectedBoxView { get; set; }


        #region Cuisine Selection
        private ObservableCollection<mstr_meal_type> _cuisines;
        public ObservableCollection<mstr_meal_type> Cuisines
        {
            get { return _cuisines; }
            set { SetProperty(ref _cuisines, value); }
        }

        private string _cuisineName;
        public string CuisineNames
        {
            get { return _cuisineName; }
            set { SetProperty(ref _cuisineName, value); }
        }

        #endregion


        #region ImageURl
        public string SetMenuImageURL = "https://www.touch2orderqa.sodexonet.com/JTH/menuimages/";
        public string MenuItemImageURL = "https://www.touch2orderqa.sodexonet.com/JTH/menuitemimgs/";
        #endregion


        public DelegateCommand LoadMoreCommand { get; set; }

        IGenericRepo<mstr_meal_time> _mstrmealRepo;
        IGenericRepo<mstr_menu_item_category> _categoryRepo;
        IGenericRepo<mstr_meal_time> _mealtimeRepo;
        IGenericRepo<mstr_bed_meal_class_mapping> _mappingRepo;
        IGenericRepo<mstr_mealclass> _mealclassRepo;
        IGenericRepo<mstr_meal_option> _mealoptionsRepo;
        IGenericRepo<mstr_diet_type> _dietTypeRepo;
        IGenericRepo<mstr_remarks> _remarksRepo;
        IGenericRepo<mstr_therapeutic> _therapeuticRepo;
        IGenericRepo<mstr_meal_order_local> _orderlocalRepo;

        public MealOrderPageViewModel(INavigationService navigationService, IGenericRepo<mstr_meal_time> mstrmealRepo, IGenericRepo<mstr_menu_item_category> categoryRepo,
            IGenericRepo<mstr_meal_time> mealtimeRepo, IGenericRepo<mstr_bed_meal_class_mapping> mappingRepo, IGenericRepo<mstr_mealclass> mealclassRepo,
            IGenericRepo<mstr_meal_option> mealoptionsRepo, IGenericRepo<mstr_diet_type> dietTypeRepo, IGenericRepo<mstr_remarks> remarksRepo,
            IGenericRepo<mstr_therapeutic> therapeuticRepo, IGenericRepo<mstr_meal_order_local> orderlocalRepo, IPageDialogService pageDialog
            ) : base(navigationService, pageDialog)
        {
            _mstrmealRepo = mstrmealRepo;
            _categoryRepo = categoryRepo;
            _mealtimeRepo = mealtimeRepo;
            _mappingRepo = mappingRepo;
            _mealclassRepo = mealclassRepo;
            _mealoptionsRepo = mealoptionsRepo;
            _dietTypeRepo = dietTypeRepo;
            _remarksRepo = remarksRepo;
            _therapeuticRepo = therapeuticRepo;
            _orderlocalRepo = orderlocalRepo;
            

            LoadMoreCommand = new DelegateCommand(LoadMore);

            PatientInfo = Library.patient;

            MenuItemClasses = new ObservableRangeCollection<MenuItemClass>();
            Languages = new ObservableCollection<Language>
            {
                new Language { Image =  "unitedkingdom.png", Title ="English"},
                new Language{Image ="thailand.png",Title="Thai"}
            };
        }

        private void LoadMore()
        {
             MenuItemClasses.AddRange(MenuItems.Skip(MenuItemClasses.Count).Take(10));
        }

        internal void ResetCuisineSelection()
        {
            if (string.IsNullOrEmpty(CuisineNames))
            {
                Cuisines = new ObservableCollection<mstr_meal_type>();
            }
            else
            {
                foreach (var cuisine in Cuisines)
                {
                    if (!CuisineNames.Split(',').Any(x => x.Equals(cuisine.meal_type_name)))
                    {
                        cuisine.IsChecked = false;
                    }

                }
            }

        }


        internal void ReloadMenuCategory()
        {
            if (_lastElementSelectedFrame != null)
            {
                VisualStateManager.GoToState(_lastElementSelectedFrame, "UnSelected");
                VisualStateManager.GoToState(_lastElementSelectedImage, "UnSelected");
                VisualStateManager.GoToState(_lastElementSelectedLabel, "UnSelected");

                _lastElementSelectedFrame = null;
                _lastElementSelectedImage = null;
                _lastElementSelectedLabel = null;
                SelectedMenuCategory = null;
                MenuItems = new ObservableCollection<MenuItemClass>();
                MenuItemClasses = new ObservableRangeCollection<MenuItemClass>();
                Carts = new List<Cart>();
                IsItemAvailable = false;
                IsMenuImageVisible = true;
            }
        }


        internal void OnCuisineSelected()
        {
            CuisineNames = string.Join(",", Cuisines.Where(x => x.IsChecked).Select(x => x.meal_type_name));

            if (!string.IsNullOrEmpty(CuisineNames))
            {
                SelectedCuisines = new List<mstr_meal_type>(Cuisines.Where(x => x.IsChecked));
                if (SelectedMenuCategory != null)
                {
                    Task.Run(async () =>
                    {
                        await SetMenuCategories(SelectedMenuCategory);
                    });

                }

            }

        }

        //public void FilterItemsOnMealTime(mstr_meal_time  mealTime)
        //{
        //    SelectedMealTime = mealTime;
        //    SetCutOffTime(SelectedMealTime);
        //}
        public void FilterItemsOnMealTime(string mealname)
        {
            SelectedMealTime = MstrMeals.FirstOrDefault(x => x.meal_name == mealname);
            SetCutOffTime(SelectedMealTime);
        }
        internal async Task AddOrRemoveMenuItem(MenuItemClass obj)
        {
            try
            {
                if (obj.ameContent == "Add to menu" || obj.ameContent == "เพิ่มเมนู")
                {
                    if (Carts.Where(o => string.Equals(SelectedMenuCategory.ID.ToString(), o.mealtimeid.ToString(), StringComparison.OrdinalIgnoreCase)).Any())
                    {
                        var msg = Library.KEY_USER_LANGUAGE != "Thai" ? "You can't select more than one item." : "คุณไม่สามารถเลือกมากกว่า 1 รายการ";
                        await PageDialog.DisplayAlertAsync("Alert!!", msg, "OK");
                    }
                    else
                    {
                        Cart ob = new Cart();
                        ob.mealitemid = obj.ID.ToString();
                        ob.mealtitleid = SelectedMealTime.ID.ToString();
                        ob.mealitemname = obj.menu_item_name;
                        ob.mealtimeid = SelectedMenuCategory.ID.ToString();
                        ob.mealtimename = SelectedMenuCategory.meal_item_name;

                        if (SelectedMenuCategory.meal_item_name.Contains("AddOn"))
                            ob.addonid = obj.ID;
                        obj.ameContent = Library.KEY_USER_LANGUAGE == "Thai" ? "เอาออก" : "Remove";
                        
                        obj.btncolor = "True";

                        Carts.Add(ob);
                    }
                }
                else
                {
                    Carts.RemoveAll((x) => x.mealitemid == obj.ID.ToString());

                    obj.ameContent = Library.KEY_USER_LANGUAGE == "Thai" ? "เพิ่มเมนู" : "Add to menu";
                    obj.btncolor = "False";
                }
            }
            catch (Exception excp)
            {

            }
        }

        private void SetOrderRemark(string value)
        {
            if (!OrderRemarks.Contains(value))
            {
                if (value != "No")
                {
                    if (string.IsNullOrEmpty(OrderRemarks))
                    {
                        string cText = value + ",";
                        OrderRemarks = cText;
                    }
                    else
                    {
                        string cText = OrderRemarks + Environment.NewLine + value + ",";

                        OrderRemarks = cText;
                    }
                }
            }

        }
        public async void PlaceOrder()
        {
            IsPageEnabled = true;
           
           try
           {
               OrderRemarks = OrderRemarks.Replace("\r", "");

                if (others.others_name == "Clear Feeds" || others.others_name == "Full Feeds" || others.others_name == "Clear Soup")
                {
                    var clearCarts = Carts.Where(item => item.mealtimename.ToLower().Contains("Fruit".ToLower())|| item.mealtimename.ToLower().Contains("beverages".ToLower()) || item.mealtimename.ToLower().Contains("Soup".ToLower())).ToList();
                    if (!clearCarts.Any())
                    {
                        var msg = Library.KEY_USER_LANGUAGE != "Thai" ? "Please select any items from Soup,Beverages,Fruit menu and select Meal Option." : "โปรดเลือกรายการใด ๆ จากซุป, เครื่องดื่ม, เมนูผลไม้และเลือกตัวเลือกอาหาร";
                        await PageDialog.DisplayAlertAsync("Alert!!", msg, "OK");
                        IsPageEnabled = false;
                        return;
                    }
                }
                else
                {
                    bool isentreeselected = false;

                    var newList = Carts.Where(item => item.mealtimename.ToLower().Contains("entree".ToLower()) || item.mealtimename.ToLower().Contains("entrée".ToLower())).ToList();

                    int count = newList.Count;

                    if (count > 0)
                        isentreeselected = true;
                    else
                        isentreeselected = false;

                    if (SelectedMealOption.ID == 0 && isentreeselected == false)
                    {
                        if (others.ID == 1 || others.others_name.Contains("TC") || others.others_name.Contains("To Collect") || others.ID == 8 || others.ID == 5 || others.others_name.Contains("NM"))
                        {

                        }
                        else
                        {
                            var msg = Library.KEY_USER_LANGUAGE != "Thai" ? "Please select any items from Entree menu Or select meal option." : "โปรดเลือกรายการใด ๆ จากเมนู ชุดอาหาร หรือเลือกตัวเลือกอาหาร";
                            await PageDialog.DisplayAlertAsync("Alert!!", msg, "OK");
                            IsPageEnabled = false;
                            return;
                        }

                    }
                }

                var localOrders = _orderlocalRepo.QueryTable().Where(x => x.P_id == PatientInfo.ID && x.meal_time_id == SelectedMealTime.ID);

                if (localOrders.Count() > 0)
                {
                    var msg = Library.KEY_USER_LANGUAGE != "Thai" ? "Order is already Placed in offline mode for this Patient. " + "Please Sync it from patient details screen." : $"คำสั่งซื้อถูกวางไว้แล้วในโหมดออฟไลน์สำหรับผู้ป่วยรายนี้. กรุณาซิงค์จากหน้าจอรายละเอียดผู้ป่วย";
                    await PageDialog.DisplayAlertAsync("Alert!!", msg, "OK");

                    return;
                }


                    bool Check_order_result = await Check_Order_Taken(Library.KEY_CHECK_ORDER_DATE, PatientInfo.ID, SelectedMealTime.meal_name, Convert.ToInt32(Library.KEY_ORDER_ID));
                    if (others.ID != 1 || others.ID == 8)
                    {

                        if (others.others_name != "TC")
                        {
                            if (Check_order_result == false)
                            {
                                await NavigateToMealSummary();
                            }
                            else
                            {
                                var msg = Library.KEY_USER_LANGUAGE != "Thai" ? $"{SelectedMealTime.meal_name} meal Order is already placed." : $"{SelectedMealTime.meal_name} อาหารที่สั่งซื้อจะถูกวางไว้แล้ว";
                                await PageDialog.DisplayAlertAsync("Alert!!", msg, "OK");
                            }

                        }
                        else
                        {
                            if (Check_order_result == true)
                        {
                            var msg = Library.KEY_USER_LANGUAGE != "Thai" ? $"Meal order is already placed. Please Sync it from patient details screen." : $" สั่งอาหารเรียบร้อยแล้ว. กรุณาซิงค์จากหน้าจอรายละเอียดผู้ป่วย";
                            await PageDialog.DisplayAlertAsync("Alert!!", msg, "OK");
                        }
                                
                            else
                            {
                                Library.KEY_langchangedfrommealpage = "no";
                                await NavigateToMealSummary();
                            }
                        }
                    }
                    else
                    {
                        if (Check_order_result == true)
                    {
                        var msg = Library.KEY_USER_LANGUAGE != "Thai" ? $"Meal order is already placed. Please Sync it from patient details screen." : $" สั่งอาหารเรียบร้อยแล้ว. กรุณาซิงค์จากหน้าจอรายละเอียดผู้ป่วย";
                        await PageDialog.DisplayAlertAsync("Alert!!", msg, "OK");
                    }
                           
                        else
                            await NavigateToMealSummary();
                    }
                    
            }
                catch (Exception)
                {
                    IsPageEnabled = false;
                }
            IsPageEnabled = false;
        }

        public async Task NavigateToMealSummary()
        {
            NavigationParameters navparam = new NavigationParameters();
            navparam.Add("Patient", PatientInfo);
            navparam.Add("mealOption", SelectedMealOption);
            navparam.Add("dietType", SelectedDietType);
            navparam.Add("remark", OrderRemarks);
            navparam.Add("MealTime", SelectedMealTime);
            navparam.Add("Therapeutics", SelectedTherapeutics);
            navparam.Add("Ingredients", SelectedIngredients);
            navparam.Add("Allergies", SelectedAllergies);
            navparam.Add("Cuisines", SelectedCuisines);
            //navparam.Add("DietTextures", SelectedDietTextures);
            navparam.Add("Carts", Carts);
            navparam.Add("Others", others);
            navparam.Add("OthersList", otherslist);

            await NavigationService.NavigateAsync("MealSummaryPage", navparam);
        }

        public async Task<bool> Check_Order_Taken(string order_date, int p_id, string meal_time, int order_id)
        {
            try
            {

                if (meal_time.ToLowerInvariant() == "Breakfast".ToLowerInvariant())
                {
                    meal_time = "BF";
                }
                if (meal_time.ToLowerInvariant() == "Lunch".ToLowerInvariant())
                {
                    meal_time = "LH";
                }
                if (meal_time.ToLowerInvariant() == "Dinner".ToLowerInvariant())
                {
                    meal_time = "DN";
                }

                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    HttpClient httpClient = new System.Net.Http.HttpClient();

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_CHECKORDERTAKEN + "/" + order_date + "/" + p_id + "/" + meal_time + "/" + order_id);
                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    // jarray= await response.Content.ReadAsStringAsync();
                    var data = await response.Content.ReadAsStringAsync();
                    //JArray jarray = JArray.Parse(data);
                    if (data == "true")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    if (meal_time == "BF")
                    {
                        var order = _orderlocalRepo.QueryTable().Where(x => x.patient_id == PatientInfo.ID && x.BF == 1 && x.order_date == Library.KEY_ORDER_DATE);
                        if (order.Any())
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                    else if (meal_time == "LH")
                    {
                        var order = _orderlocalRepo.QueryTable().Where(x => x.patient_id == PatientInfo.ID && x.LH == 1 && x.order_date == Library.KEY_ORDER_DATE);
                        if (order.Any())
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                    else if (meal_time == "DN")
                    {
                        var order = _orderlocalRepo.QueryTable().Where(x => x.patient_id == PatientInfo.ID && x.DN == 1 && x.order_date == Library.KEY_ORDER_DATE);
                        if (order.Any())
                        {
                            return true;
                        }
                        else
                            return false;
                    }

                    return false;
                }

            }
            catch (Exception excp)
            {
                return false;
                //// stop progressring
                //myring.IsActive = false;
                //myring.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            }
        }


        private void LoadData()
        {

            int bedClassID = Convert.ToInt32(PatientInfo.Bed_Class_ID);

            var bedMapping = _mappingRepo.QueryTable().Where(x => x.bed_class_id == bedClassID).FirstOrDefault();

            var mealClass = _mealclassRepo.QueryTable().First(x => x.ID == bedMapping.meal_class_id);

            MealClassName = mealClass.meal_Class_name;

            isAlaCarte = mealClass.is_A_la_carte;

            MealOptions = new ObservableCollection<mstr_meal_option>(_mealoptionsRepo.QueryTable().Where(x => x.status_id == 1));

           mstr_meal_option option = new mstr_meal_option
            {
                meal_option_name = "No",
                ID = 0
            };
            MealOptions.Insert(0, option);
            SelectedMealOption = MealOptions.FirstOrDefault(); 

            DietTypes = new ObservableCollection<mstr_diet_type>(_dietTypeRepo.QueryTable().Where(x => x.status_id == 1));

            DietTypes.Insert(0, new mstr_diet_type { ID = 0, diet_name = "NO" });

            SelectedDietType = DietTypes.FirstOrDefault();


            Remarks = new ObservableCollection<mstr_remarks>(_remarksRepo.QueryTable().Where(x => x.Status_id == 1));
            var defaultRemark = new mstr_remarks
            {
                ID = 0,
                Name = "No"
            };
            Remarks.Insert(0, defaultRemark);

            SelectedRemark = Remarks.First();

            MstrMeals = new ObservableCollection<mstr_meal_time>(_mstrmealRepo.QueryTable().Where(x => x.status_id == 1).OrderBy(x => x.ID));


            var  tempMenuCategories = new ObservableCollection<mstr_menu_item_category>();
            foreach (var item in _categoryRepo.QueryTable())
            {
                string ss = item.meal_item_name;
                if (ss.ToLower().Contains("entrée") || ss.ToLower().Contains("entree"))
                {
                    item.imgname = PlatFromImage.GetImage("entreebtn.png");
                    item.selimgname = PlatFromImage.GetImage("entreebtnover.png");
                    item.fcolor = "Transparent";
                    item.selcolor = "#FF2a295c";
                }
                else if (ss.ToLower().Contains("soup"))
                {
                    item.imgname = PlatFromImage.GetImage("soupbtn.png");
                    item.selimgname = PlatFromImage.GetImage("soupbtnover.png");
                    item.fcolor = "Transparent";
                    item.selcolor = "#FF2a295c";


                }
                else if (ss.ToLower().Contains("beverage"))
                {
                    item.imgname = PlatFromImage.GetImage("beveragebtn.png");
                    item.selimgname = PlatFromImage.GetImage("beveragebtnover.png");
                    item.fcolor = "Transparent";
                    item.selcolor = "#FF2a295c";

                }
                else if (ss.ToLower().Contains("dessert"))
                {
                    item.imgname = PlatFromImage.GetImage("dessertbtn.png");
                    item.selimgname = PlatFromImage.GetImage("dessertbtnover.png");
                    item.fcolor = "Transparent";
                    item.selcolor = "#FF2a295c";
                }
                else if (ss.ToLower().Contains("juice"))
                {
                    item.imgname = PlatFromImage.GetImage("juicebtn.png");
                    item.selimgname = PlatFromImage.GetImage("juicebtnover.png");
                    item.fcolor = "Transparent";
                    item.selcolor = "#FF2a295c";
                }
                else
                {
                    item.imgname = PlatFromImage.GetImage("entreebtn.png");
                    item.selimgname = PlatFromImage.GetImage("entreebtnover.png");
                    item.fcolor = "Transparent";
                    item.selcolor = "#FF2a295c";

                }
                
                if (others.others_name == "Clear Feeds" && (ss.ToLower().Contains("entrée") || ss.ToLower().Contains("entree")  || ss.ToLower().Contains("dessert") ))
                {
                    item.CategoryVisibility = false;
                }
                else if (others.others_name == "Full Feeds" && (ss.ToLower().Contains("entrée") || ss.ToLower().Contains("entree") || ss.ToLower().Contains("dessert") ))
                {
                    item.CategoryVisibility = false;
                }
                else if (others.others_name == "Clear Soup" && (ss.ToLower().Contains("entrée") || ss.ToLower().Contains("entree") || ss.ToLower().Contains("dessert")))
                {
                    item.CategoryVisibility = false;
                }
                //if ((others.others_name == "Clear Feeds" || others.others_name == "Full Feeds" || others.others_name == "T&A") && (ss.ToLower().Contains("entrée") || ss.ToLower().Contains("entree")))
                //    item.CategoryVisibility = false;
                //else if (others.others_name == "No Meal" || others.others_name == "NM")
                //    item.CategoryVisibility = false;
                else
                    item.CategoryVisibility = true;


                #region AlaCarte
                //if (others.others_name == "Full Feeds" || others.others_name == "T&A" || others.others_name == "Clear Feeds")
                //{

                //}
                //else if ((item.ID == 1 || item.ID == 3) && !isAlaCarte)
                //    item.CategoryVisibility = false;

                #endregion

                if (ss.ToLower().Contains("entrée") || ss.ToLower().Contains("entree"))
                    tempMenuCategories.Insert(0, item);
                else
                    tempMenuCategories.Add(item);
            }

            MenuCategories = new ObservableCollection<mstr_menu_item_category>(tempMenuCategories);
        }

        private async Task CheckOrder()
        {
            int order_id = 0;
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    string check_date = Library.KEY_CHECK_ORDER_DATE;
                    System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                    System.Net.Http.HttpRequestMessage request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, Library.URL + "/" + Library.METHODE_CHECKORDER + "/" + check_date + "/" + PatientInfo.ID);
                    System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);
                    // jarray= await response.Content.ReadAsStringAsync();
                    var data = await response.Content.ReadAsStringAsync();
                    JArray jarray = JArray.Parse(data);
                    for (int i = 0; i < jarray.Count; i++)
                    {

                        JObject row = JObject.Parse(jarray[i].ToString());
                        order_id = Convert.ToInt32(row["order_id"].ToString());
                    }
                    Library.KEY_ORDER_ID = order_id.ToString();
                }

            }
            catch (Exception excp)
            {
                Library.KEY_ORDER_ID = order_id.ToString();
            }
        }

        public async Task SetMenuCategories(mstr_menu_item_category menuitem)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsPageEnabled = true;
                IsMenuImageVisible = false;
            });

            var sqlite = DependencyService.Get<IDBInterface>().GetConnection();
            var MasterMenuInfo = sqlite.Table<mstr_menu_master>();
            if (!MasterMenuInfo.Any())
            {
                var msg = Library.KEY_USER_LANGUAGE != "Thai" ? "Please sync 'Sync Menu Items' from drawer menu to proceed further." : "โปรดซิงค์ 'ซิงค์รายการเมนู' จากเมนูลิ้นชักเพื่อดำเนินการต่อไป";
                Device.BeginInvokeOnMainThread(async() =>
                {
                    await PageDialog.DisplayAlertAsync("Alert!!", msg, "OK");
                });
                Library.KEY_SYNC_NOTIFICATION = "1";
                IsPageEnabled = false;
                return;
            }

            MenuItems = new ObservableCollection<MenuItemClass>();
            MenuItemClasses = new ObservableRangeCollection<MenuItemClass>();
            if (menuitem.meal_item_name.ToLower().Contains("entrée") || menuitem.meal_item_name.ToLower().ToLower().Contains("entree"))
            {
                //if (Title.Contains("Cut off time exceeds"))
                //{
                //    if (isallacartebreakfast)
                //    {
                //        GetsetMenuItems(menuitem);
                //    }
                //    else
                //        MenuItems = new ObservableCollection<MenuItemClass>();
                //}
                //else
                    GetsetMenuItems(menuitem);
            }
            else
                await GetMenuItems(menuitem);

            

            Device.BeginInvokeOnMainThread(() =>
            {
                TempList = new ObservableCollection<MenuItemClass>(MenuItems);

                if (MenuItems.Any())
                    IsItemAvailable = false;
                else
                    IsItemAvailable = true;


                IsPageEnabled = false;
            });
           
        }

        private void GetsetMenuItems(mstr_menu_item_category menuitem)
        {
            try
            {

                string diet_code = "";
                List<string> th_strings = new List<string>();
                List<string> diet_strings = new List<string>();
                List<string> cuisine_strings = new List<string>();

                string thString = "";
                string cuisinea = "";
                string ingredients = "";
                string allergies = "";
                string diets = "";


                //for (int i = 0; i < SelectedDietTextures.Count; i++)
                //{
                //    diet_code += SelectedDietTextures[i].diet_texture_name + ",";
                //}

                FilterItems(ref ingredients, ref allergies, ref diets, ref thString, ref cuisinea);


                string date = Library.KEY_ORDER_DATE.ToString();
                string selected_day = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture).DayOfWeek.ToString();

                var db = DependencyService.Get<IDBInterface>().GetConnection();
                string query = "";


                string query2 = "select meal_class_id,Is_A_la_carte from mstr_bed_meal_class_mapping where bed_class_id = '" + Convert.ToInt32(PatientInfo.Bed_Class_ID) + "' and status_id=1";
                var mealid = db.Query<mstr_bed_meal_class_mapping>(query2).FirstOrDefault();
                int id = mealid.meal_class_id;


               

                if (SelectedCuisines.Count > 0)
                {
                    query = "Select * From mstr_menu_master where status_id=1 and menu_time_name like '%" + SelectedMealTime.meal_name + "%' and menu_days like '%" + selected_day + "%' and meal_class_ids like '%" + id + "%' and wardtypename like '%" + PatientInfo.wardtypename + "%'  and (" + cuisinea + ")";
                }
                else
                {
                    query = "Select * From mstr_menu_master where status_id=1 and menu_time_name like '%" + SelectedMealTime.meal_name + "%' and menu_days like '%" + selected_day + "%' and meal_class_ids like '%" + id + "%' and wardtypename like '%" + PatientInfo.wardtypename + "%'";
                }

                if (SelectedTherapeutics.Count > 0)
                {
                    
                    query += " and (" + thString + ")";
                }

                if (Library.InfantDietEnable)
                {
                    query += "and is_InfantDiet order by id";
                }

                query += " order by id";
               // query = "Select ID,menu_item_name,menu_item_description,meal_class_name,ImageData ,menu_item_name_local_language From mstr_menu_master where status_id=1 and mealTime_names like '%" + Meal_Time + "%' and menu_days like '%" + selected_day + "%' and meal_class_id='" + Convert.ToInt32(Library.KEY_PATIENT_BED_CLASS_ID)) + "' and MealType like '%" + cuisine + "%'";
               var abc = db.Query<mstr_menu_master>(query);
                // List<mstr_menu_master> mylist = new List<mstr_menu_master>();

                // menuitemlistobservable mylist = new menuitemlistobservable();
                //
                //var a =  abc.Where(x => x.menu_code == "Thai 13");

                var tempData = new List<mstr_menu_master>();

                if (SelectedTherapeutics.Count > 0)
                {
                    foreach (var menu_master in abc)
                    {
                        var ids = menu_master.Therapeutic_ids.Split(',');
                        foreach (var itemID in ids)
                        {
                            if (!string.IsNullOrEmpty(itemID))
                            {
                                var selectedTheraID = SelectedTherapeutics.Select(x => x.ID);
                                foreach (var ID in selectedTheraID)
                                {
                                    if (ID == Convert.ToInt32(itemID))
                                    {

                                        tempData.Add(menu_master);

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (SelectedCuisines.Count > 0)
                {
                    foreach (var item in abc)
                    {
                        var names = item.meal_type_name.Split(',');
                        foreach (var name in names)
                        {
                            if (!string.IsNullOrEmpty(name))
                            {
                                var selectedcusinename = SelectedCuisines.Select(x => x.meal_type_name);
                                foreach (var ID in selectedcusinename)
                                {
                                    if (ID == name)
                                    {

                                        tempData.Add(item);

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                 else
                    tempData = new List<mstr_menu_master>(abc);


               

                ConditionsFilterSetMenu(tempData.Distinct().ToList(), ingredients, allergies, diets, diet_code, thString, db, others.others_name);


                MenuItemClasses = new ObservableRangeCollection<MenuItemClass>(MenuItems.Take(10));

            }
            catch (Exception)
            {
            }
        }

        private async Task GetMenuItems(mstr_menu_item_category menuitem)
        {
            try
            {
                var db = DependencyService.Get<IDBInterface>().GetConnection();
                string query = "";
                string ingredients = "";
                string allergies = "";
                string diets = "";
                List<string> th_strings = new List<string>();
                string thString = "";
                string cuisinea = string.Empty;

                FilterItems(ref ingredients, ref allergies, ref diets, ref thString, ref cuisinea);

                int halal = 1;
                int veg = 1;
                if (Convert.ToBoolean(PatientInfo.isveg))
                    veg = 1;
                else
                    veg = 0;

                if (Convert.ToBoolean(PatientInfo.ishalal))
                    halal = 1;
                else
                    halal = 0;

                query = ConditionsFilterMenuItem(ingredients, allergies, menuitem.meal_item_name, SelectedMealTime.meal_name, halal, veg, diets, cuisinea);


                
                    if (SelectedTherapeutics.Count > 0)
                    {
                        query += " and (" + thString + ") ";
                    }

                    

             

                query += " order by id";
                //-------------------------------------

                

               var queryData = db.Query<mstr_menu_item>(query);

                var tempData = new List<mstr_menu_item>();

                if (SelectedTherapeutics.Count > 0)
                {
                    foreach (var menu_item in queryData)
                    {
                        var ids = menu_item.Therapeutic_ids.Split(',');
                        foreach (var itemID in ids)
                        {
                            if (!string.IsNullOrEmpty(itemID))
                            {
                                var selectedTheraID = SelectedTherapeutics.Select(x => x.ID);
                                foreach (var ID in selectedTheraID)
                                {
                                    if (ID == Convert.ToInt32(itemID))
                                    {

                                        tempData.Add(menu_item);

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (SelectedCuisines.Count > 0)
                {
                    foreach (var item in queryData)
                    {
                        var names = item.meal_type_name.Split(',');
                        foreach (var name in names)
                        {
                            if (!string.IsNullOrEmpty(name))
                            {
                                var selectedcusinename = SelectedCuisines.Select(x => x.meal_type_name);
                                foreach (var ID in selectedcusinename)
                                {
                                    if (ID == name)
                                    {

                                        tempData.Add(item);

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    tempData = new List<mstr_menu_item>(queryData);


                string culture = System.Globalization.CultureInfo.CurrentCulture.Name;
                //  List<mstr_menu_item> Mylist = new List<mstr_menu_item>();
                foreach (var item in tempData.Distinct())
                {
                    //class
                    int classcounter = 0;

                    if (!String.IsNullOrEmpty(item.meal_class_name))
                    {
                        string[] cycles = item.meal_class_name.Split(',').ToArray();
                        foreach (var cy1 in cycles)
                        {
                            if (!String.IsNullOrEmpty(cy1))
                            {
                                if (cy1 == MealClassName)
                                {
                                    classcounter++;
                                }
                            }

                        }
                    }

                    MenuItemClass obj = new MenuItemClass();
                    if (Library.KEY_IS_CARE_GIVER == "no")
                    {
                        if (classcounter > 0)//&& isSetmenuOk
                        {
                            if (Library.KEY_langchangedfrommealpage == "yes")
                            {
                                if ((Carts.Count > 0 && Carts.Any(x => x.mealitemid == item.ID.ToString())))
                                {
                                    obj.ameContent = Library.KEY_USER_LANGUAGE == "Thai" ? "เอาออก" : "Remove";
                                    obj.btncolor = "True";
                                }
                                else
                                {
                                    obj.ameContent = Library.KEY_USER_LANGUAGE == "Thai" ? "เพิ่มเมนู" : "Add to menu";
                                    obj.btncolor = "False";
                                }

                            }
                            else
                            {


                                if ((Carts.Count > 0 && Carts.Any(x => x.mealitemid == item.ID.ToString())))
                                {
                                    obj.ameContent = Library.KEY_USER_LANGUAGE == "Thai" ? "เอาออก" : "Remove";
                                    obj.btncolor = "True";
                                }
                                else
                                {
                                    obj.ameContent = Library.KEY_USER_LANGUAGE == "Thai" ? "เพิ่มเมนู" : "Add to menu";
                                    obj.btncolor = "False";
                                }

                            }
                            obj.ID = item.ID;
                            obj.ingredient_name = item.ingredient_name;
                            obj.ImageData = item.ImageData;
                            // ob.ID = item.ID;
                            obj.meal_type_id = item.meal_type_id;
                            obj.menu_image =$"{MenuItemImageURL}{item.menu_image}";
                            if (Library.KEY_USER_LANGUAGE == "Thai")//(culture == "th")
                            {

                                obj.menu_item_name = item.menu_item_code + " - " + item.local_language ?? "N/A";

                            }
                            else
                            {
                                obj.menu_item_name = item.menu_item_code + " - " + item.menu_item_description ?? "N/A";
                            }

                            obj.menu_item_description = item.menu_item_description ?? "N/A";
                            obj.LocalLanguage = item.menu_item_code + " - " + item.local_language ?? "N/A";
                            obj.mealTime_names = item.mealTime_names;
                            obj.site_code = item.sitecode;
                            obj.ingredient_name = item.ingredient_name;
                            obj.menu_code = item.menu_item_code;
                            MenuItems.Add(obj);
                          

                        }
                    }
                    else if (Library.KEY_IS_CARE_GIVER == "yes")
                    {

                        if (classcounter > 0 && item.is_visitor)//&& isSetmenuOk
                        {
                            if (Library.KEY_langchangedfrommealpage == "yes")
                            {
                                obj.ameContent = Library.KEY_USER_LANGUAGE == "Thai" ? "เอาออก" : "Remove"; //loader.GetString("rm").ToString();
                                obj.btncolor = "True";

                            }
                            else
                            {
                                obj.ameContent = Library.KEY_USER_LANGUAGE == "Thai" ? "เพิ่มเมนู" : "Add to menu";
                                obj.btncolor = "False";
                            }

                            obj.ID = item.ID;
                            obj.ingredient_name = item.ingredient_name;
                            obj.ImageData = item.ImageData;
                            obj.LocalLanguage = item.local_language;
                            obj.meal_type_id = item.meal_type_id;
                            obj.menu_image = item.menu_image;
                            if (Library.KEY_USER_LANGUAGE == "Thai")//(culture == "th")
                            {

                                obj.menu_item_name = item.menu_item_code + " - " + item.local_language ?? "N/A";

                            }
                            else
                            {
                                obj.menu_item_name = item.menu_item_code + " - " + item.menu_item_description ?? "N/A";
                            }

                            obj.menu_item_description = item.menu_item_description;
                            obj.menu_code = item.menu_item_code;
                            obj.mealTime_names = item.mealTime_names;
                            obj.site_code = item.sitecode;
                            obj.ingredient_name = item.ingredient_name;

                             MenuItems.Add(obj);
                           

                        }

                    }
                }

                MenuItemClasses = new ObservableRangeCollection<MenuItemClass>(MenuItems.Take(10));

            }
            catch (Exception exp)
            {

                await PageDialog.DisplayAlertAsync("Alert!!", exp.Message, "OK");


            }
        }

        private void FilterItems(ref string ingredients, ref string allergies, ref string diets, ref string thString, ref string cuisinea)
        {

            //if (SelectedTherapeutics.Any())
            //{
            //    if (SelectedTherapeutics.Count > 1)
            //    {
            //        string Priority = SelectedTherapeutics.Min(x => x.Thera_Priority);
            //        var lowPriorityThera = SelectedTherapeutics.Where(x => x.Thera_Priority == Priority).ToList();
            //        for (int i = 0; i < lowPriorityThera.Count; i++)
            //        {
            //            if (i == 0)
            //            {
            //                thString = " TH_code like '%" + lowPriorityThera[i].TH_code.Replace("'", "'\'") + "%'";
            //            }
            //            else
            //            {
            //                thString = thString + " OR TH_code like '%" + lowPriorityThera[i].TH_code.Replace("'", "'\'") + "%'";
            //            }
            //        }
            //    }
            //    else
            //    {
            //        thString = " TH_code like '%" + SelectedTherapeutics.First().TH_code.Replace("'", "'\'") + "%'";
            //    }
            //}


            for (int i = 0; i < SelectedTherapeutics.Count; i++)
            {
                if (i == 0)
                {
                    thString = " Therapeutic_ids like '%" + SelectedTherapeutics[i].ID.ToString().Replace("'", "'\'") + "%'";
                }
                else
                {
                    thString = thString + " OR Therapeutic_ids like '%" + SelectedTherapeutics[i].ID.ToString().Replace("'", "'\'") + "%'";
                }
            }


            for (int j = 0; j < SelectedIngredients.Count; j++)
            {
                var list = SelectedIngredients[j].ingredient_name.Replace("'", "'\'") + ",";

                ingredients += "'" + list + "',";
            }
            if (ingredients.Length > 0)
                ingredients = ingredients.Remove(ingredients.Length - 1);


            for (int i = 0; i < SelectedAllergies.Count; i++)
            {
                if (i == 0)
                {
                    allergies = " allergies like '%" + SelectedAllergies[i].allergies_name.Replace("'", "'\'") + "%'";
                }
                else
                {
                    allergies = allergies + " OR allergies like '%" + SelectedAllergies[i].allergies_name.Replace("'", "'\'") + "%'";
                }
            }

            //for (int i = 0; i < SelectedDietTextures.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        diets = " texture_names like '%" + SelectedDietTextures[i].diet_texture_name.Replace("'", "'\'") + "%'";
            //    }
            //    else
            //    {
            //        diets = diets + " OR texture_names like '%" + SelectedDietTextures[i].diet_texture_name.Replace("'", "'\'") + "%'";
            //    }
            //}

            for (int i = 0; i < SelectedCuisines.Count; i++)
            {
                if (i == 0)
                {
                    cuisinea = " meal_type_name like '%" + SelectedCuisines[i].meal_type_name.Replace("'", "'\'") + "%'";
                }
                else
                {
                    cuisinea = cuisinea + " OR meal_type_name like '%" + SelectedCuisines[i].meal_type_name.Replace("'", "'\'") + "%'";
                }
            }
        }

        private string ConditionsFilterMenuItem(string ingredients, string allergies, string meal_item_name, string Meal_Time, int halal, int veg, string diets, string cuisinea)
        {

            string query = string.Empty;


            if (halal == 1 && veg == 1)
                query = "Select * From mstr_menu_item where meal_item_name='" + meal_item_name + "' and meal_class_name like '%" + MealClassName + "%' and ward_type_name like '%" + PatientInfo.wardtypename + "%' and mealTime_names like '%" + Meal_Time + "%' and is_halal=" + halal + " and is_vegitarian=" + veg;
            else if (halal == 1)
                query = "Select * From mstr_menu_item where meal_item_name='" + meal_item_name + "' and meal_class_name like '%" + MealClassName + "%' and ward_type_name like '%" + PatientInfo.wardtypename + "%' and mealTime_names like '%" + Meal_Time + "%' and is_halal=" + halal;
            else if (veg == 1)
                query = "Select * From mstr_menu_item where meal_item_name='" + meal_item_name + "' and meal_class_name like '%" + MealClassName + "%' and ward_type_name like '%" + PatientInfo.wardtypename + "%' and mealTime_names like '%" + Meal_Time + "%' and is_vegitarian=" + veg;
            else
                query = "Select * From mstr_menu_item where meal_item_name='" + meal_item_name + "' and meal_class_name like '%" + MealClassName + "%' and ward_type_name like '%" + PatientInfo.wardtypename + "%' and mealTime_names like '%" + Meal_Time + "%'";


            query += " and is_visitor=0";

            if (Library.IsFAGeneralEnable == null || Library.IsFAGeneralEnable == true)
            {
                if (SelectedAllergies.Count > 0)
                {

                    query += " and allergies not in (Select allergies From mstr_menu_item where " + allergies + ")";
                }

            }


            if (SelectedIngredients.Count > 0)
            {
                query += " and ingredient_name not in (" + ingredients + ")";
            }
            if (SelectedCuisines.Count > 0)
            {
                query += " and (" + cuisinea + ")";
            }
            
            return query;
        }

        private void ConditionsFilterSetMenu(List<mstr_menu_master> masters, string ingredients, string allergies, string diets, string diet_code, string thString, SQLiteConnection db, string others)
        {
            foreach (mstr_menu_master item in masters)
            {
               
                int cntingredients = 0;
                int cntallergies = 0;
                
                if (!string.IsNullOrEmpty(item.menu_item_ides))
                {
                    foreach (var itemdetail in item.menu_item_ides.Split(','))
                    {
                        if (!String.IsNullOrEmpty(itemdetail))
                        {
                            string q1;
                            if (!String.IsNullOrEmpty(ingredients))
                            {
                                string q2 = "select * from mstr_menu_item where ID ='" + itemdetail + "' and ingredient_name IN (" + ingredients + ") and is_visitor=0";
                                cntingredients += db.Query<mstr_menu_item>(q2).Count;
                            }
                            else
                                cntingredients = 0;

                            if (Library.IsFAGeneralEnable == null || Library.IsFAGeneralEnable == true)
                            {
                                if (!string.IsNullOrEmpty(allergies))
                                {
                                    q1 = "select * from mstr_menu_item where ID ='" + itemdetail + "' and (" + allergies + ") and is_visitor=0";
                                    cntallergies += db.Query<mstr_menu_item>(q1).Count;
                                }
                            }
                           
                        }

                    }
                    SetEntree(item, cntingredients, cntallergies, db);
                }
            }

           
        }

        private void SetEntree(mstr_menu_master item, int cntingredients, int cntallergies,  SQLiteConnection db)
        {

            int cyclecounter = 0;

            if (!String.IsNullOrEmpty(item.cycle_name))
            {
                string[] cycles = item.cycle_name.Split(',').ToArray();
                foreach (var cy1 in cycles)
                {
                    if (!String.IsNullOrEmpty(cy1))
                    {
                        string odate = DateTime.ParseExact(Library.KEY_ORDER_DATE, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

                        string cyncle = "select * from mstr_Cycledetails where cycle_name = '" + cy1 + "' AND '" + odate + "' BETWEEN formattedfrom AND formattedto  AND status_id = 1";
                        cyclecounter += db.Query<mstr_Cycledetails>(cyncle).Count;

                    }

                }
            }



            MenuItemClass ob = new MenuItemClass();
            int Pveg = (Convert.ToBoolean(PatientInfo.isveg)) ? 1 : 0;
            int Phalal = (Convert.ToBoolean(PatientInfo.ishalal)) ? 1 : 0;

            //                    isnonveg
            //isnonhalal
            bool isSetmenuOk = false;
            if (Phalal == 0 && Pveg == 0)
            {
                isSetmenuOk = true;
            }
            if (Phalal == 1 && Pveg == 0)
            {
                if (item.is_halal)
                {
                    isSetmenuOk = true;
                }
            }
            if (Phalal == 0 && Pveg == 1)
            {
                if (item.is_veg)
                {
                    isSetmenuOk = true;
                }
            }
            if (Phalal == 1 && Pveg == 1)
            {
                if (item.is_veg && item.is_halal)
                {
                    isSetmenuOk = true;
                }
            }

            bool isconfinementpresent = false;
            if (Library.IsConfinementEnable)
            {
                if (item.Confinement)
                    isconfinementpresent = true;
                else
                    isconfinementpresent = false;
            }
            else
                isconfinementpresent = true;


            //
            //if (((others == "DOC" && isthere <=0) || (others != "DOC" && isthere > 0)) && cntingredients <= 0 && cntallergies <= 0 && cntdiets > 0 && isSetmenuOk && cyclecounter > 0 && isconfinementok == true)//&& isSetmenuOk
            if (cntingredients <= 0 && cntallergies <= 0  && isSetmenuOk && cyclecounter > 0 && isconfinementpresent)//&& isSetmenuOk
            {
                
                //DependencyService.Get<IDownloadService>().DownloadImage(item.menu_image, url);

                if (Library.KEY_langchangedfrommealpage == "yes")
                {

                    if ((Carts.Count > 0 && Carts.Any(x => x.mealitemid == item.ID.ToString())))
                    {
                        ob.ameContent = Library.KEY_USER_LANGUAGE == "Thai" ? "เอาออก" : "Remove";
                       
                        ob.btncolor = "True";
                    }
                    else
                    {
                        ob.ameContent = Library.KEY_USER_LANGUAGE == "Thai" ? "เพิ่มเมนู" : "Add to menu";
                        ob.btncolor = "False";

                    }

                }
                else
                {
                    if ((Carts.Count > 0 && Carts.Any(x => x.mealitemid == item.ID.ToString())))
                    {
                        ob.ameContent = Library.KEY_USER_LANGUAGE == "Thai" ? "เอาออก" : "Remove";
                        ob.btncolor = "True";
                    }
                    else
                    {
                        ob.ameContent = Library.KEY_USER_LANGUAGE == "Thai" ? "เพิ่มเมนู" : "Add to menu";
                        ob.btncolor = "False";
                    }

                }
               
                ob.age_id = item.age_id;
                ob.diet_id = item.diet_id;
                ob.Confinement = item.Confinement;
                ob.ID = item.ID;
                ob.ingredient_name = item.ingredient_name;
                ob.ImageData = item.ImageData;
                ob.ID = item.ID;
                if (Library.KEY_USER_LANGUAGE == "Thai")
                {
                    ob.menu_name = item.menu_item_name_local_language ?? "N/A";
                    ob.menu_item_name = item.menu_code + " - " + item.local_language ?? "N/A";
                }
                else
                {
                    ob.menu_name = item.menu_name ?? "N/A";
                    ob.menu_item_name = item.menu_code + " - " + item.menu_description ?? "N/A";
                }

                ob.LocalLanguage = item.menu_code + " - " + item.local_language ?? "N/A";
                //var path = DependencyService.Get<IDownloadService>().GetImage(item.menu_image);
                ob.menu_image = $"{SetMenuImageURL}{item.menu_image}";
                ob.menu_days = item.menu_days;
                ob.menu_item_description = item.menu_item_name ?? "N/A";
                ob.menu_code = item.menu_code;
                ob.menu_time_name = item.menu_time_name;
                ob.site_code = item.site_code;
                //ob.ingredient_name = item.ingredient_name;

              
               MenuItems.Add(ob);
              



               // MenuItems.Add(ob);
            }
        }

       

        public async void SetCutOffTime(mstr_meal_time mealTime)
        {
            checkCutoffTime(mealTime);

            await CheckOrder();
            if (Library.KEY_ORDER_ID != "0")
            {
                bool Check_order_result = await Check_Order_Taken(Library.KEY_CHECK_ORDER_DATE, PatientInfo.ID, SelectedMealTime.meal_name, Convert.ToInt32(Library.KEY_ORDER_ID));
                if (Check_order_result)
                {
                    if (SelectedMealTime.meal_name == "Breakfast")
                    {
                        Title = Library.KEY_USER_LANGUAGE == "English" ? $"{SelectedMealTime.meal_name} Meal order is already placed." : $"{SelectedMealTime.meal_name} สั่งอาหารถูกวางไว้แล้ว.";
                    }
                    else if (SelectedMealTime.meal_name == "Lunch")
                    {
                        Title = Library.KEY_USER_LANGUAGE == "English" ? $"{SelectedMealTime.meal_name} Meal order is already placed." : $"{SelectedMealTime.meal_name} สั่งอาหารถูกวางไว้แล้ว.";
                    }
                    else
                        Title = Library.KEY_USER_LANGUAGE == "English" ? $"{SelectedMealTime.meal_name} Meal order is already placed." : $"{SelectedMealTime.meal_name} สั่งอาหารถูกวางไว้แล้ว.";

                    //Title = $"{SelectedMealTime.meal_name} Meal order is already placed.";

                    var a = AppResources.ResourceManager.GetString(Title);
                    IsMenuEnable = false;
                    if (others.ID == 8 || others.ID == 1)
                    {
                        IsBtnVisible = false;
                    }
                    await PageDialog.DisplayAlertAsync("Alert!!", Title, "OK");


                }
            }

        }

        


        private async void checkCutoffTime(mstr_meal_time mealTime)
        {
            //if (SelectedMenuCategory != null)
            //{
            //    SelectedMenuCategory = null;
            //    MenuItems = new ObservableCollection<MenuItemClass>();
            //    Carts = new List<Cart>();
            //    IsItemAvailable = false;
            //    IsMenuImageVisible = true;

            //}

            string time = DateTime.Now.ToString("HH:mm", CultureInfo.InvariantCulture);

            DateTime lateformat = Convert.ToDateTime(mealTime.late_cut_off_start_time, CultureInfo.InvariantCulture);

            DateTime dtFromDate = DateTime.ParseExact(mealTime.cut_off_start_time, "HH:mm", CultureInfo.InvariantCulture);
            DateTime dtToDate = DateTime.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture);


            if (dtToDate > lateformat && DateTime.Now.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) == Library.KEY_ORDER_DATE)
            {

                if (mealTime.meal_name == "Breakfast")
                {
                    Title = Library.KEY_USER_LANGUAGE == "English" ? $"{mealTime.meal_name} Late Cut off time exceeds" : $"{mealTime.meal_name} เวลาปิดรับเกินกำหนด";
                }
                else if (SelectedMealTime.meal_name == "Lunch")
                {
                    Title = Library.KEY_USER_LANGUAGE == "English" ? $"{mealTime.meal_name} Late Cut off time exceeds" : $"{mealTime.meal_name} เวลาปิดรับเกินกำหนด";
                }
                else
                    Title = Library.KEY_USER_LANGUAGE == "English" ? $"{mealTime.meal_name} Late Cut off time exceeds" : $"{mealTime.meal_name} เวลาปิดรับเกินกำหนด";

                //Title = $"{mealTime.meal_name} {" Late Cut off time exceeds"}";
                IsMenuEnable = false;
                IsBtnVisible = false;
                await PageDialog.DisplayAlertAsync("Alert!!", Title, "OK");
                return;
            }
            else
            {
                IsMenuEnable = true;
                if (others.ID == 9 || others.ID == 1 || others.ID == 10)
                {
                    IsBtnVisible = true;
                }
            }



            TimeSpan difference = dtFromDate - dtToDate;
            double days = difference.TotalDays;

            if (difference.TotalMinutes < 0 && DateTime.Now.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) == Library.KEY_ORDER_DATE)
            {

                if (mealTime.meal_name == "Breakfast")
                {
                    Title = Library.KEY_USER_LANGUAGE == "English" ? $"{mealTime.meal_name} Cut off time exceeds" : $"{mealTime.meal_name} เวลาในการตัดเกินกว่า";
                }
                else if (SelectedMealTime.meal_name == "Lunch")
                {
                    Title = Library.KEY_USER_LANGUAGE == "English" ? $"{mealTime.meal_name} Cut off time exceeds" : $"{mealTime.meal_name} เวลาในการตัดเกินกว่า";
                }
                else
                    Title = Library.KEY_USER_LANGUAGE == "English" ? $"{mealTime.meal_name} Cut off time exceeds" : $"{mealTime.meal_name} เวลาในการตัดเกินกว่า";

                //Title = $"{mealTime.meal_name} {" Cut off time exceeds"}";

                int bedclassID = Convert.ToInt32(PatientInfo.Bed_Class_ID);
                var bedmealMap = _mappingRepo.QueryTable().Where(x => x.bed_class_id == bedclassID && x.status_id == 1).FirstOrDefault();
                var mealClass = _mealclassRepo.QueryTable().Where(x => x.ID == bedmealMap.meal_class_id).FirstOrDefault();

                //isallacartebreakfast = mealClass.is_A_la_carte;

                //if (!isallacartebreakfast)
                //{
                //    MenuItems = new ObservableCollection<MenuItemClass>();
                //}

                Library.KEY_IS_LATE_ORDERED = "1";
            }
            else
            {
                if (mealTime.meal_name == "Breakfast")
                {
                    Title = Library.KEY_USER_LANGUAGE == "English" ? $"{mealTime.meal_name} Cut off time: {mealTime.cut_off_start_time}" : $"{mealTime.meal_name} ตัดเวลา: {mealTime.cut_off_start_time}";
                }
                else if (SelectedMealTime.meal_name == "Lunch")
                {
                    Title = Library.KEY_USER_LANGUAGE == "English" ? $"{mealTime.meal_name} Cut off time: {mealTime.cut_off_start_time}" : $"{mealTime.meal_name} ตัดเวลา: {mealTime.cut_off_start_time}";
                }
                else
                    Title = Library.KEY_USER_LANGUAGE == "English" ? $"{mealTime.meal_name} Cut off time: {mealTime.cut_off_start_time}" : $"{mealTime.meal_name} ตัดเวลา: {mealTime.cut_off_start_time}";

                // Title = $"{mealTime.meal_name} Cut off time: {mealTime.cut_off_start_time}";
                Library.KEY_IS_LATE_ORDERED = "0";
            }

            CheckNBM();
        }

        private void CheckNBM()
        {
            if (others.ID == 1 || others.ID == 9 ||  others.ID == 10)
            {
                IsMenuEnable = false;
            }
        }

        #region SearchFilter
        private void SearchMenu(string value)
        {
            if (timer != null)
            {
                timer.Dispose();
            }
            SetUpTimer(TimeSpan.FromSeconds(1), value);
        }

        private void SetUpTimer(TimeSpan alertTime, string value)
        {
            this.timer = new Timer(x =>
            {
                this.GetMenuOnSearch(value);
            }, null, alertTime, Timeout.InfiniteTimeSpan);
        }

        private void GetMenuOnSearch(string s_word)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                IsPageEnabled = true;
                if (!string.IsNullOrEmpty(s_word))
                {
                    if (TempList != null)
                    {
                        MenuItemClasses = new ObservableRangeCollection<MenuItemClass>(TempList.Where(x => x.menu_item_name.ToLower().Contains(s_word.ToLower())
                                                                                         || x.menu_item_description.ToLower().Contains(s_word.ToLower())));
                    }
                }
                else
                    MenuItemClasses = new ObservableRangeCollection<MenuItemClass>(TempList);

                if (MenuItemClasses.Any())
                    IsItemAvailable = false;
                else
                    IsItemAvailable = true;

                IsPageEnabled = false;
            });
        }
        #endregion


        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            Cuisines = new ObservableCollection<mstr_meal_type>();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("NewOrder"))
            {
                SetCutOffTime(SelectedMealTime);
                SelectedRemark = Remarks.First();
                OrderRemarks = string.Empty;
                ReloadMenuCategory();
            }

        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.ContainsKey("Allergies"))
            {
                IsPageEnabled = true;

                SelectedCuisines = parameters["Cuisines"] as List<mstr_meal_type>;
                Cuisines = new ObservableCollection<mstr_meal_type>();
                var _mealtypeRepo = PrismApplicationBase.Current.Container.Resolve<IGenericRepo<mstr_meal_type>>();
                var data = _mealtypeRepo.QueryTable();

                foreach (var item in data)
                {
                    foreach (var selectedCusisine in SelectedCuisines)
                    {
                        if (selectedCusisine.ID == item.ID)
                        {
                            item.IsChecked = selectedCusisine.IsChecked;
                        }
                        
                    }

                    Cuisines.Add(item);
                }

               

                SelectedAllergies = parameters["Allergies"] as List<mstr_allergies_master>;
                SelectedIngredients = parameters["Ingredients"] as List<mstr_ingredient>;
                SelectedTherapeutics = parameters["Therapeutics"] as List<mstr_therapeutic>;
               

                CuisineNames = string.Join(",", SelectedCuisines.Select(x => x.meal_type_name));

                others = parameters["Other"] as mstr_others_master == null ? new mstr_others_master { others_name = string.Empty } : parameters["Other"] as mstr_others_master;
                otherslist = parameters["Othercheckbox"] as List<mstr_others_master>;

                otherslist.Add(others);

                LoadData();

                IsPageEnabled = false;
            }
           
            
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
           


        }



    }

    public class MenuItemClass : BindableBase
    {
        public string menu_item_description { get; set; }
        public string menu_item_name { get; set; }

        public string menu_item_code { get; set; }
        public int ID { get; set; }


        public byte[] ImageData { get; set; }
        internal bool Confinement;

        private string _btncolor;
        public string btncolor
        {
            get { return this._btncolor; }
            set { SetProperty(ref _btncolor, value); }
        }


        private string _localLanguage;

        public string LocalLanguage
        {
            get { return this._localLanguage; }
            set { SetProperty(ref _localLanguage, value); }
        }

        private string _ameContent;

        public string ameContent
        {
            get { return this._ameContent; }
            set { SetProperty(ref _ameContent, value); }
        }
        public int age_id { get; set; }
        public int diet_id { get; internal set; }
        public string ingredient_name { get; internal set; }
        public string menu_name { get; internal set; }
        public string menu_image { get; internal set; }
        public string menu_days { get; internal set; }
        public string menu_code { get; internal set; }
        public string menu_time_name { get; internal set; }
        public string site_code { get; internal set; }
        public int meal_type_id { get; internal set; }
        public string mealTime_names { get; internal set; }

    }

    public class Cart
    {
        public string mealtitleid { get; set; }
        public string mealtimeid { get; set; }
        public string mealitemname { get; set; }
        public string mealitemid { get; set; }
        public string mealtimename { get; set; }

        public int addonid { get; set; }
    }

}
