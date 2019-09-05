using System;

namespace WebsiteTemplate.Models
{
    public class Role
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}
