namespace Sodexo_JTH.Models
{
    public class mstr_mealclass
    {
        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UID { get; set; }
        public int ID { get; set; }
        // public int tableid { get; set; }
        public string meal_Class_name { get; set; }
        public string meal_Class_description { get; set; }
        public bool is_A_la_carte { get; set; }
        public int status_id { get; set; }
        public string site_code { get; set; }
        public int site_id { get; set; }
        public string country_id { get; set; }
        public string region_id { get; set; }

        public mstr_mealclass()
        {

        }

        //public mstr_mealclass(int Para1, string Para2, string Para3, bool Para4, int Para5, string Para6)
        //{
        //    tableid = Para1;
        //    mealclass_name = Para2;
        //    mealclass_description = Para3;
        //    is_A_la_carte = Para4;
        //    status_id = Para5;
        //    site_code = Para6;

        //}
    }
}
