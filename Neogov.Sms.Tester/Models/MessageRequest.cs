using System.ComponentModel.DataAnnotations;

namespace Neogov.Sms.Tester.Models
{
    public class MessageRequest
    {
        [Required]
        public int Count { get; set; }

        [Required, MaxLength(160)]
        public string Message { get; set; }
    }
}
