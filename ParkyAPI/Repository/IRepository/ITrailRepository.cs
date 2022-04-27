using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository.IRepository
{
    public interface ITrailRepository
    {
        ICollection<Trail> GetTrails();

        ICollection<Trail>  GetTrailsNationalPark(int id);

        Trail GetTrail(int id);

        bool TrailExists(string name);

        bool TrailExists(int id);

        bool CreateTrail(Trail np);
        bool UpdateTrail(Trail np);
        bool DeleteTrail(Trail np);

        bool Save();
    }
}
