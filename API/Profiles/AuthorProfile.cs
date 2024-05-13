using API.DTOs;
using API.Models;
using AutoMapper;

namespace API.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<AuthorPostDTO, Author>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.FromBase64String(src.Image)));

            CreateMap<AuthorPutDTO, Author>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.FromBase64String(src.Image)));

            CreateMap<Author, AuthorOutputDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)));
        }
    }
}
