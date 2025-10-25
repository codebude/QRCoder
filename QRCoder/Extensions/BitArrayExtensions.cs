namespace QRCoder;

/// <summary>
/// Helper methods for <see cref="BitArray"/>.
/// </summary>
internal static class BitArrayExtensions
{
    /// <summary>
    /// Copies a specified number of elements from one <see cref="BitArray"/> to another starting at the specified offsets.
    /// </summary>
    /// <param name="source">The source <see cref="BitArray"/> from which elements will be copied.</param>
    /// <param name="sourceOffset">The zero-based index in the source <see cref="BitArray"/> at which copying begins.</param>
    /// <param name="destination">The destination <see cref="BitArray"/> to which elements will be copied.</param>
    /// <param name="destinationOffset">The zero-based index in the destination <see cref="BitArray"/> at which storing begins.</param>
    /// <param name="count">The number of elements to copy.</param>
    /// <returns>The index in the destination <see cref="BitArray"/> immediately following the last copied element.</returns>
    public static int CopyTo(this BitArray source, BitArray destination, int sourceOffset, int destinationOffset, int count)
    {
        for (int i = 0; i < count; i++)
        {
            destination[destinationOffset + i] = source[sourceOffset + i];
        }
        return destinationOffset + count;
    }

    public static void ShiftTowardsBit0(this BitArray fStrEcc, int num)
    {
#if !NETSTANDARD2_0
        fStrEcc.RightShift(num); // Shift towards bit 0
#else
        for (var i = 0; i < fStrEcc.Length - num; i++)
            fStrEcc[i] = fStrEcc[i + num];
        for (var i = fStrEcc.Length - num; i < fStrEcc.Length; i++)
            fStrEcc[i] = false;
#endif
    }

    public static void ShiftAwayFromBit0(this BitArray fStrEcc, int num)
    {
#if !NETSTANDARD2_0
        fStrEcc.LeftShift(num); // Shift away from bit 0
#else
        for (var i = fStrEcc.Length - 1; i >= num; i--)
            fStrEcc[i] = fStrEcc[i - num];
        for (var i = 0; i < num; i++)
            fStrEcc[i] = false;
#endif
    }
}
