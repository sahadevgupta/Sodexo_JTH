using Sodexo_JTH.Models;
using SQLite;

namespace Sodexo_JTH.Services
{
    public interface IMealOrderLocalManager
    {
        TableQuery<mstr_meal_order_local> GetTable();
    }
}