namespace Sodexo_JTH.Models
{
    public class mstr_remarks
    {
        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UID { get; set; }
        public int ID { get; set; }
        // public int tableid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status_id { get; set; }
        public string Sitecode { get; set; }
        public string country_id { get; set; }




        public mstr_remarks()
        {

        }
        //public mstr_remarks(int Para1, string Para2, string Para3, int Para4, string Para5)
        //{
        //    tableid = Para1;
        //    remark_name = Para2;
        //    remark_description = Para3;
        //    status_id = Para4;
        //    site_code = Para5;


        //}
        // Inserting into mstr_diet_type

    }
}
