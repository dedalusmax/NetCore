using Microsoft.EntityFrameworkCore;
using NetCore.Data;
using NetCore.Data.Common;
using NetCore.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Business.Services
{
    public class EmailService
    {
        private readonly EmailSender _emailSender;
        private readonly IGenericRepository<EmailTemplate> _emailTemplateRepository;

        public EmailService(EmailSender emailSender,
            IGenericRepository<EmailTemplate> emailTemplateRepository)
        {
            _emailSender = emailSender;
            _emailTemplateRepository = emailTemplateRepository;
        }

        public async Task<bool> SendEmailAsync(string emailAddress, EmailTemplateType templateType, IDictionary<string, string> placeholders = null, IList<string> ccEmails = null, IList<EmailAttachment> attachments = null)
        {
            var template = await _emailTemplateRepository.AsReadOnly().FirstOrDefaultAsync(_ => _.Type == templateType);

            if (template == null) return false;

            var subject = template.Subject;
            var body = template.Body;

            if (placeholders != null)
            {
                subject = ""; // template.Subject.ReplacePlaceholders(placeholders);
                body = ""; // template.Body.ReplacePlaceholders(placeholders);
            }

            var sent = await _emailSender.SendEmailAsync(emailAddress, subject, body, ccEmails, attachments);
            return sent;
        }

        public async Task<bool[]> SendEmailsAsync(string[] emailAddresses, EmailTemplateType templateType, IDictionary<string, string> placeholders)
        {
            var template = await _emailTemplateRepository.AsReadOnly().FirstOrDefaultAsync(_ => _.Type == templateType);

            if (template == null) return emailAddresses.Select(_ => false).ToArray();

            var subject = ""; // template.Subject.ReplacePlaceholders(placeholders);
            var body = ""; // template.Body.ReplacePlaceholders(placeholders);

            var sendTasks = new List<Task<bool>>();
            foreach (var emailAddress in emailAddresses)
            {
                var sendTask = _emailSender.SendEmailAsync(emailAddress, subject, body);
                sendTasks.Add(sendTask);
            }

            var result = await Task.WhenAll(sendTasks);
            return result;
        }

        public async Task<bool[]> SendEmailsAsync(string[] emailAddresses, string title, string message, IDictionary<string, string> placeholders = null)
        {
            var subject = ""; // template.Subject.ReplacePlaceholders(placeholders);
            var body = ""; // template.Body.ReplacePlaceholders(placeholders);

            var sendTasks = new List<Task<bool>>();
            foreach (var emailAddress in emailAddresses)
            {
                var sendTask = _emailSender.SendEmailAsync(emailAddress, subject, body);
                sendTasks.Add(sendTask);
            }

            var result = await Task.WhenAll(sendTasks);
            return result;
        }
    }

}
