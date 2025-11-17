using System.ComponentModel;

namespace Bookstore.Domain.Offers
{
    public enum OfferStatus
    {
        [Description("Pending Approval")]
        PendingApproval = 0,

        [Description("Approved/Awaiting Shipment from Customer")]
        Approved = 1,

        [Description("Shipment Receipt Confirmed")]
        Received = 2,

        [Description("Payment Completed")]
        Paid = 3,

        [Description("Rejected")]
        Rejected = 4
    }
}