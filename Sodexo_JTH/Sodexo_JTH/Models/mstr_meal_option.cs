namespace Sodexo_JTH.Models
{
    public class mstr_meal_option
    {
        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UID { get; set; }
        public int ID { get; set; }
        //  public int tableid { get; set; }
        public string meal_option_name { get; set; }
        public string meal_option_description { get; set; }
        public int status_id { get; set; }
        public string site_code { get; set; }




        //public mstr_meal_option(int Para1, string Para2, string Para3, int Para4, string Para5)
        //{
        //    tableid = Para1;
        //    meal_option_name = Para2;
        //    meal_option_description = Para3;
        //    status_id = Para4;
        //    site_code = Para5;


        //}

        // Inserting into mstr_meal_option
        //public void Insert(int Para1, string Para2, string Para3, int Para4, string Para5)
        //{
        //    DatabaseHelperClass Db_Helper = new DatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs  
        //    try
        //    {
        //        Db_Helper.Insert_INTO_mstr_meal_option(new mstr_meal_option(Para1, Para2, Para3, Para4, Para5));
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //}
    }
}
