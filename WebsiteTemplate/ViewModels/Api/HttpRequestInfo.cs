using System.Collections.Generic;

namespace WebsiteTemplate.ViewModels.Api
{
    public class HttpRequestInfo
    {
        public string IpAddress { get; set; }

        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }
}
