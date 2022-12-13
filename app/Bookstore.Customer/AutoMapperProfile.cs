using AutoMapper;
using Bookstore.Domain.Offers;
using Bookstore.Customer.ViewModel;

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