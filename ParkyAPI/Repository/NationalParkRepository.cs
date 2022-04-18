using ParkyAPI.Date;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _adb;

        public NationalParkRepository(ApplicationDbContext adb)
        {
            _adb = adb;
        }
        public bool CreateNationalPark(NationalPark np)
        {
            _adb.NationalParks.Add(np);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark np)
        {
            _adb.NationalParks.Remove(np);
            return Save();  
        }

        public NationalPark GetNationalPark(int id)
        {
            return _adb.NationalParks.FirstOrDefault(x=>x.ID == id);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _adb.NationalParks.OrderBy(x => x.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            bool ans = _adb.NationalParks.Any(x=>x.Name.ToLower().Trim() == name.ToLower().Trim());
            return ans;
        }

        public bool NationalParkExists(int id)
        {
            bool ans = _adb.NationalParks.Any(x => x.ID == id);
            return ans;
        }

        public bool Save()
        {
            return _adb.SaveChanges() >= 0 ? true : false;  //shortcut to savechanges
        }

        public bool UpdateNationalPark(NationalPark np)
        {
            _adb.NationalParks.Update(np);
            return Save();
        }
    }
}
