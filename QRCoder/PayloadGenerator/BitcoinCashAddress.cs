namespace QRCoder
{
    public static partial class PayloadGenerator
    {
        public class BitcoinCashAddress : BitcoinLikeCryptoCurrencyAddress
        {
            public BitcoinCashAddress(string address, double? amount, string? label = null, string? message = null)
                : base(BitcoinLikeCryptoCurrencyType.BitcoinCash, address, amount, label, message) { }
        }
    }
}
