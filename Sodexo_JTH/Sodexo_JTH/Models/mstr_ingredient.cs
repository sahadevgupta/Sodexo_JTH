using Newtonsoft.Json;
using Prism.Mvvm;
using SQLite;

namespace Sodexo_JTH.Models
{
    public class mstr_ingredient : BindableBase
    {
        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UID { get; set; }
        public int ID { get; set; }
        //   public int tableid { get; set; }
        public string ingredient_name { get; set; }
        public string ingredient_description { get; set; }
        public int status_id { get; set; }
        public string site_code { get; set; }

        private bool _isChecked;
        [Ignore]
        [JsonIgnore]
        public bool IsChecked
        {
            get { return this._isChecked; }
            set { SetProperty(ref _isChecked, value); }
        }


    }
}
