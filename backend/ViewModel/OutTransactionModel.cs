using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FireApp.Model
{
    public class OutTransactionModel
    {
        public string supplierName { get; set; }
        public string productName { get; set; }
        public int productStock { get; set; }
        //public int productUnits { get; set; }

    }
}
