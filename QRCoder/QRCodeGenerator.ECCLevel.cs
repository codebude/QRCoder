namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        /// <summary>
        /// Error correction level. These define the tolerance levels for how much of the code can be lost before the code cannot be recovered.
        /// </summary>
        public enum ECCLevel
        {
            /// <summary>
            /// 7% may be lost before recovery is not possible
            /// </summary>
            L,
            /// <summary>
            /// 15% may be lost before recovery is not possible
            /// </summary>
            M,
            /// <summary>
            /// 25% may be lost before recovery is not possible
            /// </summary>
            Q,
            /// <summary>
            /// 30% may be lost before recovery is not possible
            /// </summary>
            H
        }
    }
}
