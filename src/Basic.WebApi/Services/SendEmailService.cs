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
        public static void EmailSending(User toUser, string emailContent, string from)
        {

            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress("Basic", "system@incert.lu"));
            message.To.Add(new MailboxAddress("User", to));

            // formating the template to fill the email with variables
            string textFromTemplate = System.IO.File.ReadAllText(@"X:\_Projects\basic\front\public\email-to-employee-template copy.txt");
            textFromTemplate = string.Format(textFromTemplate, to, emailContent, from);

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
