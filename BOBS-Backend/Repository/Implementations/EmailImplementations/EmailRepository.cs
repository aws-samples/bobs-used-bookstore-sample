using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using BOBS_Backend.Models.Order;
using BOBS_Backend.Repository.EmailInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository.Implementations.EmailImplementations
{
    public class EmailRepository : IEmailRepository

    {

        private const string FROM = "ancelmop@amazon.com";

        private async void GenerateMessagePayload(string subject, string body)
        {
            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = FROM,
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { FROM }
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
                                Data = "tetx"
                            }
                        }
                    },
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

        public void SendOrderStatusUpdateEmail(string orderStatus,long orderId, string customerFirstName, string customerEmail)
        {
            string subject = "BOBS Books Update to Order Status";

            string body =
                "<h3>Hello " + customerFirstName + "</h3>" +
                "<p> Your Order Status for Order:" + orderId + " has changed to " + orderStatus + "<p>";

            GenerateMessagePayload(subject, body);
           
            
        }

    }
}
