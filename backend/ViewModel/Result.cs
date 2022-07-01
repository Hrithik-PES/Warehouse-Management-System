using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireApp.ViewModel
{
    public class ResultRecord
    {
        public long id { get; set; }
        public long camera_id { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string datetime { get; set; }
        public byte[] Photo { get; set; }
        public string detection_type { get; set; }
        public string similarity { get; set; }
    }
}
