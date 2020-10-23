namespace Sodexo_JTH.Models
{
    public class mstr_bed_details
    {
        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UID { get; set; }
        public int ID { get; set; }
        //   public int tableid { get; set; }
        public string bed_no { get; set; }
        public int ward_id { get; set; }
        public int bedclass_id { get; set; }
        public int status_id { get; set; }
        //  public string site_code { get; set; }
        public int site_id { get; set; }
        public string country_id { get; set; }
        public string region_id { get; set; }



        public mstr_bed_details()
        {

        }
        //public mstr_bed_details(int Para1, string Para2, int Para3, int Para4, int Para5, string Para6)
        //{
        //    tableid = Para1;
        //    bed_no = Para2;
        //    ward_id = Para3;
        //    bedclass_id = Para4;
        //    status_id = Para5;
        //    site_code = Para6;


        //}
        // Inserting into mstr_bed_details
        ////public void Insert(int Para1, string Para2, int Para3, int Para4, int Para5, string Para6)
        ////{
        ////    DatabaseHelperClass Db_Helper = new DatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs  
        ////    try
        ////    {
        ////        Db_Helper.Insert_INTO_Mstr_bed_details(new mstr_bed_details(Para1, Para2, Para3, Para4, Para5, Para6));
        ////    }
        ////    catch (Exception e)
        ////    {

        ////    }

        ////}

    }
}
