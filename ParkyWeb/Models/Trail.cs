using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Models
{
    public class Trail  //to display data by binding them to this model. DbMigration not needed as data will come from ParkyAPI
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        public enum DifficultyType { Easy, Moderate, Difficult, Expert }

        public DifficultyType Difficulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        public NationalPark NationalPark { get; set; }
    }
}
