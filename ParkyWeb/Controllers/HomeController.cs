using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITrailRepository _trailRepo;
        private readonly INationalParkRepository _nationalparkRepo;


        public HomeController(ILogger<HomeController> logger, INationalParkRepository national, ITrailRepository trail)
        {
            _logger = logger;
            _nationalparkRepo = national;
            _trailRepo = trail;
        }

        public async Task<IActionResult> Index()
        {
            //retrive nationalpark and trails from db
            IndexVM idxvm = new IndexVM()
            {
                NationalParkList = await _nationalparkRepo.GetAllAsync(SD.NationalParkApiUrl),
                TrailList = await _trailRepo.GetAllAsync(SD.TrailApiUrl)
            };
            return View(idxvm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
