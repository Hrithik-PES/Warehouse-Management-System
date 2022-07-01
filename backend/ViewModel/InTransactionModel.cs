using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FireApp.Model
{
    public class InTransactionModel
    {
        public string customerName { get; set; }
        public string productName { get; set; }
        public string productStock { get; set; }
        public string productUnits { get; set; }
        
    }
}
