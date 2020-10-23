using System;

namespace Sodexo_JTH.Models
{
    public class mstr_meal_order_local
    {

        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int autoid { get; set; }

        public int age_id { get; set; }
        public int ward_id { get; set; }
        public int ward_type_id { get; set; }//optional
        public int bed_id { get; set; }
        public int allergies_id { get; set; }
        public Boolean is_vegitarian { get; set; }
        public Boolean is_halal { get; set; }
        public Boolean disposable_tray { get; set; }
        public int order_id { get; set; }
        public string order_date { get; set; }
        public string order_no { get; set; }
        public Boolean is_care_giver { get; set; }
        public string site_code { get; set; }
        public int createdby { get; set; }
        public int P_id { get; set; }//patient_table_id
        public int BF { get; set; }//breakfast place order=1, NBM=3,deliverd=2
        public int LH { get; set; }//lunch  place order=1, NBM=3,deliverd=2
        public int DN { get; set; } //dinner  place order=1, NBM=3,deliverd=2
        public string Therapeutic_ids { get; set; }
        public string ingredeint_ids { get; set; }
        public string other_ids { get; set; }
        public int meal_time_id { get; set; }
        public int meal_type_id { get; set; }
        public int meal_soup_id { get; set; }
        public int meal_option_id { get; set; }
        public int meal_menu_juice_item_id { get; set; }
        public int meal_diet_id { get; set; }
        public int meal_entree_id { get; set; }
        public int meal_beverage_id { get; set; }
        public int meal_desert_id { get; set; }
        public int remark_id { get; set; }
        public string meal_remark { get; set; }

        public int ID { get; set; }
        public string adult_child { get; set; }
        public int bedclass_id { get; set; }//optional
        public string bedclass_name { get; set; }
        public string doctor_comment { get; set; }
        public string general_comment { get; set; }
        public string patient_age { get; set; }
        public int patient_id { get; set; }
        public string patient_name { get; set; }
        public string patient_race { get; set; }
        public string preferred_server { get; set; }
        public string ward_bed { get; set; }

        public bool isdietician { get; set; }
        public bool is_staff { get; set; }
        public string staff_name { get; set; }
        public string dietician_comment { get; set; }
        public int Is_Late_Ordered { get; set; }

        public string role { get; set; }
        public string orderstatus { get; set; }


        public int mode_of_payment { get; set; }
        public string payment_remark { get; set; }



        public string Diet_Texture { get; set; }
        public string Fluid_Consistency { get; set; }
        public string allergies_ids { get; set; }
        public int caregiverentree { get; set; }
        public string caregiverlist { get; set; }
        public int fluid { get; set; }
        public int meal_class_id { get; set; }
        public int role_Id { get; set; }
        public int therapeutic_condition_id { get; set; }
        public int therapeutic_id { get; set; }
        public string system_module { get; set; }
        public string work_station_IP { get; set; }

        public string Meal_Type { get; set; }

        public int meal_addon_id { get; set; }

    }
}