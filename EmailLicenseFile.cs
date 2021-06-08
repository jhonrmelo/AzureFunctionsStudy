using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AzfPluralsight
{
    public static class EmailLicenseFile
    {
        [FunctionName("EmailLicenseFile")]
        public static void Run([BlobTrigger("licenses/{orderId}.lic", Connection = "AzureWebJobsStorage")] string licenseFile,
            string orderId,
            //table/partitionKey/RowKey => select the entity
            [Table("orders","orders","{orderId}")] Order order,
            ILogger log)
        {
            var smtClient = GetSmtpServer();
            var email = order.Email;
            string emailSender = Environment.GetEnvironmentVariable("EmailSender");

            var message = new MailMessage()
            {
                From = new MailAddress(emailSender),
                Subject = "Your License File",
                IsBodyHtml = false,
                Body = "Thank you for your order",

            };

            message.To.Add(new MailAddress(email));


            var plainTextBytes = Encoding.UTF8.GetBytes(licenseFile);
            MemoryStream stream = new MemoryStream(plainTextBytes);

            message.Attachments.Add(new System.Net.Mail.Attachment(stream, $"{orderId}.lic", "plain/text"));

            if (!email.EndsWith("@test.com"))
                smtClient.Send(message);

            log.LogInformation($"Got Order from {email} \n Order Id: {orderId}");
        }


        private static SmtpClient GetSmtpServer()
        {
            string server = Environment.GetEnvironmentVariable("EmailServer");
            int emailServerPort = Convert.ToInt32(Environment.GetEnvironmentVariable("EmailServerPort"));
            string emailSender = Environment.GetEnvironmentVariable("EmailSender");
            string password = Environment.GetEnvironmentVariable("Password");
            return new SmtpClient(server, emailServerPort)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailSender, password),
                EnableSsl = true
            };
        }
    }
}
