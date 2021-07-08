using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleDotNetWebApp.models;

namespace SimpleDotNetWebApp.data
{
    public class UserRepository:IUserRepository
    {

        /* depedency injection first*/
        private readonly UserContext _context;
        public UserRepository(UserContext context)
        {
            _context = context;
        }
        public User Create(User user)
        {
            _context.Users.Add(user);
            user.id = _context.SaveChanges();

            return user;
        }  
        public User getByEmail(string email)
        {
            

            return _context.Users.FirstOrDefault(u => u.email == email);
        }  
        public User getById(int id)
        {
            

            return _context.Users.FirstOrDefault(u => u.id == id);
        }
    }
}
