using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleDotNetWebApp.Dtos
{
    /*data that come from the frontend*/
    public class RegisterDto
    {
        public string name { set; get; }
        public string email { set; get; }
        public string password { set; get; }
    }
}
