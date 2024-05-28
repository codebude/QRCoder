using System;
#if NETSTANDARD1_3
using System.Reflection;
#endif

namespace QRCoder
{
    public static partial class PayloadGenerator
    {
        public class Url : Payload
        {
            private readonly string url;

            /// <summary>
            /// Generates a link. If the protocol is not specified, the http protocol will be added.
            /// </summary>
            /// <param name="url">Link url target</param>
            public Url(string url)
            {
                this.url = url;
            }

            public override string ToString()
            {
                return (!this.url.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? "http://" + this.url : this.url);
            }
        }
    }
}
