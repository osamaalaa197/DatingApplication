using AutoMapper;
using DatingApplication.Core.DTOs;
using DatingApplication.Core.Models;

namespace DatingApplication.API.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser,RegisterDTO>().ReverseMap();
            CreateMap<ApplicationUser,AccountDTO>()
                .ForMember(des=>des.PhotoUrl,opt=>opt.MapFrom(src=>src.Photos.FirstOrDefault(e=>e.IsMain)!.URL)).ReverseMap();
            CreateMap<Photo, PhotoDto>().ReverseMap();

        }
    }
}
