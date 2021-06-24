﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using BookstoreBackend.Notifications.NotificationsInterface;

namespace BookstoreBackend.Notifications.Implementations
{
    public class Notifications : INotifications

    {
        private IConfiguration Configuration;

        public Notifications(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        private async void GenerateMessagePayload(string FROM, string TO,string subject, string body)
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
            string FROM = Configuration.GetConnectionString("FromEmailAddress");

            string TO = String.IsNullOrEmpty(Configuration.GetConnectionString("ToEmailAddressDefault")) ? customerEmail : Configuration.GetConnectionString("ToEmailAddressDefault");
           
            string subject = "BOBS Books Update to Order Status";

            string body =
                "<h3>Hello " + customerFirstName + "</h3>" +
                "<p> Your Order Status for Order:" + orderId + " has changed to " + orderStatus + "<p>";

            GenerateMessagePayload(FROM,TO,subject, body);
           
            
        }

        public void SendItemRemovalEmail(string bookName, string bookCondition, string customerFirstName, string customerEmail)
        {
            string FROM = Configuration.GetConnectionString("FromEmailAddress");

            string TO = String.IsNullOrEmpty(Configuration.GetConnectionString("ToEmailAddressDefault")) ? customerEmail : Configuration.GetConnectionString("ToEmailAddressDefault");

            string subject = "BOBS Books Can't Find Book";

            string body =
                "<h3>Hello " + customerFirstName + "</h3>" +
                "<p> We are sorry to inform you that we dont have enough copies of " + bookName + " in the condition of " + bookCondition + " . Please visit your nearest BOBS Branch to get a refund. <p>"; 

            GenerateMessagePayload(FROM, TO, subject, body);
        }

        public void SendInventoryLowEmail(List<string> BookList, string customerEmail)
        {
            string FROM = Configuration.GetConnectionString("FromEmailAddress");

            string TO = String.IsNullOrEmpty(Configuration.GetConnectionString("ToEmailAddressDefault")) ? customerEmail : Configuration.GetConnectionString("ToEmailAddressDefault");

            string subject = "Inventory Stock Running Low";

            string bookList = "";

            foreach (var i in BookList)
            {
                bookList = bookList + i + "\n";
            }

            string body =
                "<h3>Hello " + "Bob" + "</h3>" +
                "<p>This is to inform you that the inventory for the following books is running low.  <p>" + "\n" + bookList;
            
            GenerateMessagePayload(FROM, TO, subject, body);
        }

    }
}