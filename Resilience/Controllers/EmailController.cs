using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Resilience.Controllers
{
    public class EmailController : Controller
    {
        public void Email()
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("no-reply@buildingresilience.tk", "Building Resilience");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("ksur0003@student.monash.edu", "Kiran" );
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }
    }
}