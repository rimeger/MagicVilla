using AutoMapper;
using MagicVilla_WebApi.Models;
using MagicVilla_WebApi.Models.Dto;

namespace MagicVilla_WebApi
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>();
            CreateMap<VillaDTO, Villa>();

            CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();
        }
    }
}
