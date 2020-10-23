namespace Sodexo_JTH.Models
{
    public class mstr_meal_history
    {
        public string beveragesid { get; set; }
        public string dessertid { get; set; }
        public string entreeid { get; set; }
        public string addonid { get; set; }
        public string juiceid { get; set; }
        public string orderdate { get; set; }
        public string remarkid { get; set; }
        public string soupid { get; set; }
        public string status { get; set; }

        public string mealtimename { get; set; }
        public string mealtimeid { get; set; }
        public string meal_detail_id { get; set; }
        public string createdby { get; set; }
        public string remarks { get; set; }
        public string Id { get; set; }
        public string remark { get; set; }
        public string ward_bed { get; set; }
        public string wardid { get; set; }
        public string Cut_Off_time { get; set; }

        public mstr_meal_history()
        {

        }

        public mstr_meal_history(string Para1, string Para2, string Para3, string Para4, string Para5, string Para6, string Para7, string Para8)
        {
            beveragesid = Para1;
            dessertid = Para2;
            entreeid = Para3;
            juiceid = Para4;
            orderdate = Para5;
            remarkid = Para6;
            soupid = Para7;
            status = Para8;
        }

    }
}
