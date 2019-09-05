using System.Collections.Generic;
using WebsiteTemplate.Models;

namespace WebsiteTemplate.ViewModels
{
    public class OrderHistoryModel
    {
        public IList<Order> Orders { get; set; } = new List<Order>();
        public IList<PurchaseTransaction> Transactions { get; set; } = new List<PurchaseTransaction>();
    }

    public class SetAutoTopupModel
    {
        public IList<PaymentMethod> Methods { get; set; }
    }

    public class CostModel
    {
        public string cost { get; set; }
    }

    public class CreateOrderModel
    {
        public int Choice { get; set; }
        public decimal Cost { get; set; }
    }

    public class ConfirmOrderModel
    {
        public Order Order { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }

}
