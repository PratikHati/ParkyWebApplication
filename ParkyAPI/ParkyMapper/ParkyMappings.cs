using AutoMapper;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Models.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.ParkyMapper
{
    public class ParkyMappings : Profile    //Profile -> AutoMapper
    {
        public ParkyMappings()
        {
            CreateMap<NationalPark, NationalParkDTO>().ReverseMap();

            CreateMap<Trail, TrailCreateDTO>().ReverseMap();
            
            CreateMap<Trail, TrailUpdateDTO>().ReverseMap();

            CreateMap<Trail, TrailDTO>().ReverseMap();
        }
    }
}
