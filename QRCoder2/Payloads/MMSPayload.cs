namespace QRCoder2.Payloads
{
    public class MMSPayload : PayloadBase
    {
        private readonly string number, subject;
        private readonly MMSEncoding encoding;

        /// <summary>
        /// Creates a MMS payload without text
        /// </summary>
        /// <param name="number">Receiver phone number</param>
        /// <param name="encoding">Encoding type</param>
        public MMSPayload(string number, MMSEncoding encoding = MMSEncoding.MMS)
        {
            this.number = number;
            this.subject = string.Empty;
            this.encoding = encoding;
        }

        /// <summary>
        /// Creates a MMS payload with text (subject)
        /// </summary>
        /// <param name="number">Receiver phone number</param>
        /// <param name="subject">Text of the MMS</param>
        /// <param name="encoding">Encoding type</param>
        public MMSPayload(string number, string subject, MMSEncoding encoding = MMSEncoding.MMS)
        {
            this.number = number;
            this.subject = subject;
            this.encoding = encoding;
        }

        public override string ToString()
        {
            switch (this.encoding)
            {
                case MMSEncoding.MMSTO:
                    return $"mmsto:{this.number}?subject={System.Uri.EscapeDataString(this.subject)}";
                case MMSEncoding.MMS:
                    return $"mms:{this.number}?body={System.Uri.EscapeDataString(this.subject)}";
                default:
                    return "mms:";
            }
        }

        public enum MMSEncoding
        {
            MMS,
            MMSTO
        }
    }
}