using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Notifications.NotificationsInterface
{
    public interface INotifications 
    {
    
        void SendOrderStatusUpdateEmail(string orderStatus,long orderId, string customerFirstName, string customerEmail);
        void SendItemRemovalEmail(string bookName, string bookCondition, string customerFirstName, string customerEmail);
    }
}
