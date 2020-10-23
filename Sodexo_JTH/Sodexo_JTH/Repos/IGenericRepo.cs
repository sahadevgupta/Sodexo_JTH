using SQLite;
using System.Collections.Generic;

namespace Sodexo_JTH.Repos
{
    public interface IGenericRepo<T>
    {
        void Delete(string id);
        void Insert(T a);
        void InsertAll(IEnumerable<T> a);
        void InsertOrReplace(T a);
        TableQuery<T> QueryTable();
        void Update(T a);
    }
}