using Sodexo_JTH.Models;
using Sodexo_JTH.Repos;
using SQLite;

namespace Sodexo_JTH.Services
{
    public class MealOrderLocalManager : IMealOrderLocalManager
    {
        IGenericRepo<mstr_meal_order_local> _orderLocalRepo;
        public MealOrderLocalManager(IGenericRepo<mstr_meal_order_local> orderLocalRepo)
        {
            _orderLocalRepo = orderLocalRepo;
        }

        public TableQuery<mstr_meal_order_local> GetTable()
        {
            return _orderLocalRepo.QueryTable();
        }
    }
}
