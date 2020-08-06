using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.ViewModels
{
    public class OrderDetailViewModel
    {
        public string Bookname { get; set; }
        public long Book_Id { get; set; }
        public int quantity { get; set; }
        public string Url { get; set; }
        public double  price { get; set; }
    }
}
