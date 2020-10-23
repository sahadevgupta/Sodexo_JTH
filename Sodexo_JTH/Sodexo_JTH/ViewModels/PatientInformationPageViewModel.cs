using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Rg.Plugins.Popup.Extensions;
using Sodexo_JTH.Helpers;
using Sodexo_JTH.Interfaces;
using Sodexo_JTH.Models;
using Sodexo_JTH.PopUpControl;
using Sodexo_JTH.Repos;
using Sodexo_JTH.Resources;
using Sodexo_JTH.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using DependencyService = Xamarin.Forms.DependencyService;

namespace Sodexo_JTH.ViewModels
{
    public class PatientInformationPageViewModel : ViewModelBase
    {
        private mstr_patient_info _selectedPatient;
        public mstr_patient_info SelectedPatient
        {
            get { return _selectedPatient; }
            set { SetProperty(ref _selectedPatient, value); }
        }
        public List<string> HnHList { get; set; } //Halal NonHalal list
        public List<string> VegNVegList { get; set; }
        public List<string> FluidList { get; set; }


        private ObservableCollection<mstr_others_master> _othersradio;

       

        public ObservableCollection<mstr_others_master> OthersRadio
        {
            get { return this._othersradio; }
            set { SetProperty(ref _othersradio, value); }
        }

        private ObservableCollection<mstr_others_master> _othersChecBox;

        public ObservableCollection<mstr_others_master> OthersChecBox
        {
            get { return this._othersChecBox; }
            set { SetProperty(ref _othersChecBox, value); }
        }

        private ObservableCollection<mstr_allergies_master> _allergies;

        public ObservableCollection<mstr_allergies_master> Allergies
        {
            get { return this._allergies; }
            set { SetProperty(ref _allergies, value); }
        }
        private ObservableCollection<mstr_ingredient> _ingredients;

        public ObservableCollection<mstr_ingredient> Ingredients
        {
            get { return this._ingredients; }
            set { SetProperty(ref _ingredients, value); }
        }


        private ObservableCollection<mstr_therapeutic> _therapeutics;

        public ObservableCollection<mstr_therapeutic> Therapeutics
        {
            get { return this._therapeutics; }
            set { SetProperty(ref _therapeutics, value); }
        }


        private ObservableCollection<mstr_meal_type> _cuisines;

        public ObservableCollection<mstr_meal_type> Cuisines
        {
            get { return this._cuisines; }
            set { SetProperty(ref _cuisines, value); }
        }


        private bool _isInfantEnabled;

        public bool IsInfantEnabled
        {
            get { return this._isInfantEnabled; }
            set
            {
                SetProperty(ref _isInfantEnabled, value);
                Library.InfantDietEnable = value;
            }
        }

        private bool _isDisposable;

        public bool IsDisposable
        {
            get { return this._isDisposable; }
            set
            {
                SetProperty(ref _isDisposable, value);
                Library.IsDisposableEnable = value;
            }
        }
        private bool _isConfinement;

        public bool IsConfinement
        {
            get { return this._isConfinement; }
            set
            {
                SetProperty(ref _isConfinement, value);
                Library.IsConfinementEnable = value;
            }
        }


       
        private bool _isNoMealEnable = true;

        public bool IsNoMealEnable
        {
            get { return this._isNoMealEnable; }
            set { SetProperty(ref _isNoMealEnable, value); }
        }

        public List<string> RadioButtonList { get; set; }

        public DelegateCommand<string> NextCommand { get; set; }
        public INavigation _Navigation { get; internal set; }
        IDialogService _dialogService;

        IGenericRepo<mstr_others_master> _OthersRepo;
        IGenericRepo<mstr_allergies_master> _AllergyRepo;
        IGenericRepo<mstr_ingredient> _ingredientRepo;
        IGenericRepo<mstr_therapeutic> _therapeuticsRepo;
        IGenericRepo<mstr_diet_texture> _dietTextureRepo;
        IGenericRepo<mstr_meal_type> _mealTypeRepo;

        public PatientInformationPageViewModel(INavigationService navigationService, IGenericRepo<mstr_others_master> OthersRepo, IGenericRepo<mstr_allergies_master> AllergyRepo,
            IGenericRepo<mstr_ingredient> ingredientRepo, IGenericRepo<mstr_therapeutic> therapeuticsRepo, IGenericRepo<mstr_diet_texture> dietTextureRepo,
            IGenericRepo<mstr_meal_type> mealTypeRepo, IPageDialogService pageDialog,IDialogService dialogService) : base(navigationService, pageDialog)
        {
            _OthersRepo = OthersRepo;
            _AllergyRepo = AllergyRepo;
            _ingredientRepo = ingredientRepo;
            _therapeuticsRepo = therapeuticsRepo;
            _dietTextureRepo = dietTextureRepo;
            _mealTypeRepo = mealTypeRepo;
            _dialogService = dialogService;

            FluidList = new List<string> { "NA", "Thin" };
            HnHList = new List<string> { "Halal", "Non Halal" };
            VegNVegList = new List<string> { "Veg", "Non-Veg" };

            RadioButtonList = new List<string> { "Yes", "No" };


            NextCommand = new DelegateCommand<string>(NavigateToOrderPage);

        }

