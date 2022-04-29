using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{

    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]

    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(400)]

    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpec")]

    public class NationalParksV2Controller : Controller       //it will act as api controller
    {
        private INationalParkRepository _npr;
        private readonly IMapper _imap;
        public NationalParksV2Controller(INationalParkRepository npr, IMapper im)
        {
            _npr = npr;
            _imap = im;
        }

        /// <summary>
        /// 
        /// GET- List of all NationalParks
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDTO>))]    //successful
        public IActionResult GetNationalParks()
        {
            var objs = _npr.GetNationalParks().FirstOrDefault();


            return Ok(_imap.Map<NationalParkDTO>(objs));
        }

    }
}
