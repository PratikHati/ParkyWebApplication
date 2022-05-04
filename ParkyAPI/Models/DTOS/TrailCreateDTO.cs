using ParkyAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ParkyAPI.Models.Trail;

namespace ParkyAPI.Models.DTOS
{
    public class TrailCreateDTO
    {
        //no ID needed as DB will create themselve for it as ID should be unique primary key
        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }


        public DifficultyType Difficulty { get; set; }

        [Required]
        public double Elevation { get; set; }

        [Required]
        public int NationalParkId { get; set; }
    }
}
