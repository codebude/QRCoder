namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Represents a rectangle defined by its top-left corner's coordinates, width, and height.
    /// </summary>
    private readonly struct Rectangle
    {
        /// <summary>
        /// Gets the X-coordinate of the top-left corner of the rectangle.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the Y-coordinate of the top-left corner of the rectangle.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Gets the width of the rectangle.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the height of the rectangle.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct with the specified top-left corner coordinates, width, and height.
        /// </summary>
        /// <param name="x">The X-coordinate of the top-left corner of the rectangle.</param>
        /// <param name="y">The Y-coordinate of the top-left corner of the rectangle.</param>
        /// <param name="w">The width of the rectangle.</param>
        /// <param name="h">The height of the rectangle.</param>
        public Rectangle(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }
    }
}