        private async void NavigateToOrderPage(string obj)
        {
            IsPageEnabled = true;
            if (obj.Equals(AppResources.ResourceManager.GetString("btnupdateContent", AppResources.Culture)))
            {
                await UpdatePatientInfo();
            }
            else
            {
                IsPageEnabled = true;
                if (!Library.KEY_PATIENT_IS_VEG.ToLower().Equals(SelectedPatient.isveg.ToLower()) || !Library.KEY_PATIENT_IS_HALAL.ToLower().Equals(SelectedPatient.ishalal.ToLower()))
                {
                    var response = await PageDialog.DisplayAlertAsync("Meal Prefrence Changed!", "Do you want to Change the (Veg/Nonveg)/(Halal/Non-Halal) Status.", "Yes", "No");
                    if (response)
                    {
                        Library.KEY_PATIENT_IS_VEG = SelectedPatient.isveg;
                        Library.KEY_PATIENT_IS_HALAL = SelectedPatient.ishalal;
                    }
                    else
                    {
                        IsPageEnabled = false;
                        return;
                    }

                }



                var navParam = new NavigationParameters();

                //navParam.Add("Patient", SelectedPatient);
                Library.patient = SelectedPatient;
                Library.others = OthersRadio.Where(x => x.IsChecked).FirstOrDefault();
                navParam.Add("Cuisineslist", Cuisines);
                navParam.Add("Allergies", Allergies.Where(x => x.IsChecked).ToList());
                navParam.Add("Ingredients", Ingredients.Where(x => x.IsChecked).ToList());
                navParam.Add("Therapeutics", Therapeutics.Where(x => x.IsChecked).ToList());
                navParam.Add("Cuisines", Cuisines.Where(x => x.IsChecked).ToList());
                navParam.Add("Other", OthersRadio.Where(x => x.IsChecked).FirstOrDefault());
                navParam.Add("Othercheckbox", OthersChecBox.Where(x => x.IsChecked).ToList());

                await NavigationService.NavigateAsync("MealOrderPage", navParam);

                IsPageEnabled = false;
            }
        }
      
        private async Task UpdatePatientInfo()
        {
            IsPageEnabled = true;

            string isAllergy = string.Empty;
            var selectedAllergies = Allergies.Where(x => x.IsChecked);

            foreach (var item in selectedAllergies)
            {
                isAllergy += item.ID + ",";
            }

            isAllergy = isAllergy.TrimEnd(',');

            dynamic p = new JObject();
            p.halal = SelectedPatient.ishalal == "True" ? 1 : 0;
            p.isallergies = isAllergy;
            p.isdiabetic = 1;
            p.isveg = SelectedPatient.isveg == "True" ? 1 : 0;
            p.patientid = SelectedPatient.ID.ToString();

            string stringPayload = JsonConvert.SerializeObject(p);

            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");


            using (var httpClient = new System.Net.Http.HttpClient())
            {
               
                // Do the actual request and await the response

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{Library.URL}/setpatientprofile/{p.patientid}/{p.isveg}/{p.halal}/{p.isallergies}/{p.isdiabetic}");

                HttpResponseMessage response = await httpClient.SendAsync(request);
               

                // If the response contains content we want to read it!
                if (response.Content != null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (responseContent == "true")
                    {
                        await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("pio", AppResources.Culture), "OK");
                        await NavigationService.GoBackAsync();
                    }
                }
            }

