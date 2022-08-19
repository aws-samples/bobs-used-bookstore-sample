using AutoMapper;
using DataModels.Books;
using CustomerSite.Models.ViewModels;

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