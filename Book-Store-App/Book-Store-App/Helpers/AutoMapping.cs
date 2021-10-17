using AutoMapper;
using Book_Store_App.Data;
using Book_Store_App.Models;

namespace Book_Store_App.Helpers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<GalleryModel, Gallery>()
                .ReverseMap();

            CreateMap<BookModel, Book>()
                .ForMember(dest => dest.Gallery, src => src.MapFrom(d => d.Gallery))
                .ReverseMap();

            CreateMap<LanguageModel, Language>()
                .ReverseMap();

            CreateMap<SignUpModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(s => s.Email))
                .ReverseMap();
        }
    }
}