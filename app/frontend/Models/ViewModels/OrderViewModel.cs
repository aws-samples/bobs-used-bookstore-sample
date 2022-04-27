using System.Collections.Generic;

namespace BobsBookstore.Models.ViewModels
{
    public class OrderViewModel
    {
        public long Order_Id { get; set; }
        public string Status { get; set; }
        public double Subtotal { get; set; }
        public double Tax { get; set; }
        public string DeliveryDate { get; set; }
        public List<OrderDetailViewModel> Books { get; set; }
        public string Url { get; set; }
    }
}