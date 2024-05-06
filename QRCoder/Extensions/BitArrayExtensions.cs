using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace QRCoder
{
    /// <summary>
    /// Helper methods for <see cref="BitArray"/>.
    /// </summary>
    internal static class BitArrayExtensions
    {
        /// Copies a specified number of elements from one <see cref="BitArray"/> to another starting at the specified offsets.
        /// </summary>
        /// <param name="source">The source <see cref="BitArray"/> from which elements will be copied.</param>
        /// <param name="sourceOffset">The zero-based index in the source <see cref="BitArray"/> at which copying begins.</param>
        /// <param name="destination">The destination <see cref="BitArray"/> to which elements will be copied.</param>
        /// <param name="destinationOffset">The zero-based index in the destination <see cref="BitArray"/> at which storing begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        /// <returns>The index in the destination <see cref="BitArray"/> immediately following the last copied element.</returns>
        public static int CopyTo(this BitArray source, int sourceOffset, BitArray destination, int destinationOffset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                destination[destinationOffset + i] = source[sourceOffset + i];
            }
            return destinationOffset + count;
        }

        /// <summary>
        /// Copies a specified number of elements from one <see cref="BitArray"/> to another starting at the specified offsets,
        /// and reverses every set of 8 bits during the copy operation.
        /// </summary>
        /// <param name="source">The source <see cref="BitArray"/> from which elements will be copied.</param>
        /// <param name="sourceOffset">The zero-based index in the source <see cref="BitArray"/> at which copying begins.</param>
        /// <param name="destination">The destination <see cref="BitArray"/> to which elements will be copied.</param>
        /// <param name="destinationOffset">The zero-based index in the destination <see cref="BitArray"/> at which storing begins.</param>
        /// <param name="count">The number of elements to copy. Must be a multiple of 8.</param>
        /// <returns>The index in the destination <see cref="BitArray"/> immediately following the last copied element.</returns>
        public static int CopyToRev8(this BitArray source, int sourceOffset, BitArray destination, int destinationOffset, int count)
        {
            if (count % 8 != 0)
            {
                throw new ArgumentException("Count must be a multiple of 8.", nameof(count));
            }

            for (int i = 0; i < count; i += 8)
            {
                // Reverse the current set of 8 bits
                for (int j = 0; j < 8; j++)
                {
                    destination[destinationOffset + i + j] = source[sourceOffset + i + (7 - j)];
                }
            }

            return destinationOffset + count;
        }

    }
}
