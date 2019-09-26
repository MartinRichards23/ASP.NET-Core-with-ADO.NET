using System.Collections.Generic;
using WebsiteTemplate.Models;

namespace WebsiteTemplate.ViewModels
{
    public class SiteAdminModel
    {
        public SiteInfo Info { get; set; }
    }

    public class SiteAdminUsersModel
    {
        public List<UserInfo> UserInfo { get; set; }
    }
}
