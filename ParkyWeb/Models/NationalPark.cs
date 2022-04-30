using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Models
{
    public class NationalPark    //to display data by binding them to this model. DbMigration not needed as data will come from ParkyAPI
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string State { get; set; }
        public DateTime Created { get; set; }
        public DateTime Established { get; set; }
        public byte[] Picture { get; set; }
    }
}
