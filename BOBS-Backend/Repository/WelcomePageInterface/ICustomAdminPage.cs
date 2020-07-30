using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Model;
using BOBS_Backend.Models.Book;
using BOBS_Backend.Models.Order;
using BOBS_Backend.ViewModel.UpdateBooks;

namespace BOBS_Backend.Repository.WelcomePageInterface
{
    public interface ICustomAdminPage
    {
        Task<List<Price>> GetUserUpdatedBooks(string adminUsername);
        Task<List<Price>> OtherUpdatedBooks(string adminUsername);
        Task<List<FilterOrders>> GetImportantOrders();
        List<FilterOrders> SortTable(List<FilterOrders> order, string sortByValue);
    }
}
