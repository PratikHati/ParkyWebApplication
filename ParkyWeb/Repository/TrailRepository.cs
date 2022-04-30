using ParkyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ParkyWeb.Repository.IRepository;
using System.Threading.Tasks;
using System.Net.Http;

namespace ParkyWeb.Repository
{
    public class TrailRepository: Repository<Trail>, ITrailRepository
    {
        private readonly IHttpClientFactory _icf;
        public TrailRepository(IHttpClientFactory icf): base(icf)
        {
            _icf = icf;
        }
    }
}
