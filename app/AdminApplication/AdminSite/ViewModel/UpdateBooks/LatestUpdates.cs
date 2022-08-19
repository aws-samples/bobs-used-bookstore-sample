using System.Collections.Generic;
using DataAccess.Dtos;
using DataModels.Books;

namespace AdminSite.ViewModel.UpdateBooks
{
    public class LatestUpdates
    {
        public List<Price> UserBooks { get; set; }

        public List<Price> NotUserBooks { get; set; }

        public List<FilterOrdersDto> ImpOrders { get; set; }
    }
}