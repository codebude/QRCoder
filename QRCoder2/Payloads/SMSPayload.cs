namespace QRCoder2.Payloads
{
    public class SMSPayload : PayloadBase
    {
        private readonly string number, subject;
        private readonly SMSEncoding encoding;

        /// <summary>
        /// Creates a SMS payload without text
        /// </summary>
        /// <param name="number">Receiver phone number</param>
        /// <param name="encoding">Encoding type</param>
        public SMSPayload(string number, SMSEncoding encoding = SMSEncoding.SMS)
        {
            this.number = number;
            this.subject = string.Empty;
            this.encoding = encoding;
        }

        /// <summary>
        /// Creates a SMS payload with text (subject)
        /// </summary>
        /// <param name="number">Receiver phone number</param>
        /// <param name="subject">Text of the SMS</param>
        /// <param name="encoding">Encoding type</param>
        public SMSPayload(string number, string subject, SMSEncoding encoding = SMSEncoding.SMS)
        {
            this.number = number;
            this.subject = subject;
            this.encoding = encoding;
        }

        public override string ToString()
        {
            switch (this.encoding)
            {
                case SMSEncoding.SMS:
                    return $"sms:{this.number}?body={System.Uri.EscapeDataString(this.subject)}";
                case SMSEncoding.SMS_iOS:
                    return $"sms:{this.number};body={System.Uri.EscapeDataString(this.subject)}";
                case SMSEncoding.SMSTO:
                    return $"SMSTO:{this.number}:{this.subject}";
                default:
                    return "sms:";
            }
        }

        public enum SMSEncoding
        {
            SMS,
            SMSTO,
            SMS_iOS
        }
    }
}