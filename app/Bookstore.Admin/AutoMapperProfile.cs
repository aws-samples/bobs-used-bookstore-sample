using AutoMapper;
using DataAccess.Dtos;
using AdminSite.ViewModel;
using AdminSite.ViewModel.ManageInventory;
using AdminSite.ViewModel.ManageOrders;
using AdminSite.ViewModel.ResaleBooks;
using AdminSite.ViewModel.SearchBooks;
using Bookstore.Domain.Books;
using AdminSite.ViewModel.Inventory;
using System.Collections.Generic;
using System.Linq;
using Amazon.SimpleEmailV2.Model;
using Microsoft.Build.Framework;
using Bookstore.Admin.ViewModel.Inventory;
using Bookstore.Domain.ReferenceData;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminSite
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BookDetailsDto, BookDetailsViewModel>();
            CreateMap<BookDetailsDto, InventoryCreateUpdateViewModel>()
                .ForMember(dest => dest.SelectedGenreId, opt => opt.MapFrom(src => src.Genre.Name))
                .ForMember(dest => dest.SelectedBookTypeId, opt => opt.MapFrom(src => src.BookType.TypeName))
                .ForMember(dest => dest.SelectedConditionId, opt => opt.MapFrom(src => src.BookCondition.ConditionName));
            CreateMap<ManageOrderDto, ManageOrderViewModel>();
            CreateMap<InventoryCreateUpdateViewModel, BooksDto>();
            CreateMap<InventoryCreateUpdateViewModel, Book>();
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

            ///////////

            CreateMap<Book, InventoryIndexViewModel>()
                .ForMember(dest => dest.BookType, opt => opt.MapFrom(src => src.BookType.Text))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Text))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher.Text))
                .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Condition.Text));

            CreateMap<Book, InventoryDetailsViewModel>()
                .ForMember(dest => dest.BookType, opt => opt.MapFrom(src => src.BookType.Text))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Text))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher.Text));

            CreateMap<InventoryCreateUpdateViewModel, Book>()
                .ForMember(dest => dest.BookTypeId, opt => opt.MapFrom(src => src.SelectedBookTypeId))
                .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.SelectedGenreId))
                .ForMember(dest => dest.ConditionId, opt => opt.MapFrom(src => src.SelectedConditionId))
                .ForMember(dest => dest.PublisherId, opt => opt.MapFrom(src => src.SelectedPublisherId))
                .ReverseMap();

            CreateMap<IEnumerable<ReferenceDataItem>, InventoryCreateUpdateViewModel>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Where(x => x.DataType == ReferenceDataType.Genre).Select(x => new SelectListItem(x.Text, x.Id.ToString()))))
                .ForMember(dest => dest.Publishers, opt => opt.MapFrom(src => src.Where(x => x.DataType == ReferenceDataType.Publisher).Select(x => new SelectListItem(x.Text, x.Id.ToString()))))
                .ForMember(dest => dest.BookConditions, opt => opt.MapFrom(src => src.Where(x => x.DataType == ReferenceDataType.Condition).Select(x => new SelectListItem(x.Text, x.Id.ToString()))))
                .ForMember(dest => dest.BookTypes, opt => opt.MapFrom(src => src.Where(x => x.DataType == ReferenceDataType.BookType).Select(x => new SelectListItem(x.Text, x.Id.ToString()))));

        }
    }
}