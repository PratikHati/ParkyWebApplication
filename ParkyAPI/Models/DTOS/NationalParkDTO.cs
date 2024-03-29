﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Models.DTOs
{
    public class NationalParkDTO       //DATA model
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
