using AutoMapper;
using BobsBookstore.Models.Books;
using BookstoreFrontend.Models.ViewModels;

namespace BookstoreFrontend
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ResaleViewModel, Resale>();
        }
    }
}