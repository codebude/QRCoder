using System;

namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a MMS (Multimedia Messaging Service) payload for QR codes.
    /// </summary>
    public class MMS : Payload
    {
        private readonly string number, subject;
        private readonly MMSEncoding encoding;

        /// <summary>
        /// Creates a MMS payload without text.
        /// </summary>
        /// <param name="number">Receiver phone number.</param>
        /// <param name="encoding">Encoding type.</param>
        public MMS(string number, MMSEncoding encoding = MMSEncoding.MMS)
        {
            this.number = number;
            this.subject = string.Empty;
            this.encoding = encoding;
        }

        /// <summary>
        /// Creates a MMS payload with text (subject).
        /// </summary>
        /// <param name="number">Receiver phone number.</param>
        /// <param name="subject">Text of the MMS.</param>
        /// <param name="encoding">Encoding type.</param>
        public MMS(string number, string subject, MMSEncoding encoding = MMSEncoding.MMS)
        {
            this.number = number;
            this.subject = subject;
            this.encoding = encoding;
        }

        /// <summary>
        /// Returns the MMS payload as a string.
        /// </summary>
        /// <returns>The MMS payload as a string.</returns>
        public override string ToString()
        {
            var returnVal = string.Empty;
            switch (this.encoding)
            {
                case MMSEncoding.MMSTO:
                    var queryStringMmsTo = string.Empty;
                    if (!string.IsNullOrEmpty(this.subject))
                        queryStringMmsTo = $"?subject={Uri.EscapeDataString(this.subject)}";
                    returnVal = $"mmsto:{this.number}{queryStringMmsTo}";
                    break;
                case MMSEncoding.MMS:
                    var queryStringMms = string.Empty;
                    if (!string.IsNullOrEmpty(this.subject))
                        queryStringMms = $"?body={Uri.EscapeDataString(this.subject)}";
                    returnVal = $"mms:{this.number}{queryStringMms}";
                    break;
            }
            return returnVal;
        }

        /// <summary>
        /// Defines the encoding types for the MMS payload.
        /// </summary>
        public enum MMSEncoding
        {
            /// <summary>
            /// Uses the "mms:" URI scheme.
            /// </summary>
            MMS,

            /// <summary>
            /// Uses the "mmsto:" URI scheme.
            /// </summary>
            MMSTO
        }
    }
}
