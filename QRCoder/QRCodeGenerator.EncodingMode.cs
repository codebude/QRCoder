namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private enum EncodingMode
        {
            Numeric = 1,
            Alphanumeric = 2,
            Byte = 4,
            Kanji = 8,
            ECI = 7
        }
    }
}
