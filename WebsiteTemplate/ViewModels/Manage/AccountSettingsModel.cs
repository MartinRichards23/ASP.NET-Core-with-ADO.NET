using System.Collections.Generic;
using WebsiteTemplate.Models;

namespace WebsiteTemplate.ViewModels
{
    public class AccountSettingsModel
    {
        public User User { get; set; }
        public IList<LoginAttempt> Logins { get; set; }
    }
}
