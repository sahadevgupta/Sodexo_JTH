using Sodexo_JTH.Interfaces;
using Sodexo_JTH.Models;
using SQLite;
using System.Linq;
using Xamarin.Forms;

namespace Sodexo_JTH.Helpers
{
    public static class DevelopmentCode
    {
        public static void CreateTables()
        {
            SQLiteConnection conn = DependencyService.Get<IDBInterface>().GetConnection();
            
            var WardDetailInfo = conn.GetTableInfo(nameof(mstr_ward_details));
            if (!WardDetailInfo.Any())
            {
                conn.CreateTable<mstr_ward_details>();
            }
            var MstrtherapeuticInfo = conn.GetTableInfo(nameof(mstr_therapeutic));
            if (!MstrtherapeuticInfo.Any())
            {
                conn.CreateTable<mstr_therapeutic>();
            }
            var MstrPatientInfo = conn.GetTableInfo(nameof(mstr_patient_info));
            if (!MstrPatientInfo.Any())
            {
                conn.CreateTable<mstr_patient_info>();
            }
            var OtherMstrInfo = conn.GetTableInfo(nameof(mstr_others_master));
            if (!OtherMstrInfo.Any())
            {
                conn.CreateTable<mstr_others_master>();
            }
            var MstrMenuInfo = conn.GetTableInfo(nameof(mstr_menu_master));
            if (!MstrMenuInfo.Any())
            {
                conn.CreateTable<mstr_menu_master>();
            }
            var ItemCategoryInfo = conn.GetTableInfo(nameof(mstr_menu_item_category));
            if (!ItemCategoryInfo.Any())
            {
                conn.CreateTable<mstr_menu_item_category>();
            }
            var MenuItemInfo = conn.GetTableInfo(nameof(mstr_menu_item));
            if (!MenuItemInfo.Any())
            {
                conn.CreateTable<mstr_menu_item>();
            }
            var MealtimeInfo = conn.GetTableInfo(nameof(mstr_meal_time));
            if (!MealtimeInfo.Any())
            {
                conn.CreateTable<mstr_meal_time>();
            }
            var MealTypeInfo = conn.GetTableInfo(nameof(mstr_meal_type));
            if (!MealTypeInfo.Any())
            {
                conn.CreateTable<mstr_meal_type>();
            }
            var MealOptInfo = conn.GetTableInfo(nameof(mstr_meal_option));
            if (!MealOptInfo.Any())
            {
                conn.CreateTable<mstr_meal_option>();
            }
            var MstrIngInfo = conn.GetTableInfo(nameof(mstr_ingredient));
            if (!MstrIngInfo.Any())
            {
                conn.CreateTable<mstr_ingredient>();
            }
            var MstrDietTypeInfo = conn.GetTableInfo(nameof(mstr_diet_type));
            if (!MstrDietTypeInfo.Any())
            {
                conn.CreateTable<mstr_diet_type>();
            }
            var DietTextureInfo = conn.GetTableInfo(nameof(mstr_diet_texture));
            if (!DietTextureInfo.Any())
            {
                conn.CreateTable<mstr_diet_texture>();
            }
            var FluidMstrInfo = conn.GetTableInfo(nameof(mstr_fluid_master));
            if (!FluidMstrInfo.Any())
            {
                conn.CreateTable<mstr_fluid_master>();
            }
            var BedDetailsInfo = conn.GetTableInfo(nameof(mstr_bed_details));
            if (!BedDetailsInfo.Any())
            {
                conn.CreateTable<mstr_bed_details>();
            }
            var AllergiesMstrInfo = conn.GetTableInfo(nameof(mstr_allergies_master));
            if (!AllergiesMstrInfo.Any())
            {
                conn.CreateTable<mstr_allergies_master>();
            }
            
            var MealClassInfo = conn.GetTableInfo(nameof(mstr_mealclass));
            if (!MealClassInfo.Any())
            {
                conn.CreateTable<mstr_mealclass>();
            }
            var OrderLocalInfo = conn.GetTableInfo(nameof(mstr_meal_order_local));
            if (!OrderLocalInfo.Any())
            {
                conn.CreateTable<mstr_meal_order_local>();
            }
            var BedMealMappingInfo = conn.GetTableInfo(nameof(mstr_bed_meal_class_mapping));
            if (!BedMealMappingInfo.Any())
            {
                conn.CreateTable<mstr_bed_meal_class_mapping>();
            }
            var MstrRemarksInfo = conn.GetTableInfo(nameof(mstr_remarks));
            if (!MstrRemarksInfo.Any())
            {
                conn.CreateTable<mstr_remarks>();
            }
            var CycledetailsInfo = conn.GetTableInfo(nameof(mstr_Cycledetails));
            if (!CycledetailsInfo.Any())
            {
                conn.CreateTable<mstr_Cycledetails>();
            }
           
        }
    }
}
