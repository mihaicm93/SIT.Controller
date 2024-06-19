using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using SIT.Controller.Areas.EmailSender;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SIT.Controller.Components.Account
{
    public class EmailSender : IEmailSender<IdentityUser>
    {
        private readonly MailgunOptions _options;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<MailgunOptions> optionsAccessor, ILogger<EmailSender> logger)
        {
            _options = optionsAccessor.Value;
            _logger = logger;
        }

        public Task SendConfirmationLinkAsync(IdentityUser user, string email, string confirmationLink)
        {
            return SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");
        }

        public Task SendPasswordResetLinkAsync(IdentityUser user, string email, string resetLink)
        {
            return SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");
        }

        public Task SendPasswordResetCodeAsync(IdentityUser user, string email, string resetCode)
        {
            return SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                if (string.IsNullOrEmpty(_options.ApiKey) || string.IsNullOrEmpty(_options.Domain))
                {
                    throw new Exception("Mailgun ApiKey or Domain is not set");
                }

                // Log email preparation
                _logger.LogInformation("Preparing to send email to {EmailAddress} with subject '{Subject}'", toEmail, subject);

                // Create RestClient instance
                var client = new RestClient($"https://api.eu.mailgun.net/v3/{_options.Domain}");

                // Create RestRequest for sending message
                var request = new RestRequest("messages", Method.Post);
                request.AddParameter("from", $"SIT Controller <noreply@{_options.Domain}>");
                request.AddParameter("to", toEmail);
                request.AddParameter("subject", subject);
                request.AddParameter("html", message); 


                request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{_options.ApiKey}")));

                _logger.LogInformation("Sending email request to Mailgun API");

                var response = await client.ExecuteAsync(request);


                // Handle response status
                if (response.IsSuccessful)
                {
                    _logger.LogInformation("Email to {EmailAddress} sent successfully!", toEmail);
                }
                else
                {
                    _logger.LogError("Failed to send email to {EmailAddress}. Status: {Status}, Error: {Error}, Content: {Content}", toEmail, response.StatusCode, response.ErrorMessage, response.Content);
                    throw new Exception($"Failed to send email: {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while sending email to {EmailAddress}", toEmail);
                throw;
            }
        }
    }
}
