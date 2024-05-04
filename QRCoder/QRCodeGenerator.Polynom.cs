using System.Collections.Generic;
using System.Text;

namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        /// <summary>
        /// Represents a polynomial, which is a sum of polynomial terms.
        /// </summary>
        private struct Polynom
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Polynom"/> struct with a specified number of initial capacity for polynomial terms.
            /// </summary>
            /// <param name="count">The initial capacity of the polynomial items list.</param>
            public Polynom(int count)
            {
                this.PolyItems = new List<PolynomItem>(count);
            }

            /// <summary>
            /// Gets or sets the list of polynomial items, where each item represents a term in the polynomial.
            /// </summary>
            public List<PolynomItem> PolyItems { get; set; }

            /// <summary>
            /// Returns a string that represents the polynomial in standard algebraic notation.
            /// Example output: "a^2*x^3 + a^5*x^1 + a^3*x^0", which represents the polynomial 2x³ + 5x + 3.
            /// </summary>
            /// <returns>A string representation of the polynomial, formatted as a sum of terms like 'a^coefficient*x^exponent + ...'.</returns>
            public override string ToString()
            {
                var sb = new StringBuilder();

                foreach (var polyItem in this.PolyItems)
                {
                    sb.Append($"a^{polyItem.Coefficient}*x^{polyItem.Exponent} + ");
                }

                // Remove the trailing " + " if the string builder has added terms
                if (sb.Length > 0)
                    sb.Length -= 3;

                return sb.ToString();
            }
        }

    }
}
