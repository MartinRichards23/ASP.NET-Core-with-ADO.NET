using System;

namespace WebsiteTemplate.Models
{
    public class PurchaseTransaction
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }
        public int? OrderId { get; set; }
        public string Product { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string Info { get; set; }
    }
}
