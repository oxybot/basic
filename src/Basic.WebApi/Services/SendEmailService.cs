using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;


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

            message.From.Add(new MailboxAddress("Basic", "basic-no-reply@gmx.com"));
            message.To.Add(new MailboxAddress("Kevin", "kgerber@incert.lu"));

            message.Body = new TextPart("plain")
            {
                Text = @"Email content:
                
                    Bests,
                    Basic Team"
            };

            string emailAddress = "superpapajulien@gmail.com";
            string password = "dpdnhepmqprflrmk";

            SmtpClient client = new SmtpClient();

            try
            {
                Console.WriteLine("Try to connect");
                client.Connect("smtp.gmail.com", 465, true);


                Console.WriteLine("Try to authenticate");
                client.Authenticate(emailAddress, password);


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
        public static void EmailSending(string from, string to, string emailContent)
        {
            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress("Basic", "basic-no-reply@gmx.com"));
            message.To.Add(new MailboxAddress("Kevin", to));

            message.Body = new TextPart("plain") // or "html"
            {
                Text = emailContent
            };

            string emailAddress = "superpapajulien@gmail.com";
            string password = "dpdnhepmqprflrmk";

            SmtpClient client = new SmtpClient();

            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(emailAddress, password);
                client.Send(message);
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
        }
    }
}