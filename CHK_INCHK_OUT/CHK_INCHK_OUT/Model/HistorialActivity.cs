using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHK_INCHK_OUT.Model
{
    public class HistorialActivity
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string userID { get; set; }
        public bool checkInCheckOut { get; set; }
        //public DateTime date { get; set; }
        public string activityID { get; set; }

    }
   
}
