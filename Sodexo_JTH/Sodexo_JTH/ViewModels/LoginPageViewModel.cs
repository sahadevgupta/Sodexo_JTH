using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Rg.Plugins.Popup.Extensions;
using Sodexo_JTH.Helpers;
using Sodexo_JTH.Models;
using Sodexo_JTH.PopUpControl;
using Sodexo_JTH.Resources;
using Sodexo_JTH.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Sodexo_JTH.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        private string _errorText;
        public string ErrorText
        {
            get { return _errorText; }
            set { SetProperty(ref _errorText, value); }
        }

        private List<string> _roleList;
        public List<string> RoleList
        {
            get { return _roleList; }
            set { SetProperty(ref _roleList, value); }
        }


        private string _selectedRole;

        public string SelectedRole
        {
            get { return this._selectedRole; }
            set { SetProperty(ref _selectedRole, value); }
        }


        private bool _isRolePickerVisible;

        public bool IsRolePickerVisible
        {
            get { return this._isRolePickerVisible; }
            set { SetProperty(ref _isRolePickerVisible, value); }
        }
        private bool _enableSubmitButton;
        public bool EnableSubmitButton
        {
            get { return _enableSubmitButton; }
            set { SetProperty(ref _enableSubmitButton, value); }
        }


        private ObservableCollection<Language> _languages;

        public ObservableCollection<Language> Languages
        {
            get { return this._languages; }
            set { SetProperty(ref _languages, value); }
        }


        public DelegateCommand GoToADFSCommand { get; set; }

        public INavigation navigation;


        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialog) : base(navigationService, pageDialog)
        {
            App.pageDialog = pageDialog;

            RoleList = new List<string> { "Select Role" };

            Languages = new ObservableCollection<Language>
            {
                new Language { Image =  "unitedkingdom.png", Title ="English"},
                new Language{Image ="thailand.png",Title="Thai"}
            };
            GoToADFSCommand = new DelegateCommand(NavigateToADFSPage);
        }

        private async void NavigateToADFSPage()
        {
           await NavigationService.NavigateAsync(nameof(ADFSPage), null, true, true);
        }

        internal async Task<List<string>> BindRole()
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        RoleList = new List<string>();
                        IsPageEnabled = true;

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_GETUSERROLEBYUSERNAMEMOBILE + "/" + UserName);

                        HttpClient httpClient = new System.Net.Http.HttpClient();
                        HttpResponseMessage response = await httpClient.SendAsync(request);

                        var data = await response.Content.ReadAsStringAsync();
                        JArray jarray = JArray.Parse(data);

                        if (jarray.Count > 0)
                        {
                            List<string> templist = new List<string>();
                            for (int i = 0; i < jarray.Count; i++)
                            {
                                JObject row = JObject.Parse(jarray[i].ToString());
                                templist.Add(row["role"].ToString());
                            }
                            if (templist.Count > 0)
                                IsRolePickerVisible = true;
                            else
                                IsRolePickerVisible = false;


                            IsPageEnabled = false;
                            RoleList = new List<string>(templist);
                            SelectedRole = RoleList.First();
                            return RoleList;
                                             
                        }
                        else
                        {
                            IsRolePickerVisible = false;
                            IsPageEnabled = false;
                            return RoleList;
                        }

                    }
                    catch (Exception)
                    {
                        IsPageEnabled = false;
                        await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("msg10", AppResources.Culture), "OK");
                        return null;

                    }
                }
                else
                {
                    await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("msg10", AppResources.Culture), "OK");
                    return null;
                }

            }
            else
            {
                IsPageEnabled = false;
                return null;
            }
        }

        public async Task Login(bool isLDap = false)
        {
            bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();
            // checking if user name and password are not blank      
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                try
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        await checkLogin(isLDap);
                    else
                        await PageDialog.DisplayAlertAsync("Alert!!", AppResources.ResourceManager.GetString("msg10", AppResources.Culture), "OK");
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;

                }
            }
            else
            {
               ErrorText = "Please enter user name and password.";
            }

        }

        public async Task checkLogin(bool isLDap)
        {
            try
            {

                string userType = string.Empty;

                if (SelectedRole.Equals("Select Role"))
                {
                    userType = "Select Role";

                }
                else if (SelectedRole.Equals("Nurse"))
                {
                    userType = "N";
                }
                else if (SelectedRole.Equals("FSA"))
                {
                    userType = "F";
                }
                else
                {
                    userType = "NF";
                }


                IsPageEnabled = true;
                
                String method = "UserLogin";
               
                var obj = new login();
                obj.Name = UserName;
                obj.pass = Password;
                obj.roleType = userType;
                if (isLDap)
                {
                    obj.isLDap = 1;
                }

                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(obj));

                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

               

                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    var response = await httpClient.PostAsync(Library.URL + "/" + Library.METHODE_USERLOGIN, httpContent);

                    if (response.ReasonPhrase == "Bad Request")
                    {
                        IsPageEnabled = false;
                        ErrorText = "Invalid user name or password.";
                        EnableSubmitButton = false;
                        return;
                    }
                    else if (response.ReasonPhrase == "Not Found")
                    {
                        IsPageEnabled = false;
                        ErrorText = "Invalid user name or password.";
                        EnableSubmitButton = false;
                        return;
                    }

                    if (response.Content != null)
                    {
                        var data = await response.Content.ReadAsStringAsync();

                        JArray jarray = JArray.Parse(data);
                        string status = jarray[0].Value<string>("status");
                        string expireday = jarray[0].Value<string>("expireday");
                        bool isLogin = Convert.ToBoolean(jarray[0].Value<string>("is_login"));

                        if (status == "Fail")
                        {
                            IsPageEnabled = false;
                            EnableSubmitButton = true;
                            ErrorText = "Invalid user name or password.";
                            return;
                        }

                        for (int i = 0; i < jarray.Count; i++)
                        {

                            JObject row = JObject.Parse(jarray[i].ToString());
                            string id = row["ID"].ToString();
                            if (id != "0")
                            {
                                // Storing user's detail in application data locally
                                Library.KEY_USER_ID = id;
                                Library.KEY_USER_FIRST_NAME = row["FirstName"].ToString();
                                Library.KEY_USER_LAST_NAME = row["LastName"].ToString();
                                Library.KEY_ROLL_TYPE = row["RoleType"].ToString();
                                Library.KEY_USER_ROLE = row["UserRole"].ToString();
                                Library.KEY_USER_STATUS = row["status"].ToString();
                                Library.KEY_USER_SiteCode = row["SiteCode"].ToString();
                                Library.KEY_USER_ccode = row["country_id"].ToString();

                                Library.KEY_USER_regcode = row["region_id"].ToString();

                                Library.KEY_USER_siteid = row["site_id"].ToString();
                                Library.KEY_USER_roleid = row["role_Id"].ToString();

                                Library.KEY_USER_feedback_link = row["feedback_link"].ToString();
                                Library.KEY_USER_language_id = row["language_id"].ToString();
                                Library.KEY_USER_payment_mode_ids = row["payment_mode_ids"].ToString();
                                Library.KEY_USER_currency = row["country_currency"].ToString();
                                Library.KEY_USER_adjusttime = row["AdjustsiteTime"].ToString();

                                if (Library.KEY_USER_ROLE == "FSA" || Library.KEY_USER_ROLE == "Nurse" || Library.KEY_USER_ROLE == "Nurse+FSA")
                                {
                                    CreateDB();

                                    if (expireday == "")
                                    {
                                        await NavigateToHome(Library.URL, httpClient, id);
                                    }
                                    else if (expireday == "100")
                                    {
                                        await PageDialog.DisplayAlertAsync("Alert!!", "Password will expire in one day", "ok");

                                        await NavigateToHome(Library.URL, httpClient, id);
                                       
                                    }
                                    else if (expireday == "200")
                                    {

                                        await PageDialog.DisplayAlertAsync("Alert!!", "Password will expire in two days", "OK");
                                        await NavigateToHome(Library.URL, httpClient, id);
                                    }
                                    else if (expireday == "300")
                                    {
                                        await PageDialog.DisplayAlertAsync("Alert!!", "Password will expire in three days", "OK");
                                        await NavigateToHome(Library.URL, httpClient, id);

                                    }
                                    else if (expireday == "400")
                                    {
                                        var termsPopup = new TermsConditionPopup();
                                        termsPopup.Disappearing += async(s, e) => 
                                        {
                                            if (termsPopup.isAccepted)
                                            {
                                                IsPageEnabled = true;


                                                dynamic p = new JObject();
                                                p.UserId = Library.KEY_USER_ID;

                                                var Payload = JsonConvert.SerializeObject(p);
                                                var httpMSgClient = new System.Net.Http.HttpClient();
                                                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                                                var Content = new StringContent(Payload, Encoding.UTF8, "application/json");

                                                var msgResponse = await httpMSgClient.PostAsync(Library.URL + "/" + "TermsConditions", Content);
                                                await NavigateToHome(Library.URL, httpMSgClient, id);

                                                IsPageEnabled = false;
                                            }
                                        };
                                        await navigation.PushPopupAsync(termsPopup,true);
                                    }
                                    else if (expireday == "500")
                                    {
                                        //this.Frame.Navigate(typeof(PatientSearch), null);
                                        await PageDialog.DisplayAlertAsync("Alert!!", "Password is Expired.", "OK");

                                    }
                                    else if (expireday == "600")
                                    {
                                        //this.Frame.Navigate(typeof(PatientSearch), null);
                                        await PageDialog.DisplayAlertAsync("Alert!!", "This User Is Deactivated, Kindly Contact Admin.", "OK");

                                    }
                                    else
                                    {
                                        await PageDialog.DisplayAlertAsync("Alert!!", "Unknown Error.", "OK");
                                    }


                                }
                                else
                                {
                                    ErrorText = "Only FSA and Nurse can login in tablet.";

                                }


                            }
                            else
                            {
                                ErrorText = "Invalid user name or password.";
                                
                            }

                        }

                    }
                }

                IsPageEnabled = false;
            }
            catch (Exception excp)
            {
                IsPageEnabled = false;

                
                await PageDialog.DisplayAlertAsync("Alert!!", excp.Message, "OK");
            }
        }

        private async Task NavigateToHome(string URL, HttpClient httpClient, string id)
        {
         
            Device.BeginInvokeOnMainThread(async() =>
            {
                await NavigationService.NavigateAsync("app:///HomeMasterDetailPage");
            });
            
            await SessionManager.Instance.StartTrackSessionAsync();
        }

        private void CreateDB()
        {
            Helpers.DevelopmentCode.CreateTables();
        }
    }
}
