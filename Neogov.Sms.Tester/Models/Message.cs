using System;
using System.ComponentModel.DataAnnotations;

namespace Neogov.Sms.Tester.Models
{
    public class Message
    {
        #region Properties

        [Key, Required]
        public int Id { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(12)]
        public string To { get; set; }

        [Required, MaxLength(12)]
        public string From { get; set; }

        [Required]
        public string Body { get; set; }

        #endregion

        #region Methods

        public Message WithFormattedPhoneNumbers()
        {
            return new Message
            {
                Id = Id,
                CreatedUtc = CreatedUtc,
                From = PhoneNumberHelper.FormatNumber(From),
                Body = Body,
                To = PhoneNumberHelper.FormatNumber(To)
            };
        }

        #endregion
    }
}
