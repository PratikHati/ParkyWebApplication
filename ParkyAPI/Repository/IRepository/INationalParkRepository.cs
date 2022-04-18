using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository.IRepository
{
    public interface INationalParkRepository
    {
        ICollection<NationalPark> GetNationalParks();

        NationalPark GetNationalPark(int id);

        bool NationalParkExists(string name);

        bool NationalParkExists(int id);

        bool CreateNationalPark(NationalPark np);
        bool UpdateNationalPark(NationalPark np);
        bool DeleteNationalPark(NationalPark np);

        bool Save();
    }
}
