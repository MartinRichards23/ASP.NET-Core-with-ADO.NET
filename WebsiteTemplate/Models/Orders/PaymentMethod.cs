using System;

namespace WebsiteTemplate.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }
        public bool IsEnabled { get; set; }

        public PaymentType PaymentType { get; set; }
        public CardDetails CardDetails { get; set; }

        public DateTime? LastAutoPaymentTime { get; set; }
        public DateTime? NextAutoPaymentTime { get; set; }
        public PaymentMode PaymentMode { get; set; }

        public int AddressId { get; set; }

        public string FriendlyName()
        {
            if (CardDetails != null)
            {
                return CardDetails.MaskedNumber;
            }

            return "";
        }
    }

    public class CardDetails
    {
        public string Token { get; set; }
        public bool Reusable { get; set; }

        public string Name { get; set; }
        public string MaskedNumber { get; set; }
        public string CardType { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }

        public bool IsDemo { get; set; }
    }

    public enum PaymentMode : byte
    {
        None = 0,
        VariableMonthly = 1,
        FixedMonthly = 2,
    }

}
