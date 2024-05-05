namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        /// <summary>
        /// Defines the levels of error correction available in QR codes.
        /// Each level specifies the proportion of data that can be recovered if the QR code is partially obscured or damaged.
        /// </summary>
        public enum ECCLevel
        {
            /// <summary>
            /// Level L: Low error correction (approximately 7% of data can be recovered).
            /// This level allows the highest data density.
            /// </summary>
            L,

            /// <summary>
            /// Level M: Medium error correction (approximately 15% of data can be recovered).
            /// Offers a balance between data capacity and error recovery.
            /// </summary>
            M,

            /// <summary>
            /// Level Q: Quartile error correction (approximately 25% of data can be recovered).
            /// More robust error correction at the cost of reduced data capacity.
            /// </summary>
            Q,

            /// <summary>
            /// Level H: High error correction (approximately 30% of data can be recovered).
            /// Provides the highest level of error recovery, ideal for environments with high risk of data loss.
            /// </summary>
            H
        }
    }
}
