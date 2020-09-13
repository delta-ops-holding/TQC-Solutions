using RestAPI.Data.Interfaces;
using RestAPI.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data.Managers
{
    public class UserManager : IUserManager
    {
        public Task<bool> CheckPasswordAsync(User user, string userPassword)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByUserNameAsync(string userName, int userTagId)
        {
            throw new NotImplementedException();
        }
    }
}
