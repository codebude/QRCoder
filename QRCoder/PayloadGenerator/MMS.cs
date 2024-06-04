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
            subject = string.Empty;
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
            switch (encoding)
            {
                case MMSEncoding.MMSTO:
                    var queryStringMmsTo = string.Empty;
                    if (!string.IsNullOrEmpty(subject))
                        queryStringMmsTo = $"?subject={Uri.EscapeDataString(subject)}";
                    returnVal = $"mmsto:{number}{queryStringMmsTo}";
                    break;
                case MMSEncoding.MMS:
                    var queryStringMms = string.Empty;
                    if (!string.IsNullOrEmpty(subject))
                        queryStringMms = $"?body={Uri.EscapeDataString(subject)}";
                    returnVal = $"mms:{number}{queryStringMms}";
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
