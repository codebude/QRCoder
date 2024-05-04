namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private readonly struct Rectangle
        {
            public int X { get; }
            public int Y { get; }
            public int Width { get; }
            public int Height { get; }

            public Rectangle(int x, int y, int w, int h)
            {
                this.X = x;
                this.Y = y;
                this.Width = w;
                this.Height = h;
            }
        }
    }
}
