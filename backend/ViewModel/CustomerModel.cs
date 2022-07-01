using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FireApp.Model
{
    public class CustomerModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string details { get; set; }
        public string userid { get; set; }
        public string password { get; set; }
    }
}
