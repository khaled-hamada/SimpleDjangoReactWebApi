using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleDotNetWebApp.models;

namespace SimpleDotNetWebApp.data
{
    public interface IUserRepository 
    {
        User Create(User user);
        User getByEmail(string email); 
        User getById(int id);
    }
}
