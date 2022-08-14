using Basic.DataAccess;
using Basic.Model;
using MailKit.Net.Smtp;
using MimeKit;

namespace Basic.WebApi.Services
{
    /// <summary>
    /// Provides email services.
    /// </summary>
    public class EmailService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="configuration">The current configuration.</param>
        /// <param name="context">The current database context.</param>
        public EmailService(IConfiguration configuration, Context context)
        {
            this.Configuration = configuration;
            this.Context = context;
        }

        /// <summary>
        /// Gets the current database context.
        /// </summary>
        public Context Context { get; }

        /// <summary>
        /// Provides a configuration for the email server.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Provides emails sending test
        /// </summary>
        public void EmailSendingTest()
        {
            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress("Basic", "basic-system@example.com"));
            message.To.Add(new MailboxAddress("John Doe", "demo@example.com"));

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
        /// Provides email to send to employee
        /// </summary>
        public void EmailToEmployee(User manager, User user, Event @event, Status from, Status to)
        {
            string toName = user.DisplayName.Split(' ')[0];
            string toEmail = user.Email;

            // to delete if not used
            string fromName = manager.DisplayName;
            string fromEmail = manager.Email;

            string emailContent = $"The status of your request from {@event.StartDate} to {@event.EndDate}, has been modified from {from.DisplayName} to {to.DisplayName}.";

            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress("Basic", "basic-noreply@example.com"));
            message.To.Add(new MailboxAddress(toName, toEmail));

            string template = @"Template/email-to-employee-template.txt";

            // formating the template to fill the email with variables
            string textFromTemplate = System.IO.File.ReadAllText(template);
            textFromTemplate = string.Format(textFromTemplate, toName, emailContent, fromName);

            message.Body = new TextPart("plain")
            {
                Text = textFromTemplate
            };

            EmailSending(message);
        }

        /// <summary>
        /// Provides email to send to managment team
        /// </summary>
        public void EmailToManagers(EventCategory category, User fromUser, Event @event)
        {
            // set up the string variables to display
            string fromDate = @event.StartDate.ToString();
            string toDate = @event.EndDate.ToString();
            string displayCategory = category.DisplayName;
            string emailContent = $"{displayCategory} request from {fromDate}, to {toDate}.";
            string fromName = fromUser.DisplayName;

            // message creation
            MimeMessage message = new MimeMessage();

            // get the managers informations sending
            List<User> managers = Context.Set<User>().Where(m => m.Roles.Any(u => u.Code.Equals("time") || u.Code.Equals("time-ro"))).ToList();

            // for loop to add multiple receivers
            foreach (User manager in managers)
            {
                message.To.Add(new MailboxAddress(manager.DisplayName, manager.Email));
            }

            // get the sender informations
            var emailServer = this.Configuration.GetRequiredSection("EmailServer");
            string sender = emailServer.GetValue<string>("sender");
            string senderEmail = emailServer.GetValue<string>("senderEmail");
            message.From.Add(new MailboxAddress(sender, senderEmail));

            // formating the template to fill the email with variables
            string templateLink = @"Template/email-to-managment-hr-template.txt";
            string textFromTemplate = System.IO.File.ReadAllText(templateLink);
            textFromTemplate = string.Format(textFromTemplate, emailContent, fromName);

            message.Body = new TextPart("plain")
            {
                Text = textFromTemplate
            };

            EmailSending(message);
        }

        /// <summary>
        /// Provides emails sending
        /// </summary>
        public void EmailSending(MimeMessage message)
        {
            SmtpClient client = new SmtpClient();

            // provides an email server configuration
            var emailServer = this.Configuration.GetRequiredSection("EmailServer");
            string host = emailServer.GetValue<string>("host");
            int port = emailServer.GetValue<int>("port");
            bool ssl = emailServer.GetValue<bool>("SSL");

            try
            {
                Console.WriteLine("Try to connect");
                client.Connect(host, port, ssl);

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
