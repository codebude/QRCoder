using System;

namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private readonly struct Point : IEquatable<Point>
        {
            public int X { get; }
            public int Y { get; }
            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public bool Equals(Point other)
            {
                return this.X == other.X && this.Y == other.Y;
            }

        }
    }
}
