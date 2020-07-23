using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOBS_Backend.Models.Book;
using BOBS_Backend.Models.Order;
using BOBS_Backend.ViewModel.UpdateBooks;

namespace BOBS_Backend.Repository.WelcomePageInterface
{
    public interface ICustomAdminPage
    {
        Task<List<Price>> GetUpdatedBooks(string adminUsername);
        Task<List<Price>> GetGlobalUpdatedBooks(string adminUsername);
        Task<List<Order>> GetImportantOrders();
        List<Order> SortTable(List<Order> order, string sortByValue);
    }
}
