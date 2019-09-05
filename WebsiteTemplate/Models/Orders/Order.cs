using System;

namespace WebsiteTemplate.Models
{
    public class Order
    {
        public Order(int userId, string currency)
        {
            UserId = userId;
            Currency = currency;
        }

        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }
        public int? PaymentMethodId { get; set; }
        public string Code { get; set; }
        public OrderStatus Status { get; set; }
        public int Value { get; set; }
        public string Currency { get; set; }

        /// <summary>
        /// Value in payment method currency
        /// </summary>
        public decimal ValueInCurrency
        {
            get { return Value / 100m; }
        }

        public bool CanDelete
        {
            get { return Status == OrderStatus.Created; }
        }
    }

    public class Currency
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