            IsPageEnabled = false;
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            IsPageEnabled = false;
        }
        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

           
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.ContainsKey("PatientInfo"))
            {

                OthersRadio = new ObservableCollection<mstr_others_master>();
                OthersChecBox = new ObservableCollection<mstr_others_master>();
                Allergies = new ObservableCollection<mstr_allergies_master>();
                Ingredients = new ObservableCollection<mstr_ingredient>();
                Therapeutics = new ObservableCollection<mstr_therapeutic>();
                Cuisines = new ObservableCollection<mstr_meal_type>();

                SelectedPatient = parameters["PatientInfo"] as mstr_patient_info;

                SelectedPatient.FluidInfo = string.IsNullOrEmpty(SelectedPatient.FluidInfo) ? "NA" : SelectedPatient.FluidInfo;


                var tempOthers = new List<mstr_others_master>();
                var Others = _OthersRepo.QueryTable().Where(x => x.status_id == 0).OrderBy(x => x.ID);
                foreach (var other in Others)
                {
                    other.PropertyChanged += Other_PropertyChanged;
                    tempOthers.Add(other);
                }


               var  TempOthersRadio = new ObservableCollection<mstr_others_master>(tempOthers);

                var naOther = new mstr_others_master
                {
                    ID = 0,
                    others_name = "NA",
                };
                naOther.PropertyChanged += Other_PropertyChanged;

                TempOthersRadio.Add(naOther);
                OthersRadio = new ObservableCollection<mstr_others_master>(TempOthersRadio);

                foreach (var item in Others.Where(x => x.status_id == 1))
                {
                    OthersChecBox.Add(item);
                }

                //var tempAllergies = new List<mstr_allergies_master>();
                var allergies = _AllergyRepo.QueryTable().Where(x => x.status_id == 1).OrderBy(y => y.allergies_name);
                //foreach (var item in allergies)
                //{
                //    if (!string.IsNullOrEmpty(SelectedPatient.Allergies))
                //    {
                //        var foodAllergies = SelectedPatient.Allergies.Split(',');
                //        foreach (var id in foodAllergies)
                //        {
                //            var allergyID = Convert.ToInt32(id);
                //            if (item.ID == allergyID)
                //            {
                //                item.IsChecked = true;
                //            }
                //        }
                //    }
                //    tempAllergies.Add(item);
                //}


                Allergies = new ObservableCollection<mstr_allergies_master>(allergies);




                var tempIngredients = new List<mstr_ingredient>();
                var ingredients = _ingredientRepo.QueryTable().Where(x => x.status_id == 1).OrderBy(y => y.ingredient_name);
                foreach (var item in ingredients)
                {
                    if (!string.IsNullOrEmpty(SelectedPatient.Ingredient))
                    {
                        var _Ingredient = SelectedPatient.Ingredient.Split(',');
                        foreach (var name in _Ingredient)
                        {
                            if (item.ingredient_name == name)
                            {
                                item.IsChecked = true;
                            }
                        }
                    }
                    tempIngredients.Add(item);
                }


                Ingredients = new ObservableCollection<mstr_ingredient>(tempIngredients);


                var tempTherapeutics = new List<mstr_therapeutic>();
                var therapeutics = _therapeuticsRepo.QueryTable().Where(x => x.status_id == 1).OrderBy(y => y.TH_code);
                foreach (var therapeutic in therapeutics)
                {
                    if (!string.IsNullOrEmpty(SelectedPatient.Therapeutic))
                    {
                        var _Therapeutics = SelectedPatient.Therapeutic.Split(',');
                        foreach (var th_code in _Therapeutics)
                        {
                            if (therapeutic.TH_code == th_code)
                            {
                                therapeutic.IsChecked = true;
                            }
                        }
                    }
                    //therapeutic.PropertyChanged += Therapeutic_PropertyChanged;
                    tempTherapeutics.Add(therapeutic);
                }

                Therapeutics = new ObservableCollection<mstr_therapeutic>(tempTherapeutics);

                InitializeMealType(SelectedPatient);
            }
        }

        private void InitializeMealType(mstr_patient_info patient)
        {
            var tempMealTypes = new List<mstr_meal_type>();
            var mealTypes = _mealTypeRepo.QueryTable().Where(x => x.status_id == 1).OrderBy(y => y.meal_type_name);
            foreach (var mealType in mealTypes)
            {
                if (!string.IsNullOrEmpty(SelectedPatient.Meal_Type))
                {
                    var _mealTypesname = SelectedPatient.Meal_Type.Split(',');
                    foreach (var _mealTypename in _mealTypesname)
                    {
                        if (mealType.meal_type_name == _mealTypename)
                        {
                            mealType.IsChecked = true;
                        }
                    }
                }

                tempMealTypes.Add(mealType);
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                Cuisines = new ObservableCollection<mstr_meal_type>(tempMealTypes);
            });
        }

        private void Other_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsChecked")
            {
                var checkedData = sender as mstr_others_master;
                if (checkedData.IsChecked)
                {
                    if (checkedData.others_name.Contains("NBM"))
                        IsNoMealEnable = false;
                    else
                        IsNoMealEnable = true;

                    var checkedQuery = OthersRadio.Where(x => x.IsChecked == true && x.others_name != checkedData.others_name);
                    if (checkedQuery.Any())
                    {
                        checkedQuery.First().IsChecked = false;
                    }
                }
            }
        }

        
    }
}
