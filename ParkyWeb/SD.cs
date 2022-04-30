using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb
{
    public static class SD
    {
        public static string APIBaseUrl = "https://localhost:44312/";

        public static string NationalParkApiUrl =APIBaseUrl + "api/v1/nationalparks";

        public static string TrailApiUrl = APIBaseUrl + "api/v1/trails";
    }
}
