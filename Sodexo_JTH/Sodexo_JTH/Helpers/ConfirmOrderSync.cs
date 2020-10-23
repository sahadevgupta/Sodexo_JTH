using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Services;
using Sodexo_JTH.Models;
using Sodexo_JTH.Repos;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sodexo_JTH.Helpers
{
    public class ConfirmOrderSync
    {
        static string rid;

        public async static Task SyncNow(IGenericRepo<mstr_meal_order_local> orderlocalRepo, IGenericRepo<mstr_meal_time> mealtimeRepo, IPageDialogService pageDialog)
        {

            Library library = new Library();
            //string URL = Library.KEY_http + Library.KEY_SERVER_IP + "/" + Library.KEY_SERVER_LOCATION + "/sodexo.svc";
            var localOrders = orderlocalRepo.QueryTable();
            foreach (var item in localOrders)
            {
                var mealtime = mealtimeRepo.QueryTable().FirstOrDefault(x => x.ID == item.meal_time_id && x.status_id == 1);

                string time = DateTime.Now.ToString("HH:mm");
                //DateTime lateformat= DateTime.ParseExact(late_cut_off, "HH:mm",
                //                                    CultureInfo.InvariantCulture);
                DateTime lateformat = Convert.ToDateTime(mealtime.late_cut_off_start_time, CultureInfo.InvariantCulture);
                DateTime dtFromDate = DateTime.ParseExact(mealtime.cut_off_start_time, "HH:mm", CultureInfo.InvariantCulture);
                DateTime dtToDate = DateTime.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture);

                var parameterDate = DateTime.ParseExact(item.order_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                var todaysDate = DateTime.Today;

                if ((dtToDate > lateformat && DateTime.Now.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) == item.order_date) || parameterDate < todaysDate)
                {
                    orderlocalRepo.Delete(item.autoid.ToString());
                }
                else
                {
                    bool Check_order_result = await getfirstorder(item.P_id);
                    if (Check_order_result || Library.KEY_USER_ROLE == "Nurse" || Library.KEY_USER_ROLE == "Nurse+FSA")
                    {
                        await CheckOrder(item.order_date, item.P_id);
                        if (!string.IsNullOrEmpty(rid))// delete it if it is blank
                        {
                            int id = Convert.ToInt32(rid);
                            bool response = await Check_Order_Taken(item.order_date, item.P_id, mealtime.meal_name.Trim(), id);
                            if (!response)
                            {
                                item.order_id = id;
                                // Serialize our concrete class into a JSON String
                                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(item));

                                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                                // display a message jason conversion
                                //var message1 = new MessageDialog("Data is converted in json.");
                                //await message1.ShowAsync();

                                using (var httpClient = new System.Net.Http.HttpClient())
                                {
                                    var httpResponse = new System.Net.Http.HttpResponseMessage();
                                    // Do the actual request and await the response


                                    // httpResponse = new Uri(URL + "/" + Library.METHODE_UPDATE_ORDER); //replace your Url
                                    httpResponse = await httpClient.PostAsync(Library.URL + "/" + Library.METHODE_UPDATE_ORDER, httpContent);



                                    // If the response contains content we want to read it!
                                    await CheckOrderReaponse(orderlocalRepo, item, httpResponse, pageDialog);

                                }
                            }
                            else
                            {
                                orderlocalRepo.Delete(item.autoid.ToString());

                            }

                        }
                        else
                        {
                            int id = String.IsNullOrEmpty(rid) ? 0 : Convert.ToInt32(rid);

                            bool response = await Check_Order_Taken(item.order_date, item.P_id, mealtime.meal_name.Trim(), id);
                            if (!response)
                            {
                                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(item));

                                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                                using (var httpClient = new System.Net.Http.HttpClient())
                                {
                                    var httpResponse = new System.Net.Http.HttpResponseMessage();
                                    // Do the actual request and await the response


                                    // httpResponse = new Uri(URL + "/" + Library.METHODE_UPDATE_ORDER); //replace your Url
                                    httpResponse = await httpClient.PostAsync(Library.URL + "/" + Library.METHODE_SAVEORDER, httpContent);


                                    var orderResponse = await CheckOrderReaponse(orderlocalRepo, item, httpResponse, pageDialog);

                                }
                            }
                            else
                            {
                                orderlocalRepo.Delete(item.autoid.ToString());

                            }

                        }
                    }

                }

            }


        }

        private static async Task<bool> CheckOrderReaponse(IGenericRepo<mstr_meal_order_local> orderlocalRepo, mstr_meal_order_local item, HttpResponseMessage httpResponse, IPageDialogService pageDialog)
        {
            if (httpResponse.Content != null)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                if (responseContent == "true")
                {
                    MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "NewOrder", "order");
                    orderlocalRepo.Delete(item.autoid.ToString());

                    return true;
                }
                else
                {
                    orderlocalRepo.Delete(item.autoid.ToString());
                    await pageDialog.DisplayAlertAsync("Error!!", "Your order is not confirmed or already placed, there is some problem to process your request. Please check your internet connection.", "OK");
                    return false;
                }

            }
            else
                return false;
        }

        public static async Task<bool> getfirstorder(int p_id)
        {

            try
            {
                Library library = new Library();
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_CHECKFIRSTORDER + "/" + p_id);
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
            catch (Exception excp)
            {
                return false;
                //// stop progressring
                //myring.IsActive = false;
                //myring.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            }

        }

        public static async Task<bool> Check_Order_Taken(string order_date, int p_id, string meal_time, int order_id)
        {

            try
            {
                Library library = new Library();
                HttpClient httpClient = new System.Net.Http.HttpClient();
                if (meal_time == "Breakfast")
                {
                    meal_time = "BF";
                }
                if (meal_time == "Lunch")
                {
                    meal_time = "LH";
                }
                if (meal_time == "Dinner")
                {
                    meal_time = "DN";
                }

                DateTime check_date = Convert.ToDateTime(DateTime.ParseExact(order_date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                string format = check_date.Date.ToString("dd-MM-yyyy");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_CHECKORDERTAKEN + "/" + format + "/" + p_id + "/" + meal_time + "/" + order_id);
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
            catch (Exception excp)
            {
                return false;
            }
        }
        public static async Task CheckOrder(string date, int id)
        {
            int order_id = 0;
            try
            {
                int p_id = id;
                DateTime check_date = Convert.ToDateTime(DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                string format = check_date.Date.ToString("dd-MM-yyyy");
                Library library = new Library();
                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                System.Net.Http.HttpRequestMessage request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, Library.URL + "/" + Library.METHODE_CHECKORDER + "/" + format + "/" + p_id);
                System.Net.Http.HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();
                var data = await response.Content.ReadAsStringAsync();
                JArray jarray = JArray.Parse(data);
                for (int i = 0; i < jarray.Count; i++)
                {

                    JObject row = JObject.Parse(jarray[i].ToString());
                    order_id = Convert.ToInt32(row["order_id"].ToString());
                    rid = order_id.ToString();
                    //dbhelper.Insert_INTO_mstr_diet_type(new mstr_diet_type(Convert.ToInt32(row["Id"].ToString()), row["diet_name"].ToString(), row["diet_description"].ToString(), Convert.ToInt32(row["status_id"].ToString()), row["site_code"].ToString()));
                }
                if (jarray.Count == 0)
                {
                    rid = "";
                }

            }
            catch (Exception excp)
            {
                rid = "";
                // return 0;
                //// stop progressring
                //myring.IsActive = false;
                //myring.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            }

        }
    }
}
