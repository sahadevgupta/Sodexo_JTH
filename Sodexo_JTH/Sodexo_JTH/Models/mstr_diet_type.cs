namespace Sodexo_JTH.Models
{
    public class mstr_diet_type
    {
        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UID { get; set; }
        public int ID { get; set; }
        //    public int tableid { get; set; }
        public string diet_name { get; set; }
        public string diet_description { get; set; }
        public int status_id { get; set; }
        public string site_code { get; set; }



        public mstr_diet_type()
        {

        }
        //public mstr_diet_type(int Para1, string Para2, string Para3, int Para4, string Para5)
        //{
        //    tableid = Para1;
        //    diet_name = Para2;
        //    diet_description = Para3;
        //    status_id = Para4;
        //    site_code = Para5;


        //}
        // Inserting into mstr_diet_type
        //public void Insert(int Para1, string Para2, string Para3, int Para4, string Para5)
        //{
        //    DatabaseHelperClass Db_Helper = new DatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs  
        //    try
        //    {
        //        Db_Helper.Insert_INTO_mstr_diet_type(new mstr_diet_type(Para1, Para2, Para3, Para4, Para5));
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //}
    }
}
