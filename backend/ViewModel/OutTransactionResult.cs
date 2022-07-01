using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireApp.ViewModel
{
    public class OutTransactionResultRecord
    {
        public string supplierName { get; set; }
        public string productName { get; set; }
        public string productStock { get; set; }
        public string productUnits { get; set; }
        public string amount { get; set; }
        public string id { get; set; }
        public string date { get; set; }
    }
}
