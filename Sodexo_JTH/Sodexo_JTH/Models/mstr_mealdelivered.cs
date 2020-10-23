using Newtonsoft.Json;
using Prism.Mvvm;
using SQLite;
using System;

namespace Sodexo_JTH.Models
{
    public class mstr_mealdelivered : BindableBase
    {

        public int OrderedID { get; set; }
        public string date { get; set; }
        public string Ward { get; set; }
        public string Bed { get; set; }
        public string Name { get; set; }
        public string MealTime { get; set; }
        public string OrderBy { get; set; }
        public string Guest { get; set; }
        public string pateint_id { get; set; }
        public bool is_verifed { get; set; }
        public string status { get; set; }
        private bool _is_checked;
        public bool is_checked
        {
            get { return _is_checked; }
            set { SetProperty(ref _is_checked, value); }
        }
        [Ignore]
        [JsonIgnore]
        public string ward_bed { get; set; }
        public Boolean istrue { get; set; }
        [Ignore]
        [JsonIgnore]
        public int SrNo { get; set; }


        private string _patientNRIC;
        [JsonProperty("PatientNRIC")]
        public string PatientNRIC
        {
            get { return this._patientNRIC; }
            set { SetProperty(ref _patientNRIC, value); }
        }

    }
}
