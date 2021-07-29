using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BobsBookstore.DataAccess.Dtos;
using BookstoreBackend.ViewModel.ManageInventory;
using BookstoreBackend.ViewModel.ManageOrders;
using BookstoreBackend.ViewModel;
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
        }


    }
}
