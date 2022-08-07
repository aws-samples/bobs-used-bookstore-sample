using System;
using System.Collections.Generic;
using System.Text;
using Amazon;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using BobsBookstore.DataAccess.Repository.Interface.NotificationsInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BobsBookstore.DataAccess.Repository.Implementation.NotificationsImplementations
{
    public class Notifications : INotifications
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger<Notifications> _logger;


        public Notifications(IConfiguration configuration, ILogger<Notifications> logger)
        {
            Configuration = configuration;
            _logger = logger;
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
            using (var client = new AmazonSimpleEmailServiceV2Client(RegionEndpoint.USWest2))
            {
                var sendRequest = new SendEmailRequest
                {
                    FromEmailAddress = from,
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { to }
                    },
                   
                    Content = new EmailContent()
                    {
                        Simple = new Message
                        {
                            Subject = new Content
                            {
                                Data = subject,
                                Charset = "UTF-8"
                            },
                            Body = new Body
                            {
                                Html = new Content
                                {
                                    Data = body,
                                    Charset = "UTF-8"
                                },
                                Text = new Content
                                {
                                    Data = "text",
                                    Charset = "UTF-8"
                                }
                            }
                        }
                    }
                
                    
                };

                try
                {
                    _logger.LogInformation("Sending email using Amazon SES...");
                    await client.SendEmailAsync(sendRequest);
                    _logger.LogInformation("The email was sent successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("The email was not sent.");
                    _logger.LogInformation("Error message: " + ex.Message);
                }
            }
        }
    }
}
