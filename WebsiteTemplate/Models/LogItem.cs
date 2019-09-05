using System;

namespace WebsiteTemplate.Models
{
    public class LogItem
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public string Message { get; set; }
    }
}
