using System;
using System.IO;
using Android.OS;
using Sodexo_JTH.Droid.Services;
using Sodexo_JTH.Interfaces;
using SQLite;
using Xamarin.Forms;
using Environment = System.Environment;

[assembly: Dependency(typeof(SQLite_droid))]
namespace Sodexo_JTH.Droid.Services
{
    public class SQLite_droid : IDBInterface
    {
        public SQLiteConnection conn;
        public SQLiteConnection GetConnection()
        {
            InitializeDB();
            return conn;
        }

        public void InitializeDB()
        {
            string filename = "SODEXO.db3";
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
            conn = new SQLite.SQLiteConnection(path);
        }
    }
}