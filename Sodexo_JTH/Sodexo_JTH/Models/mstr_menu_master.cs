using Newtonsoft.Json;


namespace Sodexo_JTH.Models
{
    class mstr_menu_master
    {
        //Creating table
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UID { get; set; }
        public int ID { get; set; }
        //  public int tableid { get; set; }
        public string menu_code { get; set; }
        public string menu_name { get; set; }

        public string menu_description { get; set; }
        public int meal_class_id { get; set; }
        [JsonProperty("mealclass ")]
        public string mealclass { get; set; }
        public string meal_class_ids { get; set; }
        public int age_id { get; set; }

        [JsonProperty("agename")]
        public string agename { get; set; }
        public string menu_days { get; set; }
        //...................................
        public bool Confinement { get; set; }
        public int diet_id { get; set; }
        [JsonProperty("diet")]
        public string diet { get; set; }

        [JsonProperty("meal_time_name")]
        public string meal_time_name { get; set; }
        public string menu_image { get; set; }
        public byte[] ImageData { get; set; }
        public int status_id { get; set; }
        public string site_code { get; set; }
        public string TH_Code { get; set; }
        [JsonProperty("Therapeutic_ids")]
        public string Therapeutic_ids { get; set; }
        public string ingredient_name { get; set; }
        public string menu_time_name { get; set; }
        public string menu_time_ids { get; set; }

        public string menu_item_name { get; set; }
        public string cycle_ides { get; set; }
        public string menu_item_ides { get; set; }
        public string cycle_name { get; set; }

        public string wardtypename { get; set; }
        [JsonProperty("menu_name_local_language")]
        public string local_language { get; set; }

        public string menu_item_name_local_language { get; set; }

        public string btnname { get; set; }

        public string btncolor { get; set; }
        public int country_id { get; set; }
        public int region_id { get; set; }
        public int site_id { get; set; }

        //   public byte[] ImageData { get;  set; }

        // public int ID { get; set; }

        //public string MealType { get; set; }

        public bool is_veg { get; set; }
        public bool is_halal { get; set; }

        [JsonProperty("isInfantDiet")]
        public bool is_InfantDiet { get; set; }

        [JsonProperty("MealType")]
        public string meal_type_name { get; set; }
        [JsonProperty("Meal_Type_Id")]
        public string Meal_Type_Id { get; set; }

    }
}
