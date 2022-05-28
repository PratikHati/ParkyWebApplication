using ParkyAPI.Date;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _adb;

        public UserRepository(ApplicationDbContext adb)
        {
            _adb = adb;
        }
        public Users Authenticate(string name, string password)
        {
            throw new NotImplementedException();
        }

        public bool IsUserUnique(string name)
        {
            throw new NotImplementedException();
        }

        public Users Register(string name, string password)
        {
            throw new NotImplementedException();
        }
    }
}
