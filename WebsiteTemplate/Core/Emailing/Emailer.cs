using System;
using System.Net;
using System.Threading.Tasks;
using SystemPlus.ComponentModel.Logging;
using SystemPlus.Text;
using SystemPlus.Web.Email;
using WebsiteTemplate.Models;

namespace WebsiteTemplate.Core.Emailing
{
    public class Emailer
    {
        #region Fields

        public IEmailSender EmailSender { get; }

        readonly string fromName = "WebsiteTemplate";

        readonly string emailHeader;
        readonly string emailFooter;

        readonly string signedupTemplate;
        readonly string changedPasswordTemplate;
        readonly string purchaseTemplate;

        #endregion

        public Emailer(IEmailSender emailSender, string template)
        {
            EmailSender = emailSender;

            emailHeader = template.GetFragment(null, "<!-- START:Changes -->");
            emailFooter = template.GetFragment("<!-- END:Changes -->", null);

            signedupTemplate = template.GetFragment("<!-- START:Signup -->", "<!-- END:Signup -->");
            changedPasswordTemplate = template.GetFragment("<!-- START:ChangedPassword -->", "<!-- END:ChangedPassword -->");
            purchaseTemplate = template.GetFragment("<!-- START:Purchase -->", "<!-- END:Purchase -->");

            EmailTools.ReplaceTags(ref emailHeader);
            EmailTools.ReplaceTags(ref emailFooter);
            EmailTools.ReplaceTags(ref signedupTemplate);
            EmailTools.ReplaceTags(ref changedPasswordTemplate);
            EmailTools.ReplaceTags(ref purchaseTemplate);
        }

        #region Send methods

        public async Task SendFramedEmailAsync(User user, string subject, string content, bool isHtml)
        {
            if (!isHtml)
            {
                content = WebUtility.HtmlEncode(content);
            }

            Guid emailId = Guid.NewGuid();

            string header = emailHeader;
            EmailTools.FormatHeader(ref header, emailId, subject, user);
            EmailTools.DoReplace(ref header, "[[Title]]", subject);
            string footer = emailFooter;

            string body = header + content + footer;

            await EmailSender.SendEmailAsync(user.Email, GlobalSettings.AdminEmail, fromName, subject, body, true).ContinueWithLogErrors();
        }

        public async Task SendSignedupAsync(User user)
        {
            string subject = "Thank you for registering!";
            string content = signedupTemplate;

            Guid emailId = Guid.NewGuid();
            string header = emailHeader;

            EmailTools.FormatHeader(ref header, emailId, subject, user);
            EmailTools.DoReplace(ref header, "[[Title]]", subject);
            string footer = emailFooter;

            string body = header + content + footer;

            await EmailSender.SendEmailAsync(user.Email, GlobalSettings.AdminEmail, fromName, subject, body, true).ContinueWithLogErrors();
        }

        public async Task SendChangedPasswordAsync(User user)
        {
            string subject = "Password changed";
            string content = changedPasswordTemplate;

            Guid emailId = Guid.NewGuid();
            string header = emailHeader;
            EmailTools.FormatHeader(ref header, emailId, subject, user);
            EmailTools.DoReplace(ref header, "[[Title]]", subject);
            string footer = emailFooter;

            string body = header + content + footer;

            await EmailSender.SendEmailAsync(user.Email, GlobalSettings.AdminEmail, fromName, subject, body, true).ContinueWithLogErrors();
        }

        #endregion
    }
}
