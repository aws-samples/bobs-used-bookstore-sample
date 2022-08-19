using System.Collections.Generic;

namespace DataAccess.Repository.Interface.NotificationsInterface
{
    public interface INotifications
    {
        void SendOrderStatusUpdateEmail(string orderStatus, long orderId, string customerFirstName, string customerEmail);

        void SendItemRemovalEmail(string bookName, string bookCondition, string customerFirstName, string customerEmail);

        void SendInventoryLowEmail(List<string> bookList, string customerEmail);
    }
}
