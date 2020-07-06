using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.ViewModels
{
    public class OrderViewModel
    {
        public double Subtotal { get; set; }
        public double Tax { get; set; }
        public string DeliveryDate { get; set; }
        public string Status { get; set; }
    }
}
