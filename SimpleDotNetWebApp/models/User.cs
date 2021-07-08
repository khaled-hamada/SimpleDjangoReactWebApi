using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace SimpleDotNetWebApp.models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
       public string email { get; set; }

        [JsonIgnore] public string password { get; set; }
   
    }
}
