using System;
using System.Collections.Generic;
using System.Linq;

namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates an email payload that can be used to create a QR code for sending an email.
    /// </summary>
    public class Mail : Payload
    {
        private readonly string? mailReceiver, subject, message;
        private readonly MailEncoding encoding;


        /// <summary>
        /// Creates an email payload with subject and message/text.
        /// </summary>
        /// <param name="mailReceiver">Receiver's email address.</param>
        /// <param name="subject">Subject line of the email.</param>
        /// <param name="message">Message content of the email.</param>
        /// <param name="encoding">Payload encoding type. Choose dependent on your QR Code scanner app.</param>
        public Mail(string? mailReceiver = null, string? subject = null, string? message = null, MailEncoding encoding = MailEncoding.MAILTO)
        {
            this.mailReceiver = mailReceiver;
            this.subject = subject;
            this.message = message;
            this.encoding = encoding;
        }

        /// <summary>
        /// Returns the email payload as a string.
        /// </summary>
        /// <returns>The email payload as a string.</returns>
        public override string ToString()
        {
            var returnVal = string.Empty;
            switch (this.encoding)
            {
                case MailEncoding.MAILTO:
                    var parts = new List<string>();
                    if (!string.IsNullOrEmpty(this.subject))
                        parts.Add("subject=" + Uri.EscapeDataString(this.subject));
                    if (!string.IsNullOrEmpty(this.message))
                        parts.Add("body=" + Uri.EscapeDataString(this.message));
                    var queryString = parts.Any() ? $"?{string.Join("&", parts.ToArray())}" : "";
                    returnVal = $"mailto:{this.mailReceiver}{queryString}";
                    break;
                case MailEncoding.MATMSG:
                    returnVal = $"MATMSG:TO:{this.mailReceiver};SUB:{EscapeInput(this.subject ?? "")};BODY:{EscapeInput(this.message ?? "")};;";
                    break;
                case MailEncoding.SMTP:
                    returnVal = $"SMTP:{this.mailReceiver}:{EscapeInput(this.subject ?? "", true)}:{EscapeInput(this.message ?? "", true)}";
                    break;
            }
            return returnVal;
        }

        /// <summary>
        /// Defines the encoding types for the email payload.
        /// </summary>
        public enum MailEncoding
        {
            /// <summary>
            /// Uses the "mailto:" URI scheme.
            /// </summary>
            MAILTO,

            /// <summary>
            /// Uses the "MATMSG:" format.
            /// </summary>
            MATMSG,

            /// <summary>
            /// Uses the "SMTP:" format.
            /// </summary>
            SMTP
        }
    }
}
