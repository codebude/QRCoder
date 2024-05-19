using System.Collections.Generic;

namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private static partial class ModulePlacer
        {
            /// <summary>
            /// Struct that represents blocked modules using rectangles.
            /// </summary>
            public struct BlockedModules
            {
                private readonly List<Rectangle> _blockedModules;

                /// <summary>
                /// Initializes a new instance of the <see cref="BlockedModules"/> struct with a specified capacity.
                /// </summary>
                /// <param name="capacity">The initial capacity of the blocked modules list.</param>
                public BlockedModules(int capacity)
                {
                    _blockedModules = new List<Rectangle>(capacity);
                }

                /// <summary>
                /// Adds a blocked module at the specified coordinates.
                /// </summary>
                /// <param name="x">The x-coordinate of the module.</param>
                /// <param name="y">The y-coordinate of the module.</param>
                public void Add(int x, int y)
                {
                    _blockedModules.Add(new Rectangle(x, y, 1, 1));
                }

                /// <summary>
                /// Adds a blocked module defined by the specified rectangle.
                /// </summary>
                /// <param name="rect">The rectangle that defines the blocked module.</param>
                public void Add(Rectangle rect)
                {
                    _blockedModules.Add(rect);
                }

                /// <summary>
                /// Checks if the specified coordinates are blocked.
                /// </summary>
                /// <param name="x">The x-coordinate to check.</param>
                /// <param name="y">The y-coordinate to check.</param>
                /// <returns><c>true</c> if the coordinates are blocked; otherwise, <c>false</c>.</returns>
                public bool IsBlocked(int x, int y)
                {
                    return IsBlocked(new Rectangle(x, y, 1, 1));
                }

                /// <summary>
                /// Checks if the specified rectangle is blocked.
                /// </summary>
                /// <param name="r1">The rectangle to check.</param>
                /// <returns><c>true</c> if the rectangle is blocked; otherwise, <c>false</c>.</returns>
                public bool IsBlocked(Rectangle r1)
                {
                    // Iterate through the list of blocked modules to check for any intersection.
                    foreach (var r2 in _blockedModules)
                    {
                        // Check if any part of the rectangles overlap.
                        if (r2.X < r1.X + r1.Width && r1.X < r2.X + r2.Width && r2.Y < r1.Y + r1.Height && r1.Y < r2.Y + r2.Height)
                            return true;
                    }
                    return false;
                }
            }
        }
    }
}
