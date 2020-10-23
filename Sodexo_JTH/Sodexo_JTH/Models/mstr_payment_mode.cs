using System;
using System.Collections.Generic;
using System.Text;

namespace Sodexo_JTH.Models
{
    public class mstr_DisplayPaymentModeDetails
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UID { get; set; }
        public int ID { get; set; }
        public string payment_mode_name { get; set; }

        public bool status_id { get; set; }
    }
}
