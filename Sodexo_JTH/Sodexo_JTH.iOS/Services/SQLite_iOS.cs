using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using Sodexo_JTH.Interfaces;
using Sodexo_JTH.iOS.Services;
using SQLite;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace Sodexo_JTH.iOS.Services
{
    class SQLite_iOS : IDBInterface
    {
        SQLiteConnection conn;
        public SQLiteConnection GetConnection()
        {
            InitializeDB();
            return conn;
        }

        public void InitializeDB()
        {
            string filename = "SODEXO.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
            conn = new SQLite.SQLiteConnection(path);
        }
    }
}