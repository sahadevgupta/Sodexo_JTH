namespace Sodexo_JTH.Models
{
    public class mstr_ward_details
    {
        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UID { get; set; }
        public int ID { get; set; }
        //  public int tableid { get; set; }
        public string ward_name { get; set; }
        public string ward_description { get; set; }
        public int ward_type { get; set; }

        public int status_id { get; set; }
        public string site_code { get; set; }
        public string ward_type_name { get; set; }
        public int country_id { get; set; }
        public int region_id { get; set; }
        public int site_id { get; set; }

        public mstr_ward_details()
        {

        }
        //public mstr_ward_details(int Para1, String Para2, string Para3, int Para4, int Para5, string Para6,string Para7)
        //{
        //    tableid = Para1;
        //    ward_name = Para2;
        //    ward_description = Para3;
        //    ward_type = Para4;
        //    status_id = Para5;
        //    site_code = Para6;
        //    ward_type_name = Para7;

        //}

        // Inserting into mstr_ward_details
        //public void Insert(int Para1, String Para2, string Para3, int Para4, int Para5, string Para6,string Para7)
        //{
        //    DatabaseHelperClass Db_Helper = new DatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs  
        //    try
        //    {
        //        Db_Helper.Insert_INTO_mstr_ward_details(new mstr_ward_details(Para1, Para2, Para3, Para4, Para5, Para6,Para7));
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}
    }
}
