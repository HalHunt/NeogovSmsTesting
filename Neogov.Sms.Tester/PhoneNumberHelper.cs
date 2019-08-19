using System;

namespace Neogov.Sms.Tester
{
    public class PhoneNumberHelper
    {
        public static string FormatNumber(string input)
        {
            if (!String.IsNullOrWhiteSpace(input))
            {
                if (input.Length == 12)
                    return $"{input.Substring(0, 2)} ({input.Substring(2, 3)}) {input.Substring(5, 3)}-{input.Substring(8)}";
                else if (input.Length == 11)
                    return $"+{input.Substring(0, 1)} ({input.Substring(1, 3)}) {input.Substring(4, 3)}-{input.Substring(7)}";
                else if (input.Length == 10)
                    return $"+1 ({input.Substring(0, 3)}) {input.Substring(3, 3)}-{input.Substring(6)}";
            }
            return String.IsNullOrWhiteSpace(input) ? "unknown" : input;
        }
    }
}
