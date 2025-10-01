namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a payload for Litecoin payment addresses.
    /// </summary>
    public class LitecoinAddress : BitcoinLikeCryptoCurrencyAddress
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LitecoinAddress"/> class.
        /// Generates a Litecoin payment payload. QR Codes with this payload can open a payment app.
        /// </summary>
        /// <param name="address">The Litecoin address of the payment receiver.</param>
        /// <param name="amount">The amount of Litecoin to transfer.</param>
        /// <param name="label">A reference label.</param>
        /// <param name="message">A reference text or message.</param>
        public LitecoinAddress(string address, double? amount, string? label = null, string? message = null)
            : base(BitcoinLikeCryptoCurrencyType.Litecoin, address, amount, label, message) { }
    }
}
