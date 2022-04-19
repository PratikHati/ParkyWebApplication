using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class NationalParksController : Controller       //it will act as api controller
    {
        private INationalParkRepository _npr;
        private readonly IMapper _imap;
        public NationalParksController(INationalParkRepository npr, IMapper im)
        {
            _npr = npr;
            _imap = im;
        }
        public IActionResult Index()
        {
            return View();
        }

        //GET
        [HttpGet]
        public IActionResult GetNationalPark()
        {
            var objs = _npr.GetNationalParks();

            //but map to DTO
            var objdto = new List<NationalParkDTO>();

            foreach(var x in objs)
            {
                objdto.Add(_imap.Map<NationalParkDTO>(x));
            }

            return Ok(objdto);
        }
    }
}
