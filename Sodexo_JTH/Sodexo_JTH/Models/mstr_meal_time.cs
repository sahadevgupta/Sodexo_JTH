using Newtonsoft.Json;
using Prism.Mvvm;
using SQLite;

namespace Sodexo_JTH.Models
{
    public class mstr_meal_time : BindableBase
    {
        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UID { get; set; }
        public int ID { get; set; }
        // public int tableid { get; set; }
        public string meal_name { get; set; }
        public string normal_start_time { get; set; }
        public string normal_end_time { get; set; }
        public bool is_normal_time_applicable { get; set; }
        public string cut_off_start_time { get; set; }
        public string cut_off_end_time { get; set; }
        public bool is_cut_off_time_applicable { get; set; }
        public string late_cut_off_start_time { get; set; }
        public string late_cut_off_end_time { get; set; }
        public bool is_late_cut_off_time_applicable { get; set; }
        public int status_id { get; set; }
        public string site_code { get; set; }

        public bool isBF { get; set; }
        public bool isLH { get; set; }
        public bool isDN { get; set; }

        public int country_id { get; set; }
        public int region_id { get; set; }
        public int site_id { get; set; }


        private bool _isTapped;
        [Ignore]
        [JsonIgnore]
        public bool IsTapped
        {
            get { return this._isTapped; }
            set { SetProperty(ref _isTapped, value); }
        }

        public mstr_meal_time()
        {

        }
        //public mstr_meal_time(int Para1, string Para2, string Para3, string Para4, bool Para5, string Para6, string Para7, bool Para8, string Para9, string Para10, bool Para11, int Para12, string Para13, bool Para14, bool Para15, bool Para16,int Para17,int Para18,int Para19)
        //{
        //    tableid = Para1;
        //    meal_name = Para2;
        //    normal_start_time = Para3;
        //    normal_end_time = Para4;
        //    is_normal_time_applicable = Para5;
        //    cut_off_start_time = Para6;
        //    cut_off_end_time = Para7;
        //    is_cut_off_time_applicable = Para8;
        //    late_cut_off_start_time = Para9;
        //    late_cut_off_end_time = Para10;
        //    is_late_cut_off_time_applicable = Para11;
        //    status_id = Para12;
        //    site_code = Para13;
        //    isBF = Para14;
        //    isLH = Para15;
        //    isDN = Para16;
        //    country_id = Para17;
        //    region_id = Para18;
        //    site_id = Para19;

        //}

        // Inserting into mstr_meal_time
        //public void Insert(int Para1, string Para2, string Para3, string Para4, bool Para5, string Para6, string Para7, bool Para8, string Para9, string Para10, bool Para11, int Para12, string Para13, bool Para14, bool Para15, bool Para16, int Para17, int Para18, int Para19)
        //{
        //    DatabaseHelperClass Db_Helper = new DatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs  
        //    try
        //    {
        //        Db_Helper.Insert_INTO_mstr_meal_time(new mstr_meal_time(Para1, Para2, Para3, Para4, Para5, Para6, Para7, Para8, Para9, Para10, Para11, Para12, Para13,Para14,Para15,Para16,Para17,Para18,Para19));
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //}
    }
}
