using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a payload for Bitcoin-like cryptocurrency payment addresses.
    /// </summary>
    public class BitcoinLikeCryptoCurrencyAddress : Payload
    {
        private readonly BitcoinLikeCryptoCurrencyType currencyType;
        private readonly string address;
        private readonly string? label, message;
        private readonly double? amount;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitcoinLikeCryptoCurrencyAddress"/> class.
        /// Generates a Bitcoin-like cryptocurrency payment payload. QR Codes with this payload can open a payment app.
        /// </summary>
        /// <param name="currencyType">The type of Bitcoin-like cryptocurrency.</param>
        /// <param name="address">The cryptocurrency address of the payment receiver.</param>
        /// <param name="amount">The amount of coins to transfer.</param>
        /// <param name="label">A reference label.</param>
        /// <param name="message">A reference text or message.</param>           
        public BitcoinLikeCryptoCurrencyAddress(BitcoinLikeCryptoCurrencyType currencyType, string address, double? amount, string? label = null, string? message = null)
        {
            this.currencyType = currencyType;
            this.address = address;

            if (!string.IsNullOrEmpty(label))
            {
                this.label = Uri.EscapeDataString(label);
            }

            if (!string.IsNullOrEmpty(message))
            {
                this.message = Uri.EscapeDataString(message);
            }

            this.amount = amount;
        }

        /// <summary>
        /// Returns a string representation of the Bitcoin-like cryptocurrency address payload.
        /// </summary>
        /// <returns>A string representation of the cryptocurrency address payload.</returns>
        public override string ToString()
        {
            string? query = null;

            var queryValues = new KeyValuePair<string, string?>[]{
              new KeyValuePair<string, string?>(nameof(label), label),
              new KeyValuePair<string, string?>(nameof(message), message),
              new KeyValuePair<string, string?>(nameof(amount), amount.HasValue ? amount.Value.ToString("#.########", CultureInfo.InvariantCulture) : null)
            };

            if (queryValues.Any(keyPair => !string.IsNullOrEmpty(keyPair.Value)))
            {
                query = "?" + string.Join("&", queryValues
                    .Where(keyPair => !string.IsNullOrEmpty(keyPair.Value))
                    .Select(keyPair => $"{keyPair.Key}={keyPair.Value}")
                    .ToArray());
            }

            return $"{Enum.GetName(typeof(BitcoinLikeCryptoCurrencyType), currencyType)!.ToLower()}:{address}{query}";
        }

        /// <summary>
        /// Enumerates Bitcoin-like cryptocurrencies.
        /// </summary>
        public enum BitcoinLikeCryptoCurrencyType
        {
            /// <summary>
            /// Bitcoin.
            /// </summary>
            Bitcoin,

            /// <summary>
            /// Bitcoin Cash.
            /// </summary>
            BitcoinCash,

            /// <summary>
            /// Litecoin.
            /// </summary>
            Litecoin
        }
    }
}
