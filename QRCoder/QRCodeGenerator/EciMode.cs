#pragma warning disable CA1707 // Underscore in identifier

namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Enumerates the Extended Channel Interpretation (ECI) modes used in QR codes to handle different character encoding standards.
    /// ECI mode allows QR codes to efficiently encode data using character sets other than the default ISO-8859-1.
    /// </summary>
    public enum EciMode
    {
        /// <summary>
        /// Default encoding mode (typically ISO-8859-1). Used when no ECI mode is explicitly specified.
        /// This mode is assumed in basic QR codes where no extended character sets are needed.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Specifies the use of the ISO-8859-1 character set, covering most Western European languages.
        /// This mode explicitly sets the encoding to ISO-8859-1, which includes characters used in languages such as English, French, German, and Spanish.
        /// </summary>
        Iso8859_1 = 3,

        /// <summary>
        /// Specifies the use of the ISO-8859-2 character set, which is primarily used for Central and Eastern European languages.
        /// This includes characters used in languages such as Polish, Czech, Slovak, Hungarian, and Romanian.
        /// </summary>
        Iso8859_2 = 4,

        /// <summary>
        /// Specifies the use of UTF-8 encoding.
        /// UTF-8 can encode any Unicode character and is useful for QR codes that need to support multi-language content.
        /// </summary>
        Utf8 = 26
    }
}
