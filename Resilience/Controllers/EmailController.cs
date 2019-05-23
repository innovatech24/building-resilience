using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Resilience.Controllers
{
    public class EmailController : Controller
    {        
                
        public void SendConfirmation(string toEmail)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDsGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("no-reply@roadtoresilience.tk", "Road to Resillience");
            var subject = "Welcome to Road to Resillience!";
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = "Welcome to Road to Resillience!\nThis is your one stop shop to track your goals and develop yourself professionally!\nDon't have time to meet your mentors? Your mentees are too far away?\nUse Road to Resillience to connect with them online and achieve everything that you would with a face to face meeting!";
            var htmlContent = "<strong>Welcome to Road to Resillience!</strong><p>This is your one stop shop to track your goals and develop yourself professionally!</p><p>Don't have time to meet your mentors? Your mentees are too far away?</p><p>Use Road to Resillience to connect with them online and achieve everything that you would with a face to face meeting!</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public void MentorConfirmation(string toEmail, string menteeFirstName, string menteeLastName, string mentorFirstName)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDsGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("no-reply@roadtoresilience.tk", "Road to Resillience");
            var subject = "Someone has added you as a mentor";
            var to = new EmailAddress(toEmail, mentorFirstName);
            var plainTextContent = "Hello " + mentorFirstName + ",\n" + menteeFirstName + " " + menteeLastName + " has added you as a mentor. If you think this is in error, please log on to Road to Resillience and remove them as a mentee.";
            var htmlContent = "Hello " + mentorFirstName + ",<p><strong>" + menteeFirstName + " " + menteeLastName + "</strong> has added you as a mentor. If you think this is in error, please log on to Road to Resillience and remove them as a mentee.";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public void MenteeConfirmation(string toEmail, string mentorFirstName, string mentorLastName, string menteeFirstName)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDsGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("no-reply@roadtoresilience.tk", "Road to Resillience");
            var subject = "Someone has added you as a mentee";
            var to = new EmailAddress(toEmail, menteeFirstName);
            var plainTextContent = "Hello " + menteeFirstName + ",\n" + mentorFirstName + " " + mentorLastName + " has added you as a mentor. If you think this is in error, please log on to Road to Resillience and remove them as a mentor.";
            var htmlContent = "Hello " + menteeFirstName + ",<p><strong>" + mentorLastName + " " + mentorLastName + "</strong> has added you as a mentor. If you think this is in error, please log on to Road to Resillience and remove them as a mentor.";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public void NewDiary(string toEmail, string mentorFirstName, string menteeFirstName, string menteeLastName)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDsGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("no-reply@roadtoresilience.tk", "Road to Resillience");
            var subject = "You have a new diary entry to review";
            var to = new EmailAddress(toEmail, mentorFirstName);
            var plainTextContent = "Hello " + mentorFirstName + ",\n" + menteeFirstName + " " + menteeLastName + " has added a new reflective diary entry. Please log on to Road to Resillience to review and provide feedback.";
            var htmlContent = "Hello " + mentorFirstName + ",<p><strong>" + menteeFirstName + " " + menteeLastName + "</strong> has added a new reflective diary entry. Please log on to Road to Resillience to review and provide feedback.";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public void NewTask(string toEmail, string mentorFirstName, string menteeFirstName, string menteeLastName)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDsGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("no-reply@roadtoresilience.tk", "Road to Resillience");
            var subject = "You have a new goal/task to review";
            var to = new EmailAddress(toEmail, mentorFirstName);
            var plainTextContent = "Hello " + mentorFirstName + ",\n" + menteeFirstName + " " + menteeLastName + " has added a new goal/task. Please log on to Road to Resillience to review and provide feedback.";
            var htmlContent = "Hello " + mentorFirstName + ",<p><strong>" + menteeFirstName + " " + menteeLastName + "</strong> has created a new goal/task. Please log on to Road to Resillience to review and provide feedback.";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public void EditTask(string toEmail, string menteeFirstName, string mentorFirstName, string mentorLastName)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDsGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("no-reply@roadtoresilience.tk", "Road to Resillience");
            var subject = "Yor mentor has proposed changes to your tasks";
            var to = new EmailAddress(toEmail, menteeFirstName);
            var plainTextContent = "Hello " + menteeFirstName + ",\n" + mentorFirstName + " " + mentorLastName + " has proposed changes to some tasks. Please log on to Road to Resillience to review.";
            var htmlContent = "Hello " + menteeFirstName + ",<p><strong>" + mentorFirstName + " " + mentorLastName + "</strong> has proposed changes to some tasks. Please log on to Road to Resillience to review.";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public void FeedbackProvided(string toEmail, string menteeFirstName, string mentorFirstName, string mentorLastName)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENsDGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("no-reply@roadtoresilience.tk", "Road to Resillience");
            var subject = "Yor mentor has proposed changes to your tasks";
            var to = new EmailAddress(toEmail, menteeFirstName);
            var plainTextContent = "Hello " + menteeFirstName + ",\n" + mentorFirstName + " " + mentorLastName + " has provided feedback on one of your diary entries. Please log on to Road to Resillience to review.";
            var htmlContent = "Hello " + menteeFirstName + ",<p><strong>" + mentorFirstName + " " + mentorLastName + "</strong> has provided feedback on one of your diary entries. Please log on to Road to Resillience to review.";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }
    }
}