using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
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



    }
}
