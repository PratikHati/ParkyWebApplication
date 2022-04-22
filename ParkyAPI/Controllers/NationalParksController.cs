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

            foreach (var x in objs)
            {
                objdto.Add(_imap.Map<NationalParkDTO>(x));
            }

            return Ok(objdto);
        }

        [HttpGet("{id:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int id)
        {
            var obj = _npr.GetNationalPark(id);

            if (obj == null)
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
            if (ndto == null)
            {
                return BadRequest(ModelState);      //modelstate contains all error info
            }

            if (_npr.NationalParkExists(ndto.Name))
            {
                ModelState.AddModelError("", "National Park Exists Already");

                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var npobj = _imap.Map<NationalPark>(ndto);  //DTO to normal

            if (!_npr.CreateNationalPark(npobj))
            {
                ModelState.AddModelError("", $"Something went wrong During Creation For {npobj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { id = npobj.ID }, npobj);  //imprtant, it will return 201 not 200 Ok
        }

        [HttpPatch("{id:int}",Name = "UpdateNationalFlag")] //when ever we want to modify "NationalFlag"
        public IActionResult UpdateNationalFlag(int id, [FromBody]NationalParkDTO npdto)
        {
            if(npdto == null || id != npdto.ID)
            {
                return BadRequest(ModelState);
            }

            var npobj = _imap.Map<NationalPark>(npdto);

            if (!_npr.UpdateNationalPark(npobj))
            {
                //backend error
                ModelState.AddModelError("", $"Something went wrong During Updation For {npobj.Name}");
                return StatusCode(500,ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteNationalFlag")] //when ever we want to modify "NationalFlag"
        public IActionResult DeleteNationalFlag(int id)
        {
            if (!_npr.NationalParkExists(id))
            {
                return NotFound(ModelState);
            }

            var nationalpark = _npr.GetNationalPark(id);

            if (!_npr.DeleteNationalPark(nationalpark))
            {
                //backend error
                ModelState.AddModelError("", $"Something went wrong During Deletion For {nationalpark.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
