using System;
using System.Collections;

namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private static partial class ModulePlacer
        {
            /// <summary>
            /// Struct that represents blocked modules using rectangles.
            /// </summary>
            public struct BlockedModules : IDisposable
            {
                private readonly bool[,] _blockedModules;

                [ThreadStatic]
                private static bool[,] _staticBlockedModules;

                /// <summary>
                /// Initializes a new instance of the <see cref="BlockedModules"/> struct with a specified capacity.
                /// </summary>
                /// <param name="capacity">The initial capacity of the blocked modules list.</param>
                public BlockedModules(int size)
                {
                    if (_staticBlockedModules != null && _staticBlockedModules.GetUpperBound(0) >= (size - 1))
                    {
                        _blockedModules = _staticBlockedModules;
                        _staticBlockedModules = null;
#if NET6_0_OR_GREATER
                        Array.Clear(_blockedModules);
#else
                        Array.Clear(_blockedModules, 0, _blockedModules.Length);
#endif
                    }
                    else
                    {
                        _blockedModules = new bool[size, size];
                    }
                }

                /// <summary>
                /// Adds a blocked module at the specified coordinates.
                /// </summary>
                /// <param name="x">The x-coordinate of the module.</param>
                /// <param name="y">The y-coordinate of the module.</param>
                public void Add(int x, int y)
                {
                    _blockedModules[y, x] = true;
                }

                /// <summary>
                /// Adds a blocked module defined by the specified rectangle.
                /// </summary>
                /// <param name="rect">The rectangle that defines the blocked module.</param>
                public void Add(Rectangle rect)
                {
                    for (int y = rect.Y; y < rect.Y + rect.Height; y++)
                    {
                        for (int x = rect.X; x < rect.X + rect.Width; x++)
                        {
                            _blockedModules[y, x] = true;
                        }
                    }
                }

                /// <summary>
                /// Checks if the specified coordinates are blocked.
                /// </summary>
                /// <param name="x">The x-coordinate to check.</param>
                /// <param name="y">The y-coordinate to check.</param>
                /// <returns><c>true</c> if the coordinates are blocked; otherwise, <c>false</c>.</returns>
                public bool IsBlocked(int x, int y)
                {
                    return _blockedModules[y, x];
                }

                /// <summary>
                /// Checks if the specified rectangle is blocked.
                /// </summary>
                /// <param name="r1">The rectangle to check.</param>
                /// <returns><c>true</c> if the rectangle is blocked; otherwise, <c>false</c>.</returns>
                public bool IsBlocked(Rectangle r1)
                {
                    for (int y = r1.Y; y < r1.Y + r1.Height; y++)
                    {
                        for (int x = r1.X; x < r1.X + r1.Width; x++)
                        {
                            if (_blockedModules[y, x])
                                return true;
                        }
                    }
                    return false;
                }

                public void Dispose()
                {
                    if (_staticBlockedModules == null || _staticBlockedModules.GetUpperBound(0) < _blockedModules.GetUpperBound(0))
                    {
                        _staticBlockedModules = _blockedModules;
                    }
                }
            }
        }
    }
}
