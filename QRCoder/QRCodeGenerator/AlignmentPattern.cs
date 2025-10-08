namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Represents the alignment pattern used in QR codes, which helps ensure the code remains readable even if it is somewhat distorted.
    /// Each QR code version has its own specific alignment pattern locations which this struct encapsulates.
    /// </summary>
    private struct AlignmentPattern
    {
        /// <summary>
        /// The version of the QR code. Higher versions have more complex and numerous alignment patterns.
        /// </summary>
        public int Version;

        /// <summary>
        /// A list of points where alignment patterns are located within the QR code matrix.
        /// Each point represents the center of an alignment pattern.
        /// </summary>
        public List<Point> PatternPositions;
    }
}
