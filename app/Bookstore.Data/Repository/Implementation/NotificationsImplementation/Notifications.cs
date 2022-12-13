using System;
using System.Collections.Generic;
using System.Text;
using Amazon;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using Bookstore.Data.Repository.Interface.NotificationsInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bookstore.Data.Repository.Implementation.NotificationsImplementation
{
    public class Notifications : INotifications
    {
        private readonly ILogger<Notifications> _logger;

        private const string EmailAddressesSectionName = "EmailAddresses";
        private const string FromEmailAddressKey = "FromEmailAddress";
        private const string ToEmailAddressDefaultKey = "ToEmailAddressDefault";

        private string FromEmailAddress { get; }
        private string DefaultToEmailAddress { get; }

        public Notifications(IConfiguration configuration, ILogger<Notifications> logger)
        {
            _logger = logger;

            // yields empty section if doesn't exist
            var emailSettings = configuration.GetSection(EmailAddressesSectionName);
            // verified yields null if keys do not exist (not an exception)
            FromEmailAddress = emailSettings[FromEmailAddressKey];
            DefaultToEmailAddress = emailSettings[ToEmailAddressDefaultKey];
        }

        public void SendOrderStatusUpdateEmail(string orderStatus, long orderId, string customerFirstName, string customerEmail)
        {
            var to = !string.IsNullOrEmpty(customerEmail) ? customerEmail : DefaultToEmailAddress;

            const string subject = "BobsBookstore - Order Status Update";

            var body =
                $"<h3>Hello {customerFirstName}</h3><p>The status for your order: {orderId} has changed to {orderStatus}<p>";

            GenerateMessagePayload(FromEmailAddress, to, subject, body);
        }

        public void SendItemRemovalEmail(string bookName, string bookCondition, string customerFirstName, string customerEmail)
        {
            var to = !string.IsNullOrEmpty(customerEmail) ? customerEmail : DefaultToEmailAddress;

            const string subject = "BobsBookstore - Out of stock notification";

            var body =
                $"<h3>Hello {customerFirstName}</h3><p>We are sorry to inform you that we dont have enough copies of {bookName} in the requested condition of {bookCondition}. A refund of your purchase will follow shortly.<p>";

            GenerateMessagePayload(FromEmailAddress, to, subject, body);
        }

        public void SendInventoryLowEmail(List<string> bookList, string customerEmail)
        {
            var to = !string.IsNullOrEmpty(customerEmail) ? customerEmail : DefaultToEmailAddress;

            const string subject = "Inventory Stock Running Low";

            var books = new StringBuilder();
            foreach (var i in bookList) books.Append($"{i}\n");

            var body =
                $"<h3>Hello {customerEmail}</h3><p>This is to inform you that the inventory for the following books is running low.</p><p>{books}</p>";

            GenerateMessagePayload(FromEmailAddress, to, subject, body);
        }

        private async void GenerateMessagePayload(string from, string to, string subject, string body)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                _logger.LogInformation("From and/or email addresses are not configured; skipping notification email");
                return;
            }

            using var client = new AmazonSimpleEmailServiceV2Client();
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
                _logger.LogInformation($"Error message: {ex.Message}");
            }
        }
    }
}
