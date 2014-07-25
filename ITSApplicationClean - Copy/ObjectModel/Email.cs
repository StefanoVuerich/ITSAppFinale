using System.ComponentModel.DataAnnotations;

namespace ObjectModel
{
    public class Email
    {
        [Required]
        public string Sender { get; set; }

        [Required]
        [EmailAddress]
        public string From { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}