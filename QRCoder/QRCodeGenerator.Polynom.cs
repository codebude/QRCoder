using System.Collections.Generic;
using System.Text;

namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private struct Polynom
        {
            public Polynom(int count)
            {
                this.PolyItems = new List<PolynomItem>(count);
            }

            public List<PolynomItem> PolyItems { get; set; }

            public override string ToString()
            {
                var sb = new StringBuilder();

                foreach (var polyItem in this.PolyItems)
                {
                    sb.Append("a^" + polyItem.Coefficient + "*x^" + polyItem.Exponent + " + ");
                }

                if (sb.Length > 0)
                    sb.Length -= 3;

                return sb.ToString();
            }
        }
    }
}
