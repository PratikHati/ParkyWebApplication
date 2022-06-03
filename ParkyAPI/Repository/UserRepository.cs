using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyAPI.Date;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _adb;
        private readonly AppSettings _secretKey;
        public UserRepository(ApplicationDbContext adb, IOptions<AppSettings> secretKey)
        {
            _adb = adb;
            _secretKey = secretKey.Value;
        }
        public Users Authenticate(string name, string password)
        {
            var user = _adb.Users.SingleOrDefault(x => x.Password == password && x.UserName == name);

            //user not found
            if(user == null)
            {
                return null;
            }

            //generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey.Secret);   //get the key
            //token attributes
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = tokenHandler.CreateToken(tokenDesc);//at last generate token

            user.Token = tokenHandler.WriteToken(token);//assign token

            return user;        //return user object
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
