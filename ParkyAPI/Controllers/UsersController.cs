using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/Users")]   
    [ApiController]
   
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _irp;

        public UsersController(IUserRepository irp)
        {
            _irp = irp;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] Users usr)
        {
            var user = _irp.Authenticate(usr.UserName,usr.Password);
            if(user == null)
            {
                return BadRequest(new { message = "username or password is wrong"});
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] Users user)
        {
            bool isUniqueUser = _irp.IsUserUnique(user.UserName);

            if (isUniqueUser)
            {
                var newuser = _irp.Register(user.UserName,user.Password);

                if(newuser == null)
                {
                    return BadRequest(new { message = "unable to register!" });
                }
                else
                {
                    return Ok();
                }
            }
            else
            {
                return BadRequest(new { message = "user alreday exists!"});
            }
        }

    }
}
