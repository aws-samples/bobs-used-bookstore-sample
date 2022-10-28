using AutoMapper;
using CustomerSite.Models.ViewModels;
using Bookstore.Domain.Books;

namespace CustomerSite
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ResaleViewModel, Resale>();
        }
    }
}