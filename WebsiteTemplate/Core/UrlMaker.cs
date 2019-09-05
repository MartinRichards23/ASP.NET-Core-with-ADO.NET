using System.Collections.Generic;

namespace WebsiteTemplate.Core
{
    public static class UrlMaker
    {
        public static string Home
        {
            get { return "https://WebsiteTemplate.com"; }
        }

        public static string ToFullUrl(this string url)
        {
            return Home + url;
        }

        #region Images

        public static string Plus_green
        {
            get { return "/images/icons/plus_green.png"; }
        }

        public static string Minus_red
        {
            get { return "/images/icons/minus_red.png"; }
        }

        public static string Tick_green
        {
            get { return "/images/icons/tick_green.png"; }
        }

        public static string Warning_red
        {
            get { return "/images/icons/warning_red.png"; }
        }

        public static string Warning_orange
        {
            get { return "/images/icons/warning_orange.png"; }
        }

        public static string Pause
        {
            get { return "/images/icons/pause.png"; }
        }

        public static string Start
        {
            get { return "/images/icons/play.png"; }
        }

        #endregion

        #region Global

        public static string ContactUrl()
        {
            return "/Contact";
        }

        public static string AccountUrl()
        {
            return "/Manage";
        }

        public static string OrdersUrl()
        {
            return "/Orders";
        }

        public static string ManagePaymentsUrl()
        {
            return "/Orders/ManagePaymentMethods";
        }

        public static string PricingUrl()
        {
            return "/Orders/Pricing";
        }

        public static string TermsOfServiceUrl()
        {
            return "/TermsOfService";
        }

        public static string PrivacyStatementUrl()
        {
            return "/PrivacyStatement";
        }

        #endregion

        public static Dictionary<string, string> GetTagReplacements()
        {
            Dictionary<string, string> replacements = new Dictionary<string, string>
            {
                { "[[URL:Home]]", Home },
                { "[[URL:Account]]", AccountUrl().ToFullUrl() },
                { "[[URL:Pricing]]", PricingUrl().ToFullUrl() },
                { "[[URL:Contact]]", ContactUrl().ToFullUrl() },
                { "[[URL:Orders]]", OrdersUrl().ToFullUrl() },
                { "[[URL:AutoTopup]]", ManagePaymentsUrl().ToFullUrl() },
                { "[[URL:PrivacyStatement]]", PrivacyStatementUrl().ToFullUrl() },
                { "[[URL:TermsOfService]]", TermsOfServiceUrl().ToFullUrl() }
            };

            return replacements;
        }
    }
}
