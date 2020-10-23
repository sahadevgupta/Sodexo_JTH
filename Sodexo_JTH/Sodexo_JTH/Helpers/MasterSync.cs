using Newtonsoft.Json;
using Sodexo_JTH.Interfaces;
using Sodexo_JTH.Models;
using Sodexo_JTH.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sodexo_JTH.Helpers
{
    public static class MasterSync
    {
        static HomeMasterDetailPageViewModel _viewModel;

        public async static Task SyncMaster(HomeMasterDetailPageViewModel viewModel)
        {
            _viewModel = viewModel;
            await Sync_mstr_bed_details();
            await Sync_mstr_bed_meal_class_mapping();
            await Sync_mstr_Cycledetails();
            await Sync_mstr_ward_details();
            await Sync_mstr_allergies_master();
            await Sync_mstr_diet_type();
            await Sync_mstr_ingredient();
            await Sync_mstr_mealclass();
            await Sync_mstr_meal_option();
            await Sync_mstr_meal_time();
            await Sync_mstr_meal_type();
            await Sync_mstr_menu_item_category();
            await Sync_mstr_others_master();
            await Sync_mstr_remarks();
            await Sync_mstr_diet_texture();
            await Sync_mstr_displaypayment();
            await Sync_mstr_therapeutic();
            
        }
        private async static Task Sync_mstr_displaypayment()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_DisplayPaymentModeDetails);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();
                List<mstr_DisplayPaymentModeDetails> jarray = JsonConvert.DeserializeObject<List<mstr_DisplayPaymentModeDetails>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_DisplayPaymentModeDetails>();
                    dbConn.CreateTable<mstr_DisplayPaymentModeDetails>();
                    dbConn.InsertAll(jarray);
                    dbConn.Commit();
                };
                _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
            }
            catch (Exception excp)
            {

            }
        }
        public async static Task Sync_mstr_patient_info(HomeMasterDetailPageViewModel viewModel)
        {
            try
            {
                mstr_patient_info patient_info = new mstr_patient_info();
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_GETALLPATIENT + "/" + Library.KEY_USER_SiteCode);
                HttpResponseMessage response = await httpClient.SendAsync(request);

                List<mstr_patient_info> jarray = JsonConvert.DeserializeObject<List<mstr_patient_info>>(await response.Content.ReadAsStringAsync());
                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_patient_info>();
                    dbConn.CreateTable<mstr_patient_info>();
                    dbConn.BeginTransaction();
                    float itemNo = 0;
                    foreach (var item in jarray)
                    {
                        itemNo++;
                        item.ward_bed = item.ward_name + "-" + item.bed_name;
                        item.ID = item.ID;

                        if (string.Equals(item.is_care_giver, "true", StringComparison.CurrentCultureIgnoreCase))
                            item.Patientname = item.Patientname + ' ' + "(Care Giver)";

                        dbConn.Insert(item);
                        viewModel.CurrentProgress = ((itemNo / jarray.Count) * 100) / 100;

                    }
                    
                    dbConn.Commit();
                };
            }
            catch (Exception)
            {
            }
        }
        

        private async static Task Sync_mstr_meal_option()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_DISPLAYMEAL_OPTION + "/" + Library.KEY_USER_ccode);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                List<mstr_meal_option> jarray = JsonConvert.DeserializeObject<List<mstr_meal_option>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_meal_option>();
                    dbConn.CreateTable<mstr_meal_option>();
                    dbConn.BeginTransaction();
                    dbConn.InsertAll(jarray);
                    dbConn.Commit();
                };
                _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
            }
            catch (Exception excp)
            {
            }
        }

        private async static Task Sync_mstr_others_master()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_OTHERSMASTERDETAILS + "/" + Library.KEY_USER_ccode);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                List<mstr_others_master> jarray = JsonConvert.DeserializeObject<List<mstr_others_master>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_others_master>();
                    dbConn.CreateTable<mstr_others_master>();
                    dbConn.BeginTransaction();
                    dbConn.InsertAll(jarray);
                    dbConn.Commit();
                };
                _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
            }
            catch (Exception excp)
            {
            }
        }

        private async static Task Sync_mstr_therapeutic()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_THERAPEUTICDETAILS + "/" + Library.KEY_USER_ccode);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                List<mstr_therapeutic> jarray = JsonConvert.DeserializeObject<List<mstr_therapeutic>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_therapeutic>();
                    dbConn.CreateTable<mstr_therapeutic>();
                    dbConn.BeginTransaction();
                    dbConn.InsertAll(jarray);
                    dbConn.Commit();
                };
                _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
            }
            catch (Exception excp)
            {

            }
        }

        private async static Task Sync_mstr_menu_item_category()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_MENUITEMCATEGORYDETAILS + "/" + Library.KEY_USER_ccode);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();
                List<mstr_menu_item_category> jarray = JsonConvert.DeserializeObject<List<mstr_menu_item_category>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_menu_item_category>();
                    dbConn.CreateTable<mstr_menu_item_category>();
                    dbConn.BeginTransaction();
                    dbConn.InsertAll(jarray);
                    dbConn.Commit();
                };
                _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
            }
            catch (Exception excp)
            {
                // stop progressring


            }
        }

        //private async static Task Sync_mstr_displaypayment()
        //{
        //    try
        //    {
        //        HttpClient httpClient = new System.Net.Http.HttpClient();
        //        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_DisplayPaymentModeDetails);
        //        HttpResponseMessage response = await httpClient.SendAsync(request);
        //        // jarray= await response.Content.ReadAsStringAsync();
        //        List<mstr_DisplayPaymentModeDetails> jarray = JsonConvert.DeserializeObject<List<mstr_DisplayPaymentModeDetails>>(await response.Content.ReadAsStringAsync());

        //        using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
        //        {
        //            dbConn.DropTable<mstr_DisplayPaymentModeDetails>();
        //            dbConn.CreateTable<mstr_DisplayPaymentModeDetails>();
        //            dbConn.BeginTransaction();
        //            dbConn.InsertAll(jarray);
        //            dbConn.Commit();
        //        };
        //    }
        //    catch (Exception excp)
        //    {

        //    }
        //}

        private async static Task Sync_mstr_meal_time()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_MEALTIMELIST);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();
                List<mstr_meal_time> jarray = JsonConvert.DeserializeObject<List<mstr_meal_time>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_meal_time>();
                    dbConn.CreateTable<mstr_meal_time>();
                    dbConn.BeginTransaction();
                    dbConn.InsertAll(jarray);
                    dbConn.Commit();
                   
                };
                _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;


            }
            catch (Exception excp)
            {
                
            }
        }

        

        private async static Task Sync_mstr_ward_details()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_DISPLAYWARDDETAILS + "/" + Library.KEY_USER_ccode + "/" + Library.KEY_USER_regcode + "/" + Library.KEY_USER_siteid + "/0");
                HttpResponseMessage response = await httpClient.SendAsync(request);
                List<mstr_ward_details> jarray = JsonConvert.DeserializeObject<List<mstr_ward_details>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_ward_details>();
                    dbConn.CreateTable<mstr_ward_details>();
                    dbConn.BeginTransaction();
                    foreach (var item in jarray)
                    {
                        string cont_id = item.country_id.ToString();
                        string rgn_id = item.region_id.ToString();
                        string st_id = item.site_id.ToString();

                        if (Library.KEY_USER_ccode == cont_id && (Library.KEY_USER_regcode == "nil" || Convert.ToInt16(Library.KEY_USER_regcode) == 0 || Library.KEY_USER_regcode == rgn_id)
                             && (Library.KEY_USER_siteid == "nil" || Convert.ToInt32(Library.KEY_USER_siteid) == 0 || Library.KEY_USER_siteid == st_id))
                              dbConn.Insert(item);
                    }
                    dbConn.Commit();
                    _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
                };
            }
            catch (Exception excp)
            {
            }
        }

        private async static Task Sync_mstr_Cycledetails()
        {
            try
            {

                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_CYCLEDETAILS + "/" + Library.KEY_USER_ccode + "/" + Library.KEY_USER_regcode + "/" + Library.KEY_USER_siteid + "/0");
                HttpResponseMessage response = await httpClient.SendAsync(request);
               
                List<mstr_Cycledetails> jarray = JsonConvert.DeserializeObject<List<mstr_Cycledetails>>(await response.Content.ReadAsStringAsync());
                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_Cycledetails>();
                    dbConn.CreateTable<mstr_Cycledetails>();
                    dbConn.BeginTransaction();
                    foreach (var item in jarray)
                    {
                        string todate = "";
                        string fromdate = "";
                        string formattedto = "";
                        string formatedfrom = "";
                        try
                        {
                            // DateTime dt1= DateTime.ParseExact(row["tdate"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            DateTime dt1 = DateTime.ParseExact(item.todate.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            formattedto = dt1.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                            todate = String.Format("{0:yyyy-MM-dd 00:00:00.000}", dt1);
                            DateTime dt2 = DateTime.ParseExact(item.fromdate.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            formatedfrom = dt2.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                            fromdate = String.Format("{0:yyyy-MM-dd 00:00:00.000}", dt2);
                            // fromdate = row["fdate"].ToString();
                            DateTime to = Convert.ToDateTime(todate);
                            DateTime from = Convert.ToDateTime(fromdate);

                            formattedto = to.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                            formatedfrom = from.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                        }

                        string cont_id = item.country_id.ToString();
                        string rgn_id = item.region_id.ToString();
                        string st_id = item.site_id.ToString();

                        // Filtering data based on
                        //                    if (country_id1 == cont_id && (region_id1 == nil || region_id1 == 0 || region_id1 == rgn_id) && (site_id1 == nil || site_id1 == 0 || site_id1 == st_id))
                        if (Library.KEY_USER_ccode == cont_id && (Library.KEY_USER_regcode == "nil" || Convert.ToInt16(Library.KEY_USER_regcode) == 0 || Library.KEY_USER_regcode == rgn_id)
                            && (Library.KEY_USER_siteid == "nil" || Convert.ToInt32(Library.KEY_USER_siteid) == 0 || Library.KEY_USER_siteid == st_id))
                        {
                            item.formattedto = formattedto;
                            item.formattedfrom = formatedfrom;
                            item.todate = todate;
                            item.fromdate = fromdate;
                            dbConn.Insert(item);
                            //     dbhelper.Insert_INTO_mstr_Cycledetails(new mstr_Cycledetails(Convert.ToInt32(row["ID"].ToString()), row["cycle_name"].ToString(), row["cycle_description"].ToString(), row["cycle_type"].ToString(), row["fromdate"].ToString(), todate, row["sitecode"].ToString(), Convert.ToInt32(row["status_id"].ToString()), formattedto, formatedfrom, Convert.ToInt32(row["Cycle_id"].ToString())));
                        }
                    }
                    dbConn.Commit();
                    _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
                };
              
            }
            catch (Exception excp)
            {
            }
        }

        private async static Task Sync_mstr_meal_type()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_DISPLAYMEALTYPE + "/" + Library.KEY_USER_ccode);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();
                List<mstr_meal_type> jarray = JsonConvert.DeserializeObject<List<mstr_meal_type>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_meal_type>();
                    dbConn.CreateTable<mstr_meal_type>();

                    dbConn.BeginTransaction();
                    dbConn.InsertAll(jarray);
                    dbConn.Commit();
                };
                _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
            }
            catch (Exception excp)
            {
                
            }
        }

        private async static Task Sync_mstr_remarks()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_GETREMARKSDETAIL + "/" + Library.KEY_USER_ccode);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();
                List<mstr_remarks> jarray = JsonConvert.DeserializeObject<List<mstr_remarks>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_remarks>();
                    dbConn.CreateTable<mstr_remarks>();
                    dbConn.BeginTransaction();
                    dbConn.InsertAll(jarray);
                    dbConn.Commit();
                };
                _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
            }
            catch (Exception excp)
            {
            }
        }

        private async static Task Sync_mstr_ingredient()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_INGREDIENTLIST + "/" + Library.KEY_USER_ccode);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();
                List<mstr_ingredient> jarray = JsonConvert.DeserializeObject<List<mstr_ingredient>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_ingredient>();
                    dbConn.CreateTable<mstr_ingredient>();
                    dbConn.BeginTransaction();
                    dbConn.InsertAll(jarray);
                    dbConn.Commit();
                };
                _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
            }
            catch (Exception excp)
            {
            }
        }

        private async static Task Sync_mstr_fluid_master()
        {
            try
            {

                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_GETALLFLUIDCONSISTENCY + "/" + Library.KEY_USER_ccode);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();
                List<mstr_fluid_master> jarray = JsonConvert.DeserializeObject<List<mstr_fluid_master>>(await response.Content.ReadAsStringAsync());


                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_fluid_master>();
                    dbConn.CreateTable<mstr_fluid_master>();
                    dbConn.BeginTransaction();
                    dbConn.InsertAll(jarray);
                    dbConn.Commit();
                };
                _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
            }
            catch (Exception)
            {
            }
        }

        private async static Task Sync_mstr_diet_texture()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_GETALLDIETTEXTURE + "/" + Library.KEY_USER_ccode);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();
                List<mstr_diet_texture> jarray = JsonConvert.DeserializeObject<List<mstr_diet_texture>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_diet_texture>();
                    dbConn.CreateTable<mstr_diet_texture>();
                    dbConn.BeginTransaction();
                    dbConn.InsertAll(jarray);
                    dbConn.Commit();
                };
                _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
            }
            catch (Exception excp)
            {
            }
        }

        private async static Task Sync_mstr_diet_type()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_DISPLAYDIET_TYPE + "/" + Library.KEY_USER_ccode);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();
                List<mstr_diet_type> jarray = JsonConvert.DeserializeObject<List<mstr_diet_type>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_diet_type>();
                    dbConn.CreateTable<mstr_diet_type>();
                    dbConn.BeginTransaction();
                    dbConn.InsertAll(jarray);
                    dbConn.Commit();
                };
                _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
            }
            catch (Exception excp)
            {
            }
        }

        private async static Task Sync_mstr_bed_details()
        {
            try
            {

                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_DISPLAYBEDDETAILS + "/" + Library.KEY_USER_ccode + "/" + Library.KEY_USER_regcode + "/" + Library.KEY_USER_siteid + "/0");
                HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();
                List<mstr_bed_details> jarray = JsonConvert.DeserializeObject<List<mstr_bed_details>>(await response.Content.ReadAsStringAsync());


                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_bed_details>();
                    dbConn.CreateTable<mstr_bed_details>();
                    dbConn.BeginTransaction();
                    foreach (var item in jarray)
                    {

                        string cont_id = item.country_id.ToString();
                        string rgn_id = item.region_id;
                        string st_id = item.site_id.ToString();

                        // Filtering data based on

                        //                    if (country_id1 == cont_id && (region_id1 == nil || region_id1 == 0 || region_id1 == rgn_id) && (site_id1 == nil || site_id1 == 0 || site_id1 == st_id))
                        if (Library.KEY_USER_ccode == cont_id && (Library.KEY_USER_regcode == "nil" || Convert.ToInt16(Library.KEY_USER_regcode) == 0 || Library.KEY_USER_regcode == rgn_id)
                            && (Library.KEY_USER_siteid == "nil" || Convert.ToInt32(Library.KEY_USER_siteid) == 0 || Library.KEY_USER_siteid == st_id))
                        {
                            dbConn.Insert(item);
                            // dbhelper.Insert_INTO_Mstr_bed_details(new mstr_bed_details(Convert.ToInt32(row["ID"].ToString()), row["bed_no"].ToString(), Convert.ToInt32(row["ward_id"].ToString()), Convert.ToInt32(row["bedclass_id"].ToString()), Convert.ToInt32(row["status_id"].ToString()), row["site_code"].ToString()));

                        }
                    }
                    dbConn.Commit();
                    _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
                }
            }
            catch (Exception excp)
            {
            }
        }

        private async static Task Sync_mstr_allergies_master()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_ALLERGENTDIETLIST + "/" + Library.KEY_USER_ccode);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();
                List<mstr_allergies_master> jarray = JsonConvert.DeserializeObject<List<mstr_allergies_master>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_allergies_master>();
                    dbConn.CreateTable<mstr_allergies_master>();
                    dbConn.BeginTransaction();
                    dbConn.InsertAll(jarray);
                    dbConn.Commit();
                };
                _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
            }
            catch (Exception excp)
            {
            }
        }

        private async static Task Sync_mstr_bed_meal_class_mapping()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_BEDMEALCLASSMAPPING + "/" + Library.KEY_USER_ccode + "/" + Library.KEY_USER_regcode + "/" + Library.KEY_USER_siteid + "/0");
                HttpResponseMessage response = await httpClient.SendAsync(request);

                List<mstr_bed_meal_class_mapping> jarray = JsonConvert.DeserializeObject<List<mstr_bed_meal_class_mapping>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_bed_meal_class_mapping>();
                    dbConn.CreateTable<mstr_bed_meal_class_mapping>();
                    dbConn.BeginTransaction();
                    foreach (var item in jarray)
                    {
                        string cont_id = item.country_id.ToString();
                        string rgn_id = item.region_id;
                        string st_id = item.site_id.ToString();

                        // Filtering data based on
                        //                    if (country_id1 == cont_id && (region_id1 == nil || region_id1 == 0 || region_id1 == rgn_id) && (site_id1 == nil || site_id1 == 0 || site_id1 == st_id))
                        if (Library.KEY_USER_ccode == cont_id && (Library.KEY_USER_regcode == "nil" || Convert.ToInt16(Library.KEY_USER_regcode) == 0 || Library.KEY_USER_regcode == rgn_id)
                            && (Library.KEY_USER_siteid == "nil" || Convert.ToInt32(Library.KEY_USER_siteid) == 0 || Library.KEY_USER_siteid == st_id))
                        { 
                            dbConn.Insert(item);
                            // dbhelper.Insert_INTO_mstr_bed_meal_class_mapping(new mstr_bed_meal_class_mapping(Convert.ToInt32(row["ID"].ToString()), Convert.ToInt32(row["bed_class_id"].ToString()), row["bed_class_Name"].ToString(), Convert.ToInt32(row["meal_class_id"].ToString()), row["meal_class_Name"].ToString(), false, Convert.ToInt32(row["status_id"].ToString()), 1));
                        }
                    }
                    dbConn.Commit();
                    _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
                };
            }
            catch (Exception excp)
            {
            }
        }

        private async static Task Sync_mstr_mealclass()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_DISPLAYMEALCLASS + "/" + Library.KEY_USER_ccode + "/" + Library.KEY_USER_regcode + "/" + Library.KEY_USER_siteid + "/0");
                HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();
                List<mstr_mealclass> jarray = JsonConvert.DeserializeObject<List<mstr_mealclass>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_mealclass>();
                    dbConn.CreateTable<mstr_mealclass>();
                    dbConn.BeginTransaction();
                    foreach (var item in jarray)
                    {
                        string cont_id = item.country_id.ToString();
                        string rgn_id = item.region_id;
                        string st_id = item.site_id.ToString();

                        // Filtering data based on
                        //                    if (country_id1 == cont_id && (region_id1 == nil || region_id1 == 0 || region_id1 == rgn_id) && (site_id1 == nil || site_id1 == 0 || site_id1 == st_id))
                        if (Library.KEY_USER_ccode == cont_id && (Library.KEY_USER_regcode == "nil" || Convert.ToInt16(Library.KEY_USER_regcode) == 0 || Library.KEY_USER_regcode == rgn_id)
                            && (Library.KEY_USER_siteid == "nil" || Convert.ToInt32(Library.KEY_USER_siteid) == 0 || Library.KEY_USER_siteid == st_id))
                            dbConn.Insert(item);
                    }
                    dbConn.Commit();
                }
                _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.06f;
            }
            catch (Exception excp)
            {
            }
        }

        internal static async Task SyncMenuMaster(HomeMasterDetailPageViewModel homeMasterDetailPageViewModel)
        {
            _viewModel = homeMasterDetailPageViewModel;
            await Sync_mstr_menu_item();
            await Sync_mstr_menu_master();
        }



        public async static Task Sync_mstr_menu_item()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                httpClient.Timeout = TimeSpan.FromMinutes(20);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_NENUITEMDETAILS + "/" + Library.KEY_USER_ccode + "/" + Library.KEY_USER_regcode + "/" + Library.KEY_USER_siteid + "/0");
                HttpResponseMessage response = await httpClient.SendAsync(request);

                var str= await response.Content.ReadAsStringAsync();

                List<mstr_menu_item> jarray = JsonConvert.DeserializeObject<List<mstr_menu_item>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_menu_item>();
                    dbConn.CreateTable<mstr_menu_item>();
                    dbConn.BeginTransaction();

                    dbConn.InsertAll(jarray);
                    _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.5f;
                    dbConn.Commit();
                    
                };
                
            }
            catch (Exception excp)
            {
            }
        }
        public async static Task Sync_mstr_menu_master()
        {
            try
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                httpClient.Timeout = TimeSpan.FromMinutes(20);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Library.URL + "/" + Library.METHODE_NENU_MASETER + "/" + Library.KEY_USER_ccode + "/" + Library.KEY_USER_regcode + "/" + Library.KEY_USER_siteid + "/0");
                HttpResponseMessage response = await httpClient.SendAsync(request);
                // jarray= await response.Content.ReadAsStringAsync();
                List<mstr_menu_master> jarray = JsonConvert.DeserializeObject<List<mstr_menu_master>>(await response.Content.ReadAsStringAsync());

                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    dbConn.DropTable<mstr_menu_master>();
                    dbConn.CreateTable<mstr_menu_master>();
                    dbConn.BeginTransaction();

                    dbConn.InsertAll(jarray);
                    _viewModel.CurrentProgress = _viewModel.CurrentProgress + 0.5f;
                    dbConn.Commit();
                  

                };
                
            }
            catch (Exception)
            {
            }
        }
    }
}
