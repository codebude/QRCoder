using System;
using System.Text.RegularExpressions;

namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a WhatsApp message payload for QR codes.
    /// </summary>
    public class WhatsAppMessage : Payload
    {
        private readonly string number, message;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhatsAppMessage"/> class with a receiver number and message.
        /// </summary>
        /// <param name="number">
        /// Receiver phone number in full international format. 
        /// Omit any zeroes, brackets, or dashes. Use format: 1XXXXXXXXXX. Don't use: +001-(XXX)XXXXXXX.
        /// </param>
        /// <param name="message">The message to be sent.</param>
        public WhatsAppMessage(string number, string message)
        {
            this.number = number;
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WhatsAppMessage"/> class with a message only.
        /// When scanned, the user is asked to choose a contact to receive the message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        public WhatsAppMessage(string message)
        {
            this.number = string.Empty;
            this.message = message;
        }

        /// <summary>
        /// Returns the WhatsApp message payload as a string.
        /// </summary>
        /// <returns>The WhatsApp message URL as a string.</returns>
        public override string ToString()
        {
            var cleanedPhone = Regex.Replace(this.number, @"^[0+]+|[ ()-]", string.Empty);
            return ($"https://wa.me/{cleanedPhone}?text={Uri.EscapeDataString(message)}");
        }
    }
}
