using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Model;
using BobsBookstore.Models.Books;
using BobsBookstore.Models.Orders;
using BookstoreBackend.ViewModel.UpdateBooks;

namespace BookstoreBackend.Repository.WelcomePageInterface
{
    public interface ICustomAdminPage
    {
        Task<List<Price>> GetUserUpdatedBooks(string adminUsername);
        Task<List<Price>> OtherUpdatedBooks(string adminUsername);
        Task<List<FilterOrders>> GetImportantOrders(int maxRange, int minRange);
        List<FilterOrders> SortTable(List<FilterOrders> order, string sortByValue);
    }
}
