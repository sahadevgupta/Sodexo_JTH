namespace Sodexo_JTH.Models
{
    public class mstr_bed_meal_class_mapping
    {
        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UID { get; set; }

        public int ID { get; set; }
        // public int tableid { get; set; }
        public int bed_class_id { get; set; }
        public string bed_class_Name { get; set; }
        public int meal_class_id { get; set; }
        public string meal_class_Name { get; set; }
        public bool Is_A_la_carte { get; set; }
        public int status_id { get; set; }
        public int site_id { get; set; }
        public string country_id { get; set; }
        public string region_id { get; set; }



        public mstr_bed_meal_class_mapping()
        {

        }

        //    public mstr_bed_meal_class_mapping(int Para1, int Para2, string Para3, int Para4, string Para5, bool Para6, int Para7, int Para8)
        //    {
        //        tableid = Para1;
        //            bed_class_id = Para2;
        //        bedclass_name = Para3;
        //        meal_class_id = Para4;
        //        mealclass_name = Para5;
        //        Is_A_la_carte = Para6;
        //        status_id = Para7;
        //        site_id = Para8;
        //    }
    }
}
