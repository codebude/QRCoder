using System;

namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates an SMS payload.
    /// </summary>
    public class SMS : Payload
    {
        private readonly string _number, _subject;
        private readonly SMSEncoding _encoding;

        /// <summary>
        /// Creates an SMS payload without text.
        /// </summary>
        /// <param name="number">Receiver phone number.</param>
        /// <param name="encoding">Encoding type.</param>
        public SMS(string number, SMSEncoding encoding = SMSEncoding.SMS)
        {
            _number = number;
            _subject = string.Empty;
            _encoding = encoding;
        }

        /// <summary>
        /// Creates an SMS payload with text (subject).
        /// </summary>
        /// <param name="number">Receiver phone number.</param>
        /// <param name="subject">Text of the SMS.</param>
        /// <param name="encoding">Encoding type.</param>
        public SMS(string number, string subject, SMSEncoding encoding = SMSEncoding.SMS)
        {
            _number = number;
            _subject = subject;
            _encoding = encoding;
        }

        /// <summary>
        /// Returns the SMS payload as a string.
        /// </summary>
        /// <returns>The SMS payload as a string.</returns>
        public override string ToString()
        {
            var returnVal = string.Empty;
            switch (_encoding)
            {
                case SMSEncoding.SMS:
                    var queryString = string.Empty;
                    if (!string.IsNullOrEmpty(_subject))
                        queryString = $"?body={Uri.EscapeDataString(_subject)}";
                    returnVal = $"sms:{_number}{queryString}";
                    break;
                case SMSEncoding.SMS_iOS:
                    var queryStringiOS = string.Empty;
                    if (!string.IsNullOrEmpty(_subject))
                        queryStringiOS = $";body={Uri.EscapeDataString(_subject)}";
                    returnVal = $"sms:{_number}{queryStringiOS}";
                    break;
                case SMSEncoding.SMSTO:
                    returnVal = $"SMSTO:{_number}:{_subject}";
                    break;
            }
            return returnVal;
        }

        /// <summary>
        /// Specifies the encoding type for the SMS payload.
        /// </summary>
        public enum SMSEncoding
        {
            /// <summary>
            /// Standard SMS encoding.
            /// </summary>
            SMS,
            /// <summary>
            /// SMSTO encoding.
            /// </summary>
            SMSTO,
            /// <summary>
            /// SMS encoding for iOS.
            /// </summary>
            SMS_iOS
        }
    }
}
