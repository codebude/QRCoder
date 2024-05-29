using System;
using System.Text.RegularExpressions;

namespace QRCoder
{
    public static partial class PayloadGenerator
    {
        public class WhatsAppMessage : Payload
        {
            private readonly string number, message;

            /// <summary>
            /// Let's you compose a WhatApp message and send it the receiver number.
            /// </summary>
            /// <param name="number">Receiver phone number where the <number> is a full phone number in international format. 
            /// Omit any zeroes, brackets, or dashes when adding the phone number in international format.
            /// Use: 1XXXXXXXXXX | Don't use: +001-(XXX)XXXXXXX
            /// </param>
            /// <param name="message">The message</param>
            public WhatsAppMessage(string number, string message)
            {
                this.number = number;
                this.message = message;
            }

            /// <summary>
            /// Let's you compose a WhatApp message. When scanned the user is asked to choose a contact who will receive the message.
            /// </summary>
            /// <param name="message">The message</param>
            public WhatsAppMessage(string message)
            {
                this.number = string.Empty;
                this.message = message;
            }

            public override string ToString()
            {
                var cleanedPhone = Regex.Replace(this.number, @"^[0+]+|[ ()-]", string.Empty);
                return ($"https://wa.me/{cleanedPhone}?text={Uri.EscapeDataString(message)}");
            }
        }
    }
}
