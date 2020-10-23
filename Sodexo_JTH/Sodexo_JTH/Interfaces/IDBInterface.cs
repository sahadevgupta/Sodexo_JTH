using SQLite;

namespace Sodexo_JTH.Interfaces
{
    public interface IDBInterface
    {
        SQLiteConnection GetConnection();
        void InitializeDB();
    }
}
