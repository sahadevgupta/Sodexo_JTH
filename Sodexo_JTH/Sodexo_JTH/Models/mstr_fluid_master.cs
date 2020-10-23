namespace Sodexo_JTH.Models
{
    class mstr_fluid_master
    {
        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UID { get; set; }
        public int ID { get; set; }
        //    public int tableid { get; set; }
        public string fluid_name { get; set; }
        public string fluid_description { get; set; }
        public int status_id { get; set; }
        public string site_code { get; set; }



        public mstr_fluid_master()
        {

        }

    }
}
