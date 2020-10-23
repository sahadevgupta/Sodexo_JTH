using Sodexo_JTH.Interfaces;
using Sodexo_JTH.UWP.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;

[assembly:Dependency(typeof(SQLite_UWP))]
namespace Sodexo_JTH.UWP.Services
{
    public class SQLite_UWP : IDBInterface
    {
       public SQLiteConnection conn;
        public SQLite_UWP()
        {
           // InitializeDB();
        }

        private void InitializeDB()
        {
            string filename = "SODEXO.db3";
            var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
            conn = new SQLite.SQLiteConnection(path);
        }

        public SQLiteConnection GetConnection()
        {
            InitializeDB();
            return conn;
        }

        void IDBInterface.InitializeDB()
        {
            InitializeDB();
        }
    }
}
