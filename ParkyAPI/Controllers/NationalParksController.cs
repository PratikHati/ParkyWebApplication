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
        public IActionResult GetNationalParks()
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

        [HttpGet("{id:int}")]
        public IActionResult GetNationalPark(int id)
        {
            var obj = _npr.GetNationalPark(id);

            if(obj == null)
            {
                return NotFound();
            }

            //but map to DTO
            var objdto = new NationalParkDTO();

            objdto = _imap.Map<NationalParkDTO>(obj);

            return Ok(objdto);
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDTO ndto)
        {
            if(ndto == null)
            {
                return BadRequest(ModelState);      //modelstate contains all error info
            }

            if (_npr.NationalParkExists(ndto.Name))
            {
                ModelState.AddModelError("", "National Park Exists Already");

                return StatusCode(404,ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var npobj = _imap.Map<NationalPark>(ndto);  //DTO to normal

            if (!_npr.CreateNationalPark(npobj))
            {
                ModelState.AddModelError("",$"Something went wrong During Creation For {npobj.Name}");
                return StatusCode(500,ModelState);
            }

            return Ok();
        }
    }
}
