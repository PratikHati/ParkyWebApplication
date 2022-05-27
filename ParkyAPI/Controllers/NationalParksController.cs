using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(400)]

    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpec")]
    //change to ControllerBase as this controller don't have any views
    public class NationalParksController : ControllerBase       //it will act as api controller
    {
        private INationalParkRepository _npr;
        private readonly IMapper _imap;
        public NationalParksController(INationalParkRepository npr, IMapper im)
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
            var objs = _npr.GetNationalParks();

            //but map to DTO
            var objdto = new List<NationalParkDTO>();

            foreach (var x in objs)
            {
                objdto.Add(_imap.Map<NationalParkDTO>(x));
            }

            return Ok(objdto);
        }

        /// <summary>
        /// 
        /// GET- Only Particulr NationalPark
        /// 
        /// </summary>
        /// <param name="id">INT id </param>
        /// <returns></returns>

        [HttpGet("{id:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDTO))]    //successful
        [ProducesResponseType(404)]
        [Authorize]
        [ProducesDefaultResponseType]                           //if none of above returned
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

        /// <summary>
        /// 
        /// POST- Create NationalPark
        /// 
        /// </summary>
        /// <param name="ndto"> NationalParkDTO ndto</param>
        /// <returns></returns>

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDTO))]    //successful
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesDefaultResponseType]
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

            var npobj = _imap.Map<NationalPark>(ndto);  //DTO to normals

            if (!_npr.CreateNationalPark(npobj))
            {
                ModelState.AddModelError("", $"Something went wrong During Creation For {npobj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { version = HttpContext.GetRequestedApiVersion().ToString() ,
                                                            id = npobj.ID }, npobj);  //imprtant, it will return 201 not 200 Ok
        }

        /// <summary>
        /// 
        /// PATCH -   Update NationalPark
        /// 
        /// </summary>
        /// <param name="id"> INT </param>
        /// <param name="npdto"> NationalParkDTO </param>
        /// <returns></returns>

        [HttpPatch("{id:int}", Name = "UpdateNationalPark")] //when ever we want to modify "NationalPark"
        [ProducesResponseType(204)]    //successful
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateNationalPark(int id, [FromBody] NationalParkDTO npdto)
        {
            if (npdto == null || id != npdto.ID)
            {
                return BadRequest(ModelState);
            }

            var npobj = _imap.Map<NationalPark>(npdto);

            if (!_npr.UpdateNationalPark(npobj))
            {
                //backend error
                ModelState.AddModelError("", $"Something went wrong During Updation For {npobj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// 
        /// DELETE - Delete NationalPark
        /// 
        /// </summary>
        /// <param name="id"> INT </param>
        /// <returns></returns>

        [HttpDelete("{id:int}", Name = "DeleteNationalPark")] //when ever we want to modify "NationalFlag"
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeleteNationalPark(int id)
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
