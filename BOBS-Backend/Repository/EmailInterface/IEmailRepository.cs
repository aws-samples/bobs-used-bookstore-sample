using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository.EmailInterface
{
    public interface IEmailRepository 
    {

        void SendOrderStatusUpdateEmail(string orderStatus,long orderId, string customerFirstName, string customerEmail);
    }
}
