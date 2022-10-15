using Basic.DataAccess;
using Basic.Model;
using MailKit.Net.Smtp;
using MimeKit;
using SmartFormat;

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
        /// <param name="options">The associated configuration options.</param>
        /// <param name="context">The current database context.</param>
        /// <param name="logger">The associated logger.</param>
        public EmailService(EmailServiceOptions options, Context context, ILogger<EmailService> logger)
        {
            this.Options = options ?? throw new ArgumentNullException(nameof(options));
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the associated configuration options.
        /// </summary>
        public EmailServiceOptions Options { get; }

        /// <summary>
        /// Gets the current database context.
        /// </summary>
        public Context Context { get; }

        /// <summary>
        /// Gets the associated logger.
        /// </summary>
        public ILogger<EmailService> Logger { get; }

        /// <summary>
        /// Notifies the user that has created an event about a status change on this event.
        /// </summary>
        /// <param name="event">The updated event.</param>
        /// <param name="change">The changed status.</param>
        public void EventStatusChanged(Event @event, EventStatus change)
        {
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }
            else if (change is null)
            {
                throw new ArgumentNullException(nameof(change));
            }

            if (string.IsNullOrWhiteSpace(@event.User.Email))
            {
                string warningMessage = "EventStatusChanged - Can't notify {0} - no email defined";
                this.Logger.LogWarning(warningMessage, @event.User.DisplayName);
                return;
            }

            var data = new
            {
                FrontBaseUrl = this.Options.FrontBaseUrl,
                Event = @event,
                Change = change,
            };

            // Prepare the headers
            using var message = new MimeMessage();
            message.From.Add(new MailboxAddress(this.Options.SenderName, this.Options.SenderEmail));
            message.To.Add(new MailboxAddress(@event.User.DisplayName, @event.User.Email));

            // Prepare the subject
            string subjectTemplate = File.ReadAllText(@"Templates/EventStatusChanged-Subject.txt");
            string subjectContent = Smart.Format(subjectTemplate, data).Trim();
            message.Subject = subjectContent;

            // Prepare the content
            string template = File.ReadAllText(@"Templates/EventStatusChanged-Content.txt");
            string content = Smart.Format(template, data).Trim();
            message.Body = new TextPart("plain")
            {
                Text = content,
            };

            this.Send(message);
        }

        /// <summary>
        /// Provides email to send to managment team
        /// </summary>
        public void EventCreated(Event @event)
        {
            // get the time approvers informations sending
            List<User> approvers = this.Context.Set<User>()
                .Where(u => u.Roles.Any(r => r.Code.Equals(Role.Time, StringComparison.Ordinal)))
                .Where(u => u.IsActive)
                .Where(u => !string.IsNullOrEmpty(u.Email))
                .ToList();

            if (approvers.Count == 0)
            {
                // No manager to send the notification to
                this.Logger.LogWarning("Setup inconsistency - No user with email defined has the role 'time' defined");
                return;
            }

            var data = new
            {
                FrontBaseUrl = this.Options.FrontBaseUrl,
                Event = @event,
            };

            // Prepare the headers
            using var message = new MimeMessage();
            message.From.Add(new MailboxAddress(this.Options.SenderName, this.Options.SenderEmail));
            foreach (User manager in approvers)
            {
                message.To.Add(new MailboxAddress(manager.DisplayName, manager.Email));
            }

            // Prepare the subject
            string subjectTemplate = File.ReadAllText(@"Templates/EventCreated-Subject.txt");
            string subjectContent = Smart.Format(subjectTemplate, data).Trim();
            message.Subject = subjectContent;

            // Prepare the content
            string template = File.ReadAllText(@"Templates/EventCreated-Content.txt");
            string content = Smart.Format(template, data).Trim();
            message.Body = new TextPart("plain")
            {
                Text = content,
            };

            this.Send(message);
        }

        /// <summary>
        /// Sends a message prepared in this class.
        /// </summary>
        /// <param name="message">The message to be send.</param>
        private void Send(MimeMessage message)
        {
            using var client = new SmtpClient();

            try
            {
                client.Connect(this.Options.Server, this.Options.Port, this.Options.Secure);
                client.Send(message);
            }
            catch (Exception exception)
            {
                this.Logger.LogError(exception, "Can't send a notification email");
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
