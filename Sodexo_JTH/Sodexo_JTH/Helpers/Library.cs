using Sodexo_JTH.Models;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace Sodexo_JTH.Helpers
{
    public class Library
    {
       // public static string ServerName = "UAT";
        //public static string ServerName = "QA";
        public static string ServerName = "P";


        // public static string URL = "http://172.25.1.29/Sodexo_test_services/sodexo.svc/";
        //public static string URL = "http://epinetuat/Sodexo_test_services/sodexo.svc/";  // for Local Server
        //public static string URL = "https://www.touch2orderqaws.sodexonet.com/T2O_JTH_Service/Sodexo.svc/";  //  for QA Server
       public static string URL = "https://www.touch2orderws.sodexonet.com/T2O_JTH_Service/Sodexo.svc/";  //  for Production Server


        #region ADFS

        public static string Client_ID = "1d39e1b9-299c-4665-a6c2-18be4f24c4bb";
            public static string Resource_identifier = "touch2go_application_api";
            public static string Login_redirect_URI = "urn:ietf:wg:oauth:2.0:oob";

        #endregion



        //WCF service methods
        public static string METHOD_PULLPATIENTSBYWARD = "PullpatientData_by_wardno";
        public static string METHODE_SHOWPATIENTMEALDETAILSBYID = "ShowpatientmealdetailsById";
        public static string METHODE_GETDELIVEREDDATA = "GetDelivereddata";
        public static string METHODE_SETDELIVEREDSTATUS = "SetDeliveredStatus";
        public static string METHODE_GETREMARKSDETAIL = "Remarksdetails";
        public static string METHODE_CHECKORDERTAKEN = "checkordertaken";
        public static string METHODE_UPDATE_ORDER = "updateorder";
        public static string METHODE_CHECKORDER = "checkorder";
        public static string METHODE_CHECKFIRSTORDER = "checkfirstorder";
        public static string METHODE_SAVEORDER = "SaveOrder";
        public static string METHODE_GETALLPATIENT = "PullpatientData";
        public static string METHODE_SEARCHBYWARDNO = "Searchbywardno";
        public static string METHODE_SEARCHBYPATIENTNAME = "Searchbypatientname";
        public static string METHODE_BEDMEALCLASSMAPPING = "bedmealclassmapping";
        public static string METHODE_DISPLAYBEDCLASS = "Displaybedclass";
        public static string METHODE_DISPLAYMEALCLASS = "Displaymealclass";
        public static string METHODE_ALLERGENTDIETLIST = "AllergentDietList";
        public static string METHODE_DISPLAYBEDDETAILS = "DisplayBedDetails";
        public static string METHODE_DISPLAYDIET_TYPE = "Displaydiet_type";
        public static string METHODE_GETALLDIETTEXTURE = "GetAllDietTexture";
        //public static string METHODE_FLUIDlIST = "Fluidlist";
        public static string METHODE_INGREDIENTLIST = "IngredientList";
        public static string METHODE_DISPLAYMEAL_OPTION = "Displaymeal_option";
        public static string METHODE_MEALTIMELIST = "MealTimeDetails";
        public static string METHODE_DISPLAYMEALTYPE = "DisplayMealType";
        public static string METHODE_NENU_MASETER = "SetMenuDetailsWithImg";
        public static string METHODE_NENUITEMDETAILS = "MenuItemDetailsWithImg";
        public static string METHODE_MENUITEMCATEGORYDETAILS = "MenuItemCategorydetails";
        public static string METHODE_OTHERSMASTERDETAILS = "OthersMasterdetails";
        //  public static string METHODE_PATIENTDETAILS = "Patientdetails";
        public static string METHODE_THERAPEUTICDETAILS = "Therapeuticdetails";
        public static string METHODE_DISPLAYWARDDETAILS = "DisplayWardDetails";
        public static string METHODE_USERLOGIN = "UserLogin";
        public static string METHODE_CYCLEDETAILS = "Cycledetailsmaster";
        public static string METHODE_Cancel_order = "Cancel_order";
        public static string METHODE_DisplayPaymentModeDetails = "DisplayPaymentModeDetails";
        public static string METHODE_DisplayLanguageDetails = "DisplayLanguageDetails ";
        // public static string METHODE_CaregiverMenuItems = "CaregiverMenuItems";
        public static string METHODE_QRVERIFIED = "QRVerified";
        public static string METHODE_CAREGIVERMENUITEMS = "CaregiverMenuItems";
        public static string METHODE_GETMEALORDERSTATUS = "GetMealOrderStatus";
        public static string METHODE_GETUSERROLEBYUSERNAMEMOBILE = "GetUserRoleByUserNameMobile";
        public static string METHODE_GETALLFLUIDCONSISTENCY = "GetAllFluidConsistency";
        public static string METHODE_THERAPEUTICCONDITIONDETAILS = "TherapeuticConditiondetails";

        // End of WCF service...........................................................

        public static string SelectedLanguage = "English";
        public static string lang { get; internal set; }
        public static mstr_patient_info patient { get; internal set; }

        public static mstr_others_master others { get; internal set; }

        public static string USER_ROLE_KEY { get; internal set; }
        public static string KEY_USER_FIRST_NAME { get; internal set; }
        public static string KEY_USER_LAST_NAME { get; internal set; }

        public static string KEY_ROLL_TYPE { get; internal set; }
        public static string KEY_USER_STATUS { get; internal set; }

        public static string KEY_USER_SiteCode { get; internal set; }

        public static string KEY_USER_ccode { get; internal set; }

        public static string KEY_USER_regcode { get; internal set; }

        public static string KEY_USER_siteid { get; internal set; }

        public static string KEY_USER_ROLE { get; set; }

        public static string KEY_USER_roleid { get; internal set; }
        public static string KEY_USER_currency { get; set; }
        public static string KEY_USER_feedback_link { get; internal set; }

        public static string KEY_USER_language_id { get; internal set; }
        public static string KEY_USER_payment_mode_ids { get; internal set; }

        public static string KEY_PATIENT_IS_HALAL { get; internal set; }
        public static string KEY_PATIENT_IS_VEG { get; internal set; }

        public static string KEY_ORDER_DATE { get; internal set; }
        public static int KEY_PATIENT_WARD_TYPE_ID { get; internal set; }
        public static string KEY_USER_adjusttime { get; internal set; }

        public static string KEY_langchangedfrommealpage { get; internal set; }
        public static string KEY_IS_LATE_ORDERED { get; internal set; }

        public static string KEY_IS_CARE_GIVER = "no";

        public static string KEY_ORDER_ID { get; internal set; }
        public static string KEY_CHECK_ORDER_DATE { get; internal set; }

        public static bool? IsFAGeneralEnable { get; internal set; }
        public static bool InfantDietEnable { get; internal set; }
        public static bool IsDisposableEnable { get; internal set; }
        public static bool IsConfinementEnable { get; internal set; }



        private static int _orderCount;

        public static int OrderCount
        {
            get { return _orderCount; }
            set { _orderCount = value; }
        }


        private const string USER_LANGUAGE_KEY = "KEY_USER_LANGUAGE";

        public static string KEY_USER_LANGUAGE
        {
            get
            {
                return Preferences.Get(USER_LANGUAGE_KEY, "");
            }
            set
            {
                Preferences.Set(USER_LANGUAGE_KEY, value);
            }
        }



        private const string USER_ID_KEY = "KEY_USER_ID";

        public static string KEY_USER_ID
        {
            get
            {
                return Preferences.Get(USER_ID_KEY, "");
            }
            set
            {
                Preferences.Set(USER_ID_KEY, value);
            }
        }


        // KEY_SYNC_NOTIFICATION
        private const string SYNC_NOTIFICATION_KEY = "SYNC_NOTIFICATION_KEY";

        public static string KEY_SYNC_NOTIFICATION
        {
            get
            {
                return Preferences.Get(SYNC_NOTIFICATION_KEY, "");
            }
            set
            {
                Preferences.Set(SYNC_NOTIFICATION_KEY, value);
            }
        }
        // KEY_SYNC_NOTIFICATION end


        // last_mastersynctime
        private const string last_mastersynctime_key = "last_mastersynctime_key";

        public static string last_mastersynctime
        {
            get
            {
                return Preferences.Get(last_mastersynctime_key, "");
            }
            set
            {
                Preferences.Set(last_mastersynctime_key, value);
            }
        }
        // last_mastersynctime end

        // last_mealssynctime
        private const string last_mealssynctime_key = "last_mealssynctime_key";

        public static string last_mealssynctime
        {
            get
            {
                return Preferences.Get(last_mealssynctime_key, "");
            }
            set
            {
                Preferences.Set(last_mealssynctime_key, value);
            }
        }
        // last_mealssynctime end


       // private static  IPreferences AppSettings => Xamarin.Essentials.InterfacPreferencesImplementation;


    }
}
