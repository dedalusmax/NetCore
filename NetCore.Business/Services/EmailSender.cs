using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetCore.Business.Services
{
    public class EmailSender
    {
        private const string TYPE_TO = "to";
        private const string TYPE_CC = "cc";
        private const string STATUS_SENT = "sent";
        private const string MANDRILL_SEND_ENDPOINT = "https://mandrillapp.com/api/1.0/messages/send.json";

        string mandrillKey { get; }
        string fromEmail { get; }
        string fromName { get; }
        bool configEmpty { get; }

        public EmailSender(IConfiguration configuration)
        {
            mandrillKey = configuration["MandrillKey"];
            fromEmail = configuration["FromEmail"];
            fromName = configuration["FromName"];

            configEmpty = string.IsNullOrWhiteSpace(mandrillKey) ||
                string.IsNullOrWhiteSpace(fromEmail) ||
                string.IsNullOrWhiteSpace(fromName);
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string message, IList<string> ccEmails = null, IList<EmailAttachment> attachments = null)
        {
            var toEmail = email;
            var ccMailList = ccEmails;

            // ######### IDA / SOFIE / ANDJELKO DEMO OVERRIDE ############
            if (email.EndsWith("@ida-demo.com", System.StringComparison.OrdinalIgnoreCase)) toEmail = "ida.stambuk@dink.eu";
            else if (email.EndsWith("@andjelko-demo.com", System.StringComparison.OrdinalIgnoreCase)) toEmail = "andjelko.marcinko@dink.eu";
            else if (email.EndsWith("@sofie-demo.com", System.StringComparison.OrdinalIgnoreCase)) toEmail = "sofie.vangestel@dink.eu";
            ccMailList = new List<string>();
            if (ccEmails != null)
            {
                foreach (var cc in ccEmails)
                {
                    var ccEmail = cc;
                    if (cc.EndsWith("@ida-demo.com", System.StringComparison.OrdinalIgnoreCase)) ccEmail = "ida.stambuk@dink.eu";
                    else if (cc.EndsWith("@andjelko-demo.com", System.StringComparison.OrdinalIgnoreCase)) ccEmail = "andjelko.marcinko@dink.eu";
                    else if (cc.EndsWith("@sofie-demo.com", System.StringComparison.OrdinalIgnoreCase)) ccEmail = "sofie.vangestel@dink.eu";
                    ccMailList.Add(ccEmail);
                }
            }

            if (configEmpty)
            {
                return false;
            }

            var to = new[]
            {
                new { email = toEmail, name = email, type = TYPE_TO }
            }
            .ToList();

            if (ccMailList != null)
            {
                to.AddRange(ccMailList.Select(_ => new { email = _, name = _, type = TYPE_CC }));
            }

            using (var client = new HttpClient { BaseAddress = new System.Uri(MANDRILL_SEND_ENDPOINT) })
            {
                var requestData = new
                {
                    key = mandrillKey,
                    message = new
                    {
                        html = message,
                        subject = subject,
                        from_email = fromEmail,
                        from_name = fromName,
                        to,
                        preserve_recipients = true,
                        attachments = attachments
                    },
                    async = false
                };

                var requestString = JsonConvert.SerializeObject(requestData);

                var response = await client.PostAsync(string.Empty, new StringContent(requestString));
                var resultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeAnonymousType(
                    resultString,
                    new[]
                    {
                        new { email = string.Empty, status = string.Empty, reject_reason = string.Empty, _id = string.Empty }
                    });


                return result.Length == 1 && result[0].status == STATUS_SENT;
            }
        }
    }

}
