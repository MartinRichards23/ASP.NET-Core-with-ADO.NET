using System;
using System.Runtime.Serialization;

namespace WebsiteTemplate.Models
{
    /// <summary>
    /// Config for a user
    /// </summary>
    [DataContract]
    public class UserConfig
    {
        [DataMember]
        string timeZone;

        [DataMember]
        string currency;

        [DataMember]
        bool apiEnabled;

        TimeZoneInfo timeZoneInfo;
        readonly Currency currencyInfo;

        #region Properties

        public string TimeZone
        {
            get { return timeZone; }
            set { timeZone = value; }
        }

        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }

        public bool ApiEnabled
        {
            get { return apiEnabled; }
            set { apiEnabled = value; }
        }

        public TimeZoneInfo TimeZoneInfo
        {
            get
            {
                if (timeZoneInfo == null && !string.IsNullOrEmpty(timeZone))
                {
                    try
                    {
                        timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                    }
                    catch
                    { }
                }

                return timeZoneInfo;
            }
        }

        public Currency CurrencyInfo
        {
            get
            {
                //if (currencyInfo == null)
                //{
                //    currencyInfo = PaymentTools.GetCurrency(Currency);

                //    if (currencyInfo == null)
                //        currencyInfo = PaymentTools.GetCurrency("GBP");
                //}

                return currencyInfo;
            }
        }

        public static UserConfig DefaultConfig
        {
            get { return new UserConfig(); }
        }

        #endregion

    }
}
