using Prism.Mvvm;

namespace Sodexo_JTH.Models
{
    public class mstr_menu_item_category : BindableBase
    {
        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UID { get; set; }
        public int ID { get; set; }
        //  public int tableid { get; set; }
        public string meal_item_name { get; set; }
        public string meal_item_description { get; set; }
        public int status_id { get; set; }
        public string sitecode { get; set; }
        public string country_id { get; set; }
        public string imgname { get; set; }
        public string selimgname { get; set; }
        public string fcolor { get; set; }
        public string selcolor { get; set; }


        private bool _categoryVisibility;

        public bool CategoryVisibility
        {
            get { return this._categoryVisibility; }
            set { SetProperty(ref _categoryVisibility, value); }
        }





        public mstr_menu_item_category()
        {

        }
        //public mstr_menu_item_category(int Para1, string Para2, string Para3, int Para4, string Para5)
        //{
        //    tableid = Para1;
        //    meal_item_name = Para2;
        //    meal_item_description = Para3;
        //    status_id = Para4;
        //    site_code = Para5;
        //}

        // Inserting into mstr_menu_item_category
        //public void Insert(int Para1, string Para2, string Para3, int Para4, string Para5)
        //{
        //    DatabaseHelperClass Db_Helper = new DatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs  
        //    try
        //    {
        //        Db_Helper.Insert_INTO_mstr_menu_item_category(new mstr_menu_item_category(Para1, Para2, Para3, Para4, Para5));
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}
    }
}
