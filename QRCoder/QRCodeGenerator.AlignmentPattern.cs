using System.Collections.Generic;

namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private struct AlignmentPattern
        {
            public int Version;
            public List<Point> PatternPositions;
        }
    }
}
