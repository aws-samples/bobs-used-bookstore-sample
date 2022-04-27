using AutoMapper;
using BobsBookstore.DataAccess.Dtos;
using BobsBookstore.Models.Books;
using BookstoreBackend.ViewModel;
using BookstoreBackend.ViewModel.ManageInventory;
using BookstoreBackend.ViewModel.ManageOrders;
using BookstoreBackend.ViewModel.ResaleBooks;
using BookstoreBackend.ViewModel.SearchBooks;

namespace BookstoreBackend
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BookDetailsDto, BookDetailsViewModel>();
            CreateMap<ManageOrderDto, ManageOrderViewModel>();
            CreateMap<BooksViewModel, BooksDto>();
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