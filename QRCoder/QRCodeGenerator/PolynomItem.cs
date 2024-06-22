namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Represents an individual term of a polynomial, consisting of a coefficient and an exponent.
    /// For example, the term 3x² would be represented as a <see cref="PolynomItem"/> with a coefficient of 3 and an exponent of 2.
    /// </summary>
    private struct PolynomItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolynomItem"/> struct with the specified coefficient and exponent.
        /// </summary>
        /// <param name="coefficient">The coefficient of the polynomial term. For example, in the term 3x², the coefficient is 3.</param>
        /// <param name="exponent">The exponent of the polynomial term. For example, in the term 3x², the exponent is 2.</param>
        public PolynomItem(int coefficient, int exponent)
        {
            Coefficient = coefficient;
            Exponent = exponent;
        }

        /// <summary>
        /// Gets the coefficient of the polynomial term.
        /// </summary>
        public int Coefficient { get; }

        /// <summary>
        /// Gets the exponent of the polynomial term.
        /// </summary>
        public int Exponent { get; }
    }
}
