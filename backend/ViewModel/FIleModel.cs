using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FireApp.Model
{
    public class FileModel
    {        
        public IFormFile Img1 { get; set; }
        public string AccessorName { get; set; }
    }
}
