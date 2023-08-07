using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using TimeTigrerFunction.Model;


namespace TimeTigrerFunction
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public void Run([TimerTrigger("%FUNCTIONS%")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }


        [Function("ReminderFunction")]
        public static async Task Run2Async([TimerTrigger("0 * * * * *")] MyInfo myTimer, FunctionContext context)
        {
            var logger = context.GetLogger("ReminderFunction");

                string[] recipientEmails = new string[]
          {
                "maheshworlamichhane777@gmail.com",
                "maheshworlamichhane07@gmail.com",
              // Add more recipient email addresses as needed.
          };

           // string recipientEmail = "sangamrimal4@gmail.com";
            

            // email subject and content
            string emailSubject = "Reminder: Don't forget to do something!";
            string emailContent = "This is a friendly reminder to do something important.";




            /* try
             {
                 await EmailService.SendReminderEmail(recipientEmail, emailSubject, emailContent);
                 logger.LogInformation($"Reminder email sent successfully from: {EmailService.SMTPSetting.Email}");
             }*/
            try
            {
                foreach (string recipientEmail in recipientEmails)
                {
                    await EmailService.SendReminderEmail(recipientEmail, emailSubject, emailContent);
                    logger.LogInformation($"Reminder email sent successfully to: {recipientEmail} from: {EmailService.SMTPSetting.Email}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error sending reminder email: {ex.Message}");
            }
        }
    }
    public static class EmailService
    {
        private static SMTPSetting smtpSetting;

        // Property to set the SMTPSetting
        public static SMTPSetting SMTPSetting
        {
            get
            {
                if (smtpSetting == null)
                {
                    smtpSetting = new SMTPSetting();
                    return smtpSetting;
                }
                
                else

                    return smtpSetting;
            }
            set
            {
                smtpSetting = value;
            }
        }


        public static async Task SendReminderEmail(string toEmail, string subject, string content)
        {
            // Replace with your SMTP server and credentials


            //string smtpServer = "smtp.gmail.com";
            //int smtpPort = 587;
            //string smtpUsername = "maheshworlamichhane777@gmail.com";
            //string smtpPassword = "xqwzinhtdxguaczt";

            //String Email = smtpSetting.Email;


            using (var client = new SmtpClient(smtpSetting.Host, smtpSetting.Port))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(smtpSetting.Email, smtpSetting.Secret);

                var from = new MailAddress("maheshworlamichhane777@gmail.com", "Maheshwor Name");
                var to = new MailAddress(toEmail);
                var message = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = content,
                    IsBodyHtml = true // Set to false if your content is plain text
                };

                await client.SendMailAsync(message);
            }
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
