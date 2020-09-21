using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BOBS_Backend.Models.Book;
using BOBS_Backend.Models.Order;

namespace BOBS_Backend.ViewModel.UpdateBooks
{
    public class FilterOrders
    {

       
        public Order Order { get; set; }
        public int Severity { get; set; }
    }
}
