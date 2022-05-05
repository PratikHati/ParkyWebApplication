using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _inpr;
        private readonly ITrailRepository _itr;


        public TrailsController(INationalParkRepository inpr, ITrailRepository itr)
        {
            _inpr = inpr;
            _itr = itr;
        }
        public IActionResult Index()
        {
            return View(new Trail() { });    //not needed 
        }

        public async Task<IActionResult> Upsert(int? id)               //Update and create in same
        {
            IEnumerable<NationalPark> objlist = await _inpr.GetAllAsync(SD.NationalParkApiUrl);

            TrailsVM trailvm = new TrailsVM()
            {
                NationalParkList = objlist.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    //for DROPDOWN 
                    Text = x.Name,
                    Value = x.ID.ToString()
                }),
                //Trail will be filled later
                Trail = new Trail()     //create a dummy instance (not null)
            };
                
            if(id== null)                   //insert or create
            {
                return View(trailvm);
            }

            //update case
            trailvm.Trail = await _itr.GetAsync(SD.TrailApiUrl,id.GetValueOrDefault());

            if(trailvm.Trail == null)   //not updated as not in api db
            {
                return NotFound();
            }

            return View(trailvm);    //return updated object    
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _itr.GetAllAsync(SD.TrailApiUrl)});  //will retreive all NP objects from ParkyAPI through "Repository"
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM np)
        {
            if (ModelState.IsValid)
            {
                if(np.Trail.Id == 0)    //create
                {
                    await _itr.CreateAsync(SD.TrailApiUrl,np.Trail);
                }   
                else                    //update
                {
                    await _itr.UpdateAsync(SD.TrailApiUrl+np.Trail.Id, np.Trail);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(np);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            bool status = await _itr.DeleteAsync(SD.TrailApiUrl,id);

            if (status)
            {
                return Json(new { success = true, message = "deleted successfully" }) ;
            }
            return Json(new { success = false, message = "deleted unsuccessfully" });
        }

    }
}
