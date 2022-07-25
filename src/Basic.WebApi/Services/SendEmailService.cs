using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Basic.WebApi.Models;
using Basic.WebApi.DTOs;
using Basic.WebApi.Controllers;
using Basic.Model;

namespace Basic.WebApi.Services
{
    /// <summary>
    /// Provides emails services
    /// </summary>
    public static class SendEmailService
    {
        /// <summary>
        /// Provides emails sending test
        /// </summary>
        public static void EmailSendingTest()
        {
            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress("Basic", "system@incert.lu"));
            message.To.Add(new MailboxAddress("Kevin", "kgerber@incert.lu"));
            
            // formating the template to fill the email with variables
            string textFromTemplate = System.IO.File.ReadAllText(@"X:\_Projects\basic\front\public\email-to-employee-template copy.txt");
            textFromTemplate = string.Format(textFromTemplate, 12, 13);

            message.Body = new TextPart("plain")
            {
                Text = textFromTemplate
            };

            SmtpClient client = new SmtpClient();

            try
            {
                Console.WriteLine("Try to connect");
                client.Connect("localhost", 1025, false);

                Console.WriteLine("Try to send email");
                client.Send(message);
                Console.WriteLine("Success");
            }
            catch (Exception e)
            {
                Console.WriteLine("error: " + e.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
            Console.WriteLine("Over");
        }

        /// <summary>
        /// Provides emails sending to employee
        /// </summary>
        public static void EmailSendingToEmployee(Basic.WebApi.DTOs.UserForView toUser, string emailContent, Basic.WebApi.DTOs.UserForView fromUser)
        {
            toUser.DisplayName = "Margot Prezzavento";
            toUser.Email = "mprezzavento@incert.lu";
            fromUser.DisplayName = "Kevin Gerber";
            fromUser.Email = "kgerber@incert.lu";


            string toName = toUser.DisplayName.Split(' ')[0];
            string toEmail = toUser.Email;

            string fromName = fromUser.DisplayName;
            string fromEmail = fromUser.Email;

            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress("Basic", fromEmail));
            message.To.Add(new MailboxAddress("User", toEmail));

            string template = @"X:\_Projects\basic\front\public\email-to-employee-template.txt";

            // formating the template to fill the email with variables
            string textFromTemplate = System.IO.File.ReadAllText(template);
            textFromTemplate = string.Format(textFromTemplate, toName, emailContent, fromName);

            message.Body = new TextPart("plain")
            {
                Text = textFromTemplate
            };

            SmtpClient client = new SmtpClient();

            try
            {
                Console.WriteLine("Try to connect");
                client.Connect("localhost", 1025, false);

                Console.WriteLine("Try to send email");
                client.Send(message);

                Console.WriteLine("Success");
            }
            catch (Exception e)
            {
                Console.WriteLine("error: " + e.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
            Console.WriteLine("Over");
        }

        /// <summary>
        /// Provides emails sending to managment
        /// </summary>
        public static void EmailSendingToManagers(EventCategory category, User fromUser, EventForEdit @event)
        {
            // Get the managers emails
            string managersEmails = System.IO.File.ReadAllText(@"X:\_Projects\basic\front\public\managers-emails.txt");
            string[] managersEmailsList = managersEmails.Split(' ');
            string toName = "management team";

            // set up the string variables to display
            string fromDate = @event.StartDate.ToString();
            string toDate =  @event.EndDate.ToString();
            string displayCategory = category.DisplayName;
            string emailContent = $"{displayCategory} request from {fromDate}, to {toDate}.";
            string fromName = fromUser.DisplayName;

            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress("Basic", "system.basic@incert.lu"));
            foreach(string manager in managersEmailsList)
            {
                message.To.Add(new MailboxAddress("Management team", manager));
            }

            string template = @"X:\_Projects\basic\front\public\email-to-managing-hr-template.txt";

            // formating the template to fill the email with variables
            string textFromTemplate = System.IO.File.ReadAllText(template);
            textFromTemplate = string.Format(textFromTemplate, toName, emailContent, fromName);

            message.Body = new TextPart("plain")
            {
                Text = textFromTemplate
            };

            SmtpClient client = new SmtpClient();

            // Send email
            try
            {
                Console.WriteLine("Try to connect");
                client.Connect("localhost", 1025, false);

                Console.WriteLine("Try to send email");
                client.Send(message);

                Console.WriteLine("Success");
            }
            catch (Exception e)
            {
                Console.WriteLine("error: " + e.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
            Console.WriteLine("Over");
        }
    }
}
