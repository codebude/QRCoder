using System;

namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a MMS (Multimedia Messaging Service) payload for QR codes.
    /// </summary>
    public class MMS : Payload
    {
        private readonly string _number, _subject;
        private readonly MMSEncoding _encoding;

        /// <summary>
        /// Creates a MMS payload without text.
        /// </summary>
        /// <param name="number">Receiver phone number.</param>
        /// <param name="encoding">Encoding type.</param>
        public MMS(string number, MMSEncoding encoding = MMSEncoding.MMS)
        {
            this._number = number;
            _subject = string.Empty;
            this._encoding = encoding;
        }

        /// <summary>
        /// Creates a MMS payload with text (subject).
        /// </summary>
        /// <param name="number">Receiver phone number.</param>
        /// <param name="subject">Text of the MMS.</param>
        /// <param name="encoding">Encoding type.</param>
        public MMS(string number, string subject, MMSEncoding encoding = MMSEncoding.MMS)
        {
            this._number = number;
            this._subject = subject;
            this._encoding = encoding;
        }

        /// <summary>
        /// Returns the MMS payload as a string.
        /// </summary>
        /// <returns>The MMS payload as a string.</returns>
        public override string ToString()
        {
            var returnVal = string.Empty;
            switch (_encoding)
            {
                case MMSEncoding.MMSTO:
                    var queryStringMmsTo = string.Empty;
                    if (!string.IsNullOrEmpty(_subject))
                        queryStringMmsTo = $"?subject={Uri.EscapeDataString(_subject)}";
                    returnVal = $"mmsto:{_number}{queryStringMmsTo}";
                    break;
                case MMSEncoding.MMS:
                    var queryStringMms = string.Empty;
                    if (!string.IsNullOrEmpty(_subject))
                        queryStringMms = $"?body={Uri.EscapeDataString(_subject)}";
                    returnVal = $"mms:{_number}{queryStringMms}";
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
