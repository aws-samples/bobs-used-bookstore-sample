using System.Collections.Generic;
using System.Threading.Tasks;
using BobsBookstore.DataAccess.Dtos;
using BobsBookstore.Models.Books;

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