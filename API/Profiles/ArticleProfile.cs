using API.DTOs;
using API.Models;
using AutoMapper;

namespace API.Profiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticlePostDTO, Article>()
    .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.FromBase64String(src.Image)));

            CreateMap<ArticlePutDTO, Article>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.FromBase64String(src.Image)));

            CreateMap<Article, ArticleOutputDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)));
        }
    }
}
