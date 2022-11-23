using AutoMapper;
using DataAccess.Dtos;
using AdminSite.ViewModel;
using AdminSite.ViewModel.ResaleBooks;
using AdminSite.ViewModel.SearchBooks;
using Bookstore.Domain.Books;

namespace AdminSite
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Resale, ResaleViewModel>();
            CreateMap<ResaleViewModel, BooksDto>()
                .ForMember(dest => dest.FrontPhoto, opt => opt.MapFrom(src => src.FrontUrl))
                .ForMember(dest => dest.BackPhoto, opt => opt.MapFrom(src => src.BackUrl))
                .ForMember(dest => dest.LeftSidePhoto, opt => opt.MapFrom(src => src.LeftUrl))
                .ForMember(dest => dest.RightSidePhoto, opt => opt.MapFrom(src => src.RightUrl))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.GenreName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.BookPrice))
                .ForMember(dest => dest.BookCondition, opt => opt.MapFrom(src => src.ConditionName))
                .ForMember(dest => dest.BookType, opt => opt.MapFrom(src => src.TypeName));
        }
    }
}