using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOS;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    //[Route("api/Trails")]
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrails")]
    [ProducesResponseType(400)]

    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpec")]

    public class TrailsController : Controller       //it will act as api controller
    {
        private ITrailRepository _tpr;
        private readonly IMapper _imap;
        public TrailsController(ITrailRepository tpr, IMapper im)
        {
            _tpr = tpr;
            _imap = im;
        }

        /// <summary>
        /// 
        /// GET- List of all Trails
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDTO>))]    //successful
        public IActionResult GetTrails()
        {
            var objs = _tpr.GetTrails();

            //but map to DTO
            var objdto = new List<TrailDTO>();

            foreach (var x in objs)
            {
                objdto.Add(_imap.Map<TrailDTO>(x));
            }

            return Ok(objdto);
        }

        /// <summary>
        /// 
        /// GET- Only Particulr Trail
        /// 
        /// </summary>
        /// <param name="id">INT id </param>
        /// <returns></returns>

        [HttpGet("{id:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDTO))]    //successful
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]                           //if none of above returned
        public IActionResult GetTrail(int id)
        {
            var obj = _tpr.GetTrail(id);

            if (obj == null)
            {
                return NotFound();
            }

            //but map to DTO
            var objdto = new TrailDTO();

            objdto = _imap.Map<TrailDTO>(obj);

            return Ok(objdto);
        }

        /// B<summary>
        /// 
        /// POST- Create Trail 
        /// 
        /// </summary>
        /// <param name="ndto"> TrailDTO ndto</param>
        /// <returns></returns>

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDTO))]    //successful
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesDefaultResponseType]
        public IActionResult CreateTrail([FromBody] TrailCreateDTO ndto)
        {
            if (ndto == null)
            {
                return BadRequest(ModelState);      //modelstate contains all error info
            }

            if (_tpr.TrailExists(ndto.Name))
            {
                ModelState.AddModelError("", "Trail Exists Already");

                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var npobj = _imap.Map<Trail>(ndto);  //DTO to normal

            if (!_tpr.CreateTrail(npobj))
            {
                ModelState.AddModelError("", $"Something went wrong During Creation For {npobj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { id = npobj.Id }, npobj);  //imprtant, it will return 201 not 200 Ok
        }

        /// <summary>
        /// 
        /// PATCH -   Update Trail
        /// 
        /// </summary>
        /// <param name="id"> INT </param>
        /// <param name="npdto"> TrailDTO </param>
        /// <returns></returns>

        [HttpPatch("{id:int}", Name = "UpdateTrail")] //when ever we want to modify "NationalFlag"
        [ProducesResponseType(204)]    //successful
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateTrail(int id, [FromBody] TrailUpdateDTO npdto)
        {   
            if (npdto == null || id != npdto.Id)
            {
                return BadRequest(ModelState);
            }
             
            var npobj = _imap.Map<Trail>(npdto);

            if (!_tpr.UpdateTrail(npobj))
            {
                //backend error
                ModelState.AddModelError("", $"Something went wrong During Updation For {npobj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// 
        /// DELETE - Delete Trail
        /// 
        /// </summary>
        /// <param name="id"> INT </param>
        /// <returns></returns>

        [HttpDelete("{id:int}", Name = "DeleteTrail")] //when ever we want to modify "NationalFlag"
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeleteTrail(int id)
        {
            if (!_tpr.TrailExists(id))
            {
                return NotFound(ModelState);
            }

            var nationalpark = _tpr.GetTrail(id);

            if (!_tpr.DeleteTrail(nationalpark))
            {
                //backend error
                ModelState.AddModelError("", $"Something went wrong During Deletion For {nationalpark.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
