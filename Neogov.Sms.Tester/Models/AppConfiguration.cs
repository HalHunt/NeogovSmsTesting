using System.ComponentModel.DataAnnotations;

namespace Neogov.Sms.Tester.Models
{
    public class AppConfiguration
    {
        #region Properties

        public const string ConfigKeyTwilioAuthToken = "TwilioAuthToken";
        public const string ConfigKeyMessageToNumber = "MessageToNumber";
        public const string ConfigKeyMessagesPageSize = "MessagesPageSize";

        [Required]
        public string TwilioAuthToken { get; set; }

        [Required, MaxLength(12)]
        public string MessageToNumber { get; set; }

        public int MessagesPageSize { get; set; } = 25;

        #endregion

        #region Methods

        public AppConfiguration WithFormattedPhoneNumbers()
        {
            return new AppConfiguration
            {
                TwilioAuthToken = TwilioAuthToken,
                MessageToNumber = PhoneNumberHelper.FormatNumber(MessageToNumber),
                MessagesPageSize = MessagesPageSize
            };
        }

        #endregion
    }
}
