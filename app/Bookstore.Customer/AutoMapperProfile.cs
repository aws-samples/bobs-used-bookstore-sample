using AutoMapper;
using CustomerSite.Models.ViewModels;
using Bookstore.Domain.Offers;

namespace CustomerSite
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ResaleViewModel, Offer>();
        }
    }
}