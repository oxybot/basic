using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Basic.WebApi.Models;


namespace Basic.WebApi.Services
{
    /// <summary>
    /// Provides emails services
    /// </summary>
    public class SendEmailService
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
        /// Provides emails sending
        /// </summary>
        public static void EmailSending(Basic.Model.User toUser, string emailContent, Basic.Model.User fromUser)
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

            // string template = @"X:\_Projects\basic\front\public\email-to-managing-hr-template.txt";
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
    }
}
