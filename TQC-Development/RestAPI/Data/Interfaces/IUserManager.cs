using RestAPI.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data.Interfaces
{
    public interface IUserManager
    {
        Task<User> FindByUserNameAsync(string userName, int userTagId);
        Task<bool> CheckPasswordAsync(User user, string userPassword);
    }
}
