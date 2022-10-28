using AutoMapper;
using DataAccess.Dtos;
using AdminSite.ViewModel;
using AdminSite.ViewModel.ManageInventory;
using AdminSite.ViewModel.ManageOrders;
using AdminSite.ViewModel.ResaleBooks;
using AdminSite.ViewModel.SearchBooks;
using Bookstore.Domain.Books;

namespace AdminSite
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BookDetailsDto, BookDetailsViewModel>();
            CreateMap<BookDetailsDto, BooksViewModel>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
                .ForMember(dest => dest.BookType, opt => opt.MapFrom(src => src.BookType.TypeName))
                .ForMember(dest => dest.BookCondition, opt => opt.MapFrom(src => src.BookCondition.ConditionName));
            CreateMap<ManageOrderDto, ManageOrderViewModel>();
            CreateMap<BooksViewModel, BooksDto>();
            CreateMap<BooksViewModel, Book>();
            CreateMap<BookDetailsViewModel, BookDetailsDto>();
            CreateMap<FullBookDto, FullBook>();
            CreateMap<SearchBookDto, SearchBookViewModel>();
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