using System.ComponentModel.DataAnnotations;

namespace WebsiteTemplate.ViewModels
{
    public class NewCardModel
    {
        public string Token { get; set; }

        public string Name { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        [Required]
        public string City { get; set; }

        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string PostCode { get; set; }
    }
}
