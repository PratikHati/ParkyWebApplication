using Microsoft.EntityFrameworkCore;
using ParkyAPI.Date;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _adb;

        public TrailRepository(ApplicationDbContext adb)
        {
            _adb = adb;
        }
        public bool CreateTrail(Trail np)
        {
            _adb.Trails.Add(np);
            return Save();
        }

        public bool DeleteTrail(Trail np)
        {
            _adb.Trails.Remove(np);
            return Save();  
        }

        public Trail GetTrail(int id)
        {
            return _adb.Trails.Include(x => x.NationalPark).FirstOrDefault(x=>x.Id == id);
        }

        public ICollection<Trail> GetTrails()
        {
            return _adb.Trails.Include(x => x.NationalPark).OrderBy(x => x.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            bool ans = _adb.Trails.Any(x=>x.Name.ToLower().Trim() == name.ToLower().Trim());
            return ans;
        }

        public bool TrailExists(int id)
        {
            bool ans = _adb.Trails.Any(x => x.Id == id);
            return ans;
        }

        public bool Save()
        {
            return _adb.SaveChanges() >= 0 ? true : false;  //shortcut to savechanges
        }

        public bool UpdateTrail(Trail np)
        {
            _adb.Trails.Update(np);
            return Save();
        }

        public ICollection<Trail> GetTrailsNationalPark(int id)
        {
            return _adb.Trails.Include(x => x.NationalPark).Where(x => x.NationalParkId == id).ToList();    //Eager loading
        }
    }
}
