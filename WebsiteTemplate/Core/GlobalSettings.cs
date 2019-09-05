using System;

namespace WebsiteTemplate.Core
{
    public static class GlobalSettings
    {
        #region Email

        public static string FeedbackEmail
        {
            get { return "feedback@WebsiteTemplate.com"; }
        }

        public static string AdminEmail
        {
            get { return "admin@WebsiteTemplate.com"; }
        }

        #endregion
    }
}
