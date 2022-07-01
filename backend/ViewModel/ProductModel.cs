using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FireApp.Model
{
    public class ProductModel
    {
        
        public string name { get; set; }
        public string cost { get; set; }
        public string description { get; set; }
        public string stock { get; set; }
        public string purchaseDate { get; set; }
        public string location { get; set; }
        public string manufctdate { get; set; }
        public string expiredate { get; set; }
    }
}