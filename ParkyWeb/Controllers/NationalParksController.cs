using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _inpr;

        public NationalParksController(INationalParkRepository inpr)
        {
            _inpr = inpr;
        }
        public IActionResult Index()
        {
            return View(new NationalPark() { });    //not needed 
        }

        public async Task<IActionResult> Upsert(int? id)               //Update and create in same
        {
            NationalPark obj = new NationalPark();
                
            if(id== null)                   //insert or create
            {
                return View(obj);
            }

            //update case
            obj = await _inpr.GetAsync(SD.NationalParkApiUrl,id.GetValueOrDefault());

            if(obj == null) //not updated as not in api db
            {
                return NotFound();
            }

            return View(obj);    //return updated object    
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            return Json(new { data = await _inpr.GetAllAsync(SD.NationalParkApiUrl)});  //will retreive all NP objects from ParkyAPI through "Repository"
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark np)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files; //retrive file info from frontend page

                if(files.Count > 0)
                {
                    byte[] pic = null;      //create byte[]
                    
                    using (var fin = files[0].OpenReadStream())     //retrive file info
                    {
                        using (var cin = new MemoryStream())        //create memorystream to FileStream->MemorySteam->DB
                        {
                            fin.CopyTo(cin);        //file to memory
                                    
                            pic = cin.ToArray();    //momory to array
                        }
                    }
                    np.Picture = pic;       //array to DB
                }
                else
                {
                    var objfromdb = await _inpr.GetAsync(SD.NationalParkApiUrl,np.ID);  //retrive entire NationalPark object from db

                    np.Picture = objfromdb.Picture;
                }

                if(np.ID == 0)  //create case
                {
                    await _inpr.CreateAsync(SD.NationalParkApiUrl,np);      //create new object
                }
                else           //update case
                {
                    await _inpr.UpdateAsync(SD.NationalParkApiUrl+np.ID, np);      //update new object
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
            bool status = await _inpr.DeleteAsync(SD.NationalParkApiUrl,id);

            if (status)
            {
                return Json(new { success = true, message = "deleted successfully" }) ;
            }
            return Json(new { success = false, message = "deleted unsuccessfully" });
        }

    }
}
