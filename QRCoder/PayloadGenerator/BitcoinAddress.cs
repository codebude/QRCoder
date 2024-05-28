#if NETSTANDARD1_3
using System.Reflection;
#endif

namespace QRCoder
{
    public static partial class PayloadGenerator
    {
        public class BitcoinAddress : BitcoinLikeCryptoCurrencyAddress
        {
            public BitcoinAddress(string address, double? amount, string label = null, string message = null)
                : base(BitcoinLikeCryptoCurrencyType.Bitcoin, address, amount, label, message) { }
        }
    }
}
