using System;
using System.Collections.Generic;
using System.Text;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using BookstoreBackend.Notifications.NotificationsInterface;
using Microsoft.Extensions.Configuration;

namespace BookstoreBackend.Notifications.Implementations
{
    public class Notifications : INotifications
    {
        private readonly IConfiguration Configuration;

        public Notifications(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void SendOrderStatusUpdateEmail(string orderStatus, long orderId, string customerFirstName, string customerEmail)
        {
            var from = Configuration.GetConnectionString("FromEmailAddress");
            var to = string.IsNullOrEmpty(Configuration.GetConnectionString("ToEmailAddressDefault"))
                ? customerEmail
                : Configuration.GetConnectionString("ToEmailAddressDefault");
            var subject = "BobsBookstore - Order Status Update";

            var body =
                $"<h3>Hello {customerFirstName}</h3><p>The status for your order: {orderId} has changed to {orderStatus}<p>";

            GenerateMessagePayload(from, to, subject, body);
        }

        public void SendItemRemovalEmail(string bookName, string bookCondition, string customerFirstName, string customerEmail)
        {
            var from = Configuration.GetConnectionString("FromEmailAddress");
            var to = string.IsNullOrEmpty(Configuration.GetConnectionString("ToEmailAddressDefault"))
                ? customerEmail
                : Configuration.GetConnectionString("ToEmailAddressDefault");
            var subject = "BobsBookstore - Out of stock notification";

            var body =
                $"<h3>Hello {customerFirstName}</h3><p>We are sorry to inform you that we dont have enough copies of {bookName} in the requested condition of {bookCondition}. A refund of your purchase will follow shorly.<p>";

            GenerateMessagePayload(from, to, subject, body);
        }

        public void SendInventoryLowEmail(List<string> bookList, string customerEmail)
        {
            var from = Configuration.GetConnectionString("FromEmailAddress");
            var to = string.IsNullOrEmpty(Configuration.GetConnectionString("ToEmailAddressDefault"))
                ? customerEmail
                : Configuration.GetConnectionString("ToEmailAddressDefault");
            var subject = "Inventory Stock Running Low";

            var books = new StringBuilder();
            foreach (var i in bookList) books.Append($"{i}\n");

            var body =
                $"<h3>Hello {customerEmail}</h3><p>This is to inform you that the inventory for the following books is running low.</p><p>{books}</p>";

            GenerateMessagePayload(from, to, subject, body);
        }

        private async void GenerateMessagePayload(string from, string to, string subject, string body)
        {
            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = from,
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { to }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = body
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = "text"
                            }
                        }
                    }
                };

                try
                {
                    Console.WriteLine("Sending email using Amazon SES...");
                    await client.SendEmailAsync(sendRequest);
                    Console.WriteLine("The email was sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + ex.Message);
                }
            }
        }
    }
}
