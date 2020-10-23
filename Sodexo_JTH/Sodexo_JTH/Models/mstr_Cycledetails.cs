namespace Sodexo_JTH.Models
{
    class mstr_Cycledetails
    {
        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UID { get; set; }
        public int ID { get; set; }
        //  public int tableid { get; set; }

        public string cycle_name { get; set; }
        public string cycle_description { get; set; }
        public string cycletype { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string sitecode { get; set; }
        public int status_id { get; set; }
        public string formattedto { get; set; }
        public string formattedfrom { get; set; }
        public int Cycle_id { get; set; }
        public int country_id { get; set; }
        public int region_id { get; set; }
        public int site_id { get; set; }

        public mstr_Cycledetails()
        {

        }


        //public mstr_Cycledetails(int Para1, string Para2, string Para3, string Para4, string Para5, string Para6, string Para7, int Para8,string Para9,string Para10,int Para11)
        //{
        //    tableid = Para1;
        //    cycle_name = Para2;
        //    cycle_description = Para3;
        //    cycletype = Para4;
        //    fromdate = Para5;
        //    todate = Para6;
        //    sitecode = Para7;
        //    status_id = Para8;
        //    formattedto = Para9;
        //    formattedfrom = Para10;
        //    Cycle_id = Para11;
        //}

    }
}

