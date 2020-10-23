namespace Sodexo_JTH.Models
{
    class mstr_patient
    {
        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int id { get; set; }

        public string admission_date { get; set; }
        public string age_name { get; set; }
        public int age_id { get; set; }
        public int bed_class_id { get; set; }
        public string bed_class_name { get; set; }
        public string bed_name { get; set; }
        public int bed_id { get; set; }
        public string birthdate { get; set; }
        public string created_on { get; set; }
        public int created_by { get; set; }
        public string discharge_date { get; set; }
        public string doctor_comment { get; set; }
        public string gender { get; set; }
        public string general_comment { get; set; }
        public string NRIC { get; set; }
        public int tableid { get; set; }
        public string patient_name { get; set; }
        public string patient_age { get; set; }
        public string patient_race { get; set; }
        public string prefered_server { get; set; }
        public string religion { get; set; }
        public string site_code { get; set; }
        public int status_id { get; set; }
        public int ward_id { get; set; }
        public string ward_name { get; set; }


        public mstr_patient()
        {

        }
        public mstr_patient(string Para1, string Para2, int Para3, int Para4, string Para5, string Para6, int Para7, string Para8, string Para9, int Para10, string Para11, string Para12, string Para13, string Para14, string Para15, int Para16, string Para17, string Para18, string Para19, string Para20, string Para21, string Para22, int Para23, int Para24, string Para25)
        {
            admission_date = Para1;
            age_name = Para2;
            age_id = Para3;
            bed_class_id = Para4;
            bed_class_name = Para5;
            bed_name = Para6;
            bed_id = Para7;
            birthdate = Para8;
            created_on = Para9;
            created_by = Para10;
            discharge_date = Para11;
            doctor_comment = Para12;
            gender = Para13;
            general_comment = Para14;
            NRIC = Para15;
            tableid = Para16;
            patient_name = Para17;
            patient_age = Para18;
            patient_race = Para19;
            prefered_server = Para20;
            religion = Para21;
            site_code = Para22;
            status_id = Para23;
            ward_id = Para24;
            ward_name = Para25;

        }
        //Inserting into mstr_patient
        //public void Insert(string Para1, string Para2, int Para3, int Para4, string Para5, string Para6, int Para7, string Para8, string Para9, int Para10, string Para11, string Para12, string Para13, string Para14, string Para15, int Para16, string Para17, string Para18, string Para19, string Para20, string Para21, string Para22, int Para23, int Para24, string Para25)
        //{
        //    DatabaseHelperClass Db_Helper = new DatabaseHelperClass();//Creating object for DatabaseHelperClass.cs from ViewModel/DatabaseHelperClass.cs  
        //    try
        //    {
        //        Db_Helper.Insert_INTO_mstr_patient(new mstr_patient(Para1, Para2, Para3, Para4, Para5, Para6, Para7, Para8, Para9, Para10, Para11, Para12, Para13, Para14, Para15, Para16, Para17, Para18, Para19, Para20, Para21, Para22, Para23, Para24, Para25));
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}
    }
}
