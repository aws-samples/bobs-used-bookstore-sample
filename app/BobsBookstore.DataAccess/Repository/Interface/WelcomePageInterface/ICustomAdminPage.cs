using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Model;
using BobsBookstore.DataAccess.Dtos;
using BobsBookstore.Models.Books;
using BobsBookstore.Models.Orders;

namespace BobsBookstore.DataAccess.Repository.Interface.WelcomePageInterface
{
    public interface ICustomAdminPage
    {
        Task<List<Price>> GetUserUpdatedBooks(string adminUsername);
        Task<List<Price>> OtherUpdatedBooks(string adminUsername);
        Task<List<FilterOrdersDto>> GetImportantOrders(int maxRange, int minRange);
        List<FilterOrdersDto> SortTable(List<FilterOrdersDto> order, string sortByValue);
    }
}
