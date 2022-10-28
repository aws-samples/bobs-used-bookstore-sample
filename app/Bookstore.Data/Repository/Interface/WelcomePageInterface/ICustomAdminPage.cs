using System.Collections.Generic;
using System.Threading.Tasks;
using Bookstore.Domain.Books;

namespace Bookstore.Data.Repository.Interface.WelcomePageInterface
{
    public interface ICustomAdminPage
    {
        Task<List<Price>> GetUserUpdatedBooksAsync(string adminUsername);

        Task<List<Price>> OtherUpdatedBooksAsync(string adminUsername);

        //Task<List<FilterOrdersDto>> GetImportantOrdersAsync(int maxRange, int minRange);

        //List<FilterOrdersDto> SortTable(List<FilterOrdersDto> order, string sortByValue);
    }
}