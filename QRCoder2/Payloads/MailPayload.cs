namespace QRCoder2.Payloads
{
    public class MailPayload : PayloadBase
    {
        private readonly string mailReceiver, subject, message;
        private readonly MailEncoding encoding;

        /// <summary>
        /// Creates an empty email payload
        /// </summary>
        /// <param name="mailReceiver">Receiver's email address</param>
        /// <param name="encoding">Payload encoding type. Choose dependent on your QR Code scanner app.</param>
        public MailPayload(string mailReceiver, MailEncoding encoding = MailEncoding.MAILTO)
        {
            this.mailReceiver = mailReceiver;
            this.subject = this.message = string.Empty;
            this.encoding = encoding;
        }

        /// <summary>
        /// Creates an email payload with subject
        /// </summary>
        /// <param name="mailReceiver">Receiver's email address</param>
        /// <param name="subject">Subject line of the email</param>
        /// <param name="encoding">Payload encoding type. Choose dependent on your QR Code scanner app.</param>
        public MailPayload(string mailReceiver, string subject, MailEncoding encoding = MailEncoding.MAILTO)
        {
            this.mailReceiver = mailReceiver;
            this.subject = subject;
            this.message = string.Empty;
            this.encoding = encoding;
        }

        /// <summary>
        /// Creates an email payload with subject and message/text
        /// </summary>
        /// <param name="mailReceiver">Receiver's email address</param>
        /// <param name="subject">Subject line of the email</param>
        /// <param name="message">Message content of the email</param>
        /// <param name="encoding">Payload encoding type. Choose dependent on your QR Code scanner app.</param>
        public MailPayload(string mailReceiver, string subject, string message, MailEncoding encoding = MailEncoding.MAILTO)
        {
            this.mailReceiver = mailReceiver;
            this.subject = subject;
            this.message = message;
            this.encoding = encoding;
        }

        public override string ToString()
        {
            switch (this.encoding)
            {
                case MailEncoding.MAILTO:
                    return
                        $"mailto:{this.mailReceiver}?subject={System.Uri.EscapeDataString(this.subject)}&body={System.Uri.EscapeDataString(this.message)}";
                case MailEncoding.MATMSG:
                    return
                        $"MATMSG:TO:{this.mailReceiver};SUB:{EscapeInput(this.subject)};BODY:{EscapeInput(this.message)};;";
                case MailEncoding.SMTP:
                    return
                        $"SMTP:{this.mailReceiver}:{EscapeInput(this.subject, true)}:{EscapeInput(this.message, true)}";
                default:
                    return this.mailReceiver;
            }
        }

        public enum MailEncoding
        {
            MAILTO,
            MATMSG,
            SMTP
        }
    }
}