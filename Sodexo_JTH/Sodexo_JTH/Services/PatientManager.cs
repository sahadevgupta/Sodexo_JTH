using Newtonsoft.Json;
using Sodexo_JTH.Helpers;
using Sodexo_JTH.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sodexo_JTH.Services
{
    public class PatientManager : IPatientManager
    {

        public PatientManager()
        {

        }

        public async Task<ObservableCollection<mstr_patient_info>> GetPatientsByWardBed(string DateFormat, int wardID, int bedID)
        {
            HttpClient httpClient = new System.Net.Http.HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{Library.URL}/{Library.METHOD_PULLPATIENTSBYWARD}/{Library.KEY_USER_SiteCode}/{DateFormat}/{wardID}/{bedID}");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ObservableCollection<mstr_patient_info>>(data);
        }
    }
}
