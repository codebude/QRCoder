using System.Reflection;

namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Represents a 2D point with integer coordinates.
    /// </summary>
    private readonly struct Point : IEquatable<Point>
    {
        /// <summary>
        /// Gets the X-coordinate of the point.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the Y-coordinate of the point.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> struct with specified X and Y coordinates.
        /// </summary>
        /// <param name="x">The X-coordinate of the point.</param>
        /// <param name="y">The Y-coordinate of the point.</param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Point"/> is equal to the current <see cref="Point"/>.
        /// </summary>
        /// <param name="other">The <see cref="Point"/> to compare with the current <see cref="Point"/>.</param>
        /// <returns>True if the specified <see cref="Point"/> has the same X and Y coordinates as the current <see cref="Point"/>; otherwise, false.</returns>
        /// <remarks>
        /// If this method which implements <see cref="IEquatable{T}.Equals(T)"/> is not implemented, comparisons used by methods such as <see cref="List{T}.Contains(T)"/>
        /// fall back to reflection, which causes heap allocations internally during the calls to <see cref="FieldInfo.GetValue(object)"/>.
        /// </remarks>
        public bool Equals(Point other)
            => X == other.X && Y == other.Y;

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Point point && Equals(point);

        /// <inheritdoc/>
        public override int GetHashCode()
#if NET5_0_OR_GREATER
            => HashCode.Combine(X, Y);
#else
            => X ^ (int)(((uint)Y << 16) | ((uint)Y >> 16));
#endif
    }
}
