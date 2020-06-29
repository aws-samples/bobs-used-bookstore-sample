using BOBS_Backend.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository
{
    public interface IOrderDetailRepository
    {

        Task<OrderDetail> FindOrderDetailById(long id);

        Task<List<OrderDetail>> FindOrderDetailByOrderId(long orderId);
    }
}
