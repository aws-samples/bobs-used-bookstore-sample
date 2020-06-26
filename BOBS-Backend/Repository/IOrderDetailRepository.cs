using BOBS_Backend.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository
{
    public interface IOrderDetailRepository
    {

        OrderDetail FindOrderDetailById(long id);

        IEnumerable<OrderDetail> FindOrderDetailByOrderId(long orderId);
    }
}
