namespace QRCoder
{
    public static partial class PayloadGenerator
    {
        public class LitecoinAddress : BitcoinLikeCryptoCurrencyAddress
        {
            public LitecoinAddress(string address, double? amount, string? label = null, string? message = null)
                : base(BitcoinLikeCryptoCurrencyType.Litecoin, address, amount, label, message) { }
        }
    }
}
