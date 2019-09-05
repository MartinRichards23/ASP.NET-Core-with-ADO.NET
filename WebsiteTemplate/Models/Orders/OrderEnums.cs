namespace WebsiteTemplate.Models
{
    public enum OrderStatus : short
    {
        Created = 0,
        Complete = 10,
    }

    public enum PaymentType : short
    {
        Card = 0,
        Paypal = 1,
    }

}
