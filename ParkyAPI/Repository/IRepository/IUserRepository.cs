using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUserUnique(string name);

        Users Authenticate(string name,string password);

        Users Register(string name, string password);
    }
}
