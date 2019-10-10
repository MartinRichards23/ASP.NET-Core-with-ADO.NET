using System;
using System.Collections.Generic;
using System.Net;
using WebsiteTemplate.Models;

namespace WebsiteTemplate.Core.Emailing
{
    public static class EmailTools
    {
        public static Dictionary<string, string> Replacements { get; } = new Dictionary<string, string>();

        public static void DoReplace(ref string html, string tag, object value, bool htmlEncode = true)
        {
            string s;

            if (value != null)
                s = value.ToString();
            else
                s = null;

            DoReplace(ref html, tag, s, htmlEncode);
        }

        public static void DoReplace(ref string html, string tag, string value, bool htmlEncode = true)
        {
            if (value == null)
                value = string.Empty;

            if (htmlEncode)
                value = WebUtility.HtmlEncode(value);

            html = html.Replace(tag, value);
        }

        public static void ReplaceTags(ref string html)
        {
            foreach (KeyValuePair<string, string> kvp in Replacements)
            {
                DoReplace(ref html, kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Wraps the content text in a span with an appropriate style
        /// </summary>
        public static string WrapContent(string content, string tag)
        {
            string style;

            if (tag == "h1")
                style = "font-size:20px;";
            else if (tag == "h2")
                style = "font-size:19px;";
            else if (tag == "h3")
                style = "font-size:18px;";
            else if (tag == "h4")
                style = "font-size:17px;";
            else if (tag == "h5")
                style = "font-size:16px;";
            else if (tag == "h6")
                style = "font-size:15px;";
            else
                style = "font-size:14px;";

            content = string.Format("<span style=\"{0}\">{1}</span>", style, content);
            return content;
        }

        /// <summary>
        /// Applies standard replacements to header
        /// </summary>
        public static void FormatHeader(ref string header, Guid emailId, string previewText, User user)
        {
            DoReplace(ref header, "[[PreviewText]]", previewText);
            DoReplace(ref header, "[[EmailId]]", emailId.ToString());
        }

    }
}
