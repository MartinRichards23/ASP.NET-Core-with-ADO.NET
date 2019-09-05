using System.Collections.Generic;

namespace WebsiteTemplate.Models
{
    public class SiteInfo
    {
        public int UsersCount { get; set; }
        public int LoginAttemptCount { get; set; }
        
        public int UsersNewCount { get; set; }

        public List<LogItem> Logs { get; set; } = new List<LogItem>();
    }

}
