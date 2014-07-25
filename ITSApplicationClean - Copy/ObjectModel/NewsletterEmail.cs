using System.ComponentModel.DataAnnotations;

namespace ObjectModel
{
    public class NewsletterEmail
    {
        [Required]
        [EmailAddress]
        public string EmailAdress { get; set; }
    }
}