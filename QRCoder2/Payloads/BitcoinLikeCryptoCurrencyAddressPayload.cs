using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace QRCoder2.Payloads
{
    public class BitcoinLikeCryptoCurrencyAddressPayload : PayloadBase
    {
        private readonly BitcoinLikeCryptoCurrencyType currencyType;
        private readonly string address, label, message;
        private readonly double? amount;

        /// <summary>
        /// Generates a Bitcoin like cryptocurrency payment payload. QR Codes with this payload can open a payment app.
        /// </summary>
        /// <param name="currencyType">Bitcoin like cryptocurrency address of the payment receiver</param>
        /// <param name="address">Bitcoin like cryptocurrency address of the payment receiver</param>
        /// <param name="amount">Amount of coins to transfer</param>
        /// <param name="label">Reference label</param>
        /// <param name="message">Referece text aka message</param>
        public BitcoinLikeCryptoCurrencyAddressPayload(BitcoinLikeCryptoCurrencyType currencyType, string address, double? amount, string label = null, string message = null)
        {
            this.currencyType = currencyType;
            this.address = address;

            if (!string.IsNullOrEmpty(label))
            {
                this.label = Uri.EscapeUriString(label);
            }

            if (!string.IsNullOrEmpty(message))
            {
                this.message = Uri.EscapeUriString(message);
            }

            this.amount = amount;
        }

        public override string ToString()
        {
            string query = null;

            var queryValues = new KeyValuePair<string,string>[]{
              new KeyValuePair<string, string>(nameof(label), label),
              new KeyValuePair<string, string>(nameof(message), message),
              new KeyValuePair<string, string>(nameof(amount), amount.HasValue ? amount.Value.ToString("#.########", CultureInfo.InvariantCulture) : null)
            };

            if (queryValues.Any(keyPair => !string.IsNullOrEmpty(keyPair.Value)))
            {
                query = "?" + string.Join("&", queryValues
                    .Where(keyPair => !string.IsNullOrEmpty(keyPair.Value))
                    .Select(keyPair => $"{keyPair.Key}={keyPair.Value}")
                    .ToArray());
            }

            return $"{Enum.GetName(typeof(BitcoinLikeCryptoCurrencyType), currencyType).ToLower()}:{address}{query}";
        }

        public enum BitcoinLikeCryptoCurrencyType
        {
            Bitcoin,
            BitcoinCash,
            Litecoin
        }
    }
    
    public class BitcoinAddressPayload : BitcoinLikeCryptoCurrencyAddressPayload
    {
        public BitcoinAddressPayload(string address, double? amount, string label = null, string message = null)
            : base(BitcoinLikeCryptoCurrencyType.Bitcoin, address, amount, label, message) { }
    }

    public class BitcoinCashAddressPayload : BitcoinLikeCryptoCurrencyAddressPayload
    {
        public BitcoinCashAddressPayload(string address, double? amount, string label = null, string message = null)
            : base(BitcoinLikeCryptoCurrencyType.BitcoinCash, address, amount, label, message) { }
    }

    public class LitecoinAddressPayload : BitcoinLikeCryptoCurrencyAddressPayload
    {
        public LitecoinAddressPayload(string address, double? amount, string label = null, string message = null)
            : base(BitcoinLikeCryptoCurrencyType.Litecoin, address, amount, label, message) { }
    }
}