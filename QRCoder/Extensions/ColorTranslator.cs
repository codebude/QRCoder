#if !NETCOREAPP3_1_OR_GREATER

using System;
using System.Collections.Generic;
using System.Text;

namespace QRCoder;

internal static class ColorTranslator
{
    /// <summary>
    /// Dictionary of known HTML color names mapped to their RGB values.
    /// </summary>
    private static readonly Dictionary<string, System.Drawing.Color> _knownColors = new Dictionary<string, System.Drawing.Color>(StringComparer.OrdinalIgnoreCase)
    {
        { "AliceBlue", System.Drawing.Color.FromArgb(255, 240, 248, 255) },
        { "AntiqueWhite", System.Drawing.Color.FromArgb(255, 250, 235, 215) },
        { "Aqua", System.Drawing.Color.FromArgb(255, 0, 255, 255) },
        { "Aquamarine", System.Drawing.Color.FromArgb(255, 127, 255, 212) },
        { "Azure", System.Drawing.Color.FromArgb(255, 240, 255, 255) },
        { "Beige", System.Drawing.Color.FromArgb(255, 245, 245, 220) },
        { "Bisque", System.Drawing.Color.FromArgb(255, 255, 228, 196) },
        { "Black", System.Drawing.Color.FromArgb(255, 0, 0, 0) },
        { "BlanchedAlmond", System.Drawing.Color.FromArgb(255, 255, 235, 205) },
        { "Blue", System.Drawing.Color.FromArgb(255, 0, 0, 255) },
        { "BlueViolet", System.Drawing.Color.FromArgb(255, 138, 43, 226) },
        { "Brown", System.Drawing.Color.FromArgb(255, 165, 42, 42) },
        { "BurlyWood", System.Drawing.Color.FromArgb(255, 222, 184, 135) },
        { "CadetBlue", System.Drawing.Color.FromArgb(255, 95, 158, 160) },
        { "Chartreuse", System.Drawing.Color.FromArgb(255, 127, 255, 0) },
        { "Chocolate", System.Drawing.Color.FromArgb(255, 210, 105, 30) },
        { "Coral", System.Drawing.Color.FromArgb(255, 255, 127, 80) },
        { "CornflowerBlue", System.Drawing.Color.FromArgb(255, 100, 149, 237) },
        { "Cornsilk", System.Drawing.Color.FromArgb(255, 255, 248, 220) },
        { "Crimson", System.Drawing.Color.FromArgb(255, 220, 20, 60) },
        { "Cyan", System.Drawing.Color.FromArgb(255, 0, 255, 255) },
        { "DarkBlue", System.Drawing.Color.FromArgb(255, 0, 0, 139) },
        { "DarkCyan", System.Drawing.Color.FromArgb(255, 0, 139, 139) },
        { "DarkGoldenrod", System.Drawing.Color.FromArgb(255, 184, 134, 11) },
        { "DarkGray", System.Drawing.Color.FromArgb(255, 169, 169, 169) },
        { "DarkGrey", System.Drawing.Color.FromArgb(255, 169, 169, 169) },
        { "DarkGreen", System.Drawing.Color.FromArgb(255, 0, 100, 0) },
        { "DarkKhaki", System.Drawing.Color.FromArgb(255, 189, 183, 107) },
        { "DarkMagenta", System.Drawing.Color.FromArgb(255, 139, 0, 139) },
        { "DarkOliveGreen", System.Drawing.Color.FromArgb(255, 85, 107, 47) },
        { "DarkOrange", System.Drawing.Color.FromArgb(255, 255, 140, 0) },
        { "DarkOrchid", System.Drawing.Color.FromArgb(255, 153, 50, 204) },
        { "DarkRed", System.Drawing.Color.FromArgb(255, 139, 0, 0) },
        { "DarkSalmon", System.Drawing.Color.FromArgb(255, 233, 150, 122) },
        { "DarkSeaGreen", System.Drawing.Color.FromArgb(255, 143, 188, 143) },
        { "DarkSlateBlue", System.Drawing.Color.FromArgb(255, 72, 61, 139) },
        { "DarkSlateGray", System.Drawing.Color.FromArgb(255, 47, 79, 79) },
        { "DarkSlateGrey", System.Drawing.Color.FromArgb(255, 47, 79, 79) },
        { "DarkTurquoise", System.Drawing.Color.FromArgb(255, 0, 206, 209) },
        { "DarkViolet", System.Drawing.Color.FromArgb(255, 148, 0, 211) },
        { "DeepPink", System.Drawing.Color.FromArgb(255, 255, 20, 147) },
        { "DeepSkyBlue", System.Drawing.Color.FromArgb(255, 0, 191, 255) },
        { "DimGray", System.Drawing.Color.FromArgb(255, 105, 105, 105) },
        { "DimGrey", System.Drawing.Color.FromArgb(255, 105, 105, 105) },
        { "DodgerBlue", System.Drawing.Color.FromArgb(255, 30, 144, 255) },
        { "Firebrick", System.Drawing.Color.FromArgb(255, 178, 34, 34) },
        { "FloralWhite", System.Drawing.Color.FromArgb(255, 255, 250, 240) },
        { "ForestGreen", System.Drawing.Color.FromArgb(255, 34, 139, 34) },
        { "Fuchsia", System.Drawing.Color.FromArgb(255, 255, 0, 255) },
        { "Gainsboro", System.Drawing.Color.FromArgb(255, 220, 220, 220) },
        { "GhostWhite", System.Drawing.Color.FromArgb(255, 248, 248, 255) },
        { "Gold", System.Drawing.Color.FromArgb(255, 255, 215, 0) },
        { "Goldenrod", System.Drawing.Color.FromArgb(255, 218, 165, 32) },
        { "Gray", System.Drawing.Color.FromArgb(255, 128, 128, 128) },
        { "Grey", System.Drawing.Color.FromArgb(255, 128, 128, 128) },
        { "Green", System.Drawing.Color.FromArgb(255, 0, 128, 0) },
        { "GreenYellow", System.Drawing.Color.FromArgb(255, 173, 255, 47) },
        { "Honeydew", System.Drawing.Color.FromArgb(255, 240, 255, 240) },
        { "HotPink", System.Drawing.Color.FromArgb(255, 255, 105, 180) },
        { "IndianRed", System.Drawing.Color.FromArgb(255, 205, 92, 92) },
        { "Indigo", System.Drawing.Color.FromArgb(255, 75, 0, 130) },
        { "Ivory", System.Drawing.Color.FromArgb(255, 255, 255, 240) },
        { "Khaki", System.Drawing.Color.FromArgb(255, 240, 230, 140) },
        { "Lavender", System.Drawing.Color.FromArgb(255, 230, 230, 250) },
        { "LavenderBlush", System.Drawing.Color.FromArgb(255, 255, 240, 245) },
        { "LawnGreen", System.Drawing.Color.FromArgb(255, 124, 252, 0) },
        { "LemonChiffon", System.Drawing.Color.FromArgb(255, 255, 250, 205) },
        { "LightBlue", System.Drawing.Color.FromArgb(255, 173, 216, 230) },
        { "LightCoral", System.Drawing.Color.FromArgb(255, 240, 128, 128) },
        { "LightCyan", System.Drawing.Color.FromArgb(255, 224, 255, 255) },
        { "LightGoldenrodYellow", System.Drawing.Color.FromArgb(255, 250, 250, 210) },
        { "LightGray", System.Drawing.Color.FromArgb(255, 211, 211, 211) },
        { "LightGrey", System.Drawing.Color.FromArgb(255, 211, 211, 211) },
        { "LightGreen", System.Drawing.Color.FromArgb(255, 144, 238, 144) },
        { "LightPink", System.Drawing.Color.FromArgb(255, 255, 182, 193) },
        { "LightSalmon", System.Drawing.Color.FromArgb(255, 255, 160, 122) },
        { "LightSeaGreen", System.Drawing.Color.FromArgb(255, 32, 178, 170) },
        { "LightSkyBlue", System.Drawing.Color.FromArgb(255, 135, 206, 250) },
        { "LightSlateGray", System.Drawing.Color.FromArgb(255, 119, 136, 153) },
        { "LightSlateGrey", System.Drawing.Color.FromArgb(255, 119, 136, 153) },
        { "LightSteelBlue", System.Drawing.Color.FromArgb(255, 176, 196, 222) },
        { "LightYellow", System.Drawing.Color.FromArgb(255, 255, 255, 224) },
        { "Lime", System.Drawing.Color.FromArgb(255, 0, 255, 0) },
        { "LimeGreen", System.Drawing.Color.FromArgb(255, 50, 205, 50) },
        { "Linen", System.Drawing.Color.FromArgb(255, 250, 240, 230) },
        { "Magenta", System.Drawing.Color.FromArgb(255, 255, 0, 255) },
        { "Maroon", System.Drawing.Color.FromArgb(255, 128, 0, 0) },
        { "MediumAquamarine", System.Drawing.Color.FromArgb(255, 102, 205, 170) },
        { "MediumBlue", System.Drawing.Color.FromArgb(255, 0, 0, 205) },
        { "MediumOrchid", System.Drawing.Color.FromArgb(255, 186, 85, 211) },
        { "MediumPurple", System.Drawing.Color.FromArgb(255, 147, 112, 219) },
        { "MediumSeaGreen", System.Drawing.Color.FromArgb(255, 60, 179, 113) },
        { "MediumSlateBlue", System.Drawing.Color.FromArgb(255, 123, 104, 238) },
        { "MediumSpringGreen", System.Drawing.Color.FromArgb(255, 0, 250, 154) },
        { "MediumTurquoise", System.Drawing.Color.FromArgb(255, 72, 209, 204) },
        { "MediumVioletRed", System.Drawing.Color.FromArgb(255, 199, 21, 133) },
        { "MidnightBlue", System.Drawing.Color.FromArgb(255, 25, 25, 112) },
        { "MintCream", System.Drawing.Color.FromArgb(255, 245, 255, 250) },
        { "MistyRose", System.Drawing.Color.FromArgb(255, 255, 228, 225) },
        { "Moccasin", System.Drawing.Color.FromArgb(255, 255, 228, 181) },
        { "NavajoWhite", System.Drawing.Color.FromArgb(255, 255, 222, 173) },
        { "Navy", System.Drawing.Color.FromArgb(255, 0, 0, 128) },
        { "OldLace", System.Drawing.Color.FromArgb(255, 253, 245, 230) },
        { "Olive", System.Drawing.Color.FromArgb(255, 128, 128, 0) },
        { "OliveDrab", System.Drawing.Color.FromArgb(255, 107, 142, 35) },
        { "Orange", System.Drawing.Color.FromArgb(255, 255, 165, 0) },
        { "OrangeRed", System.Drawing.Color.FromArgb(255, 255, 69, 0) },
        { "Orchid", System.Drawing.Color.FromArgb(255, 218, 112, 214) },
        { "PaleGoldenrod", System.Drawing.Color.FromArgb(255, 238, 232, 170) },
        { "PaleGreen", System.Drawing.Color.FromArgb(255, 152, 251, 152) },
        { "PaleTurquoise", System.Drawing.Color.FromArgb(255, 175, 238, 238) },
        { "PaleVioletRed", System.Drawing.Color.FromArgb(255, 219, 112, 147) },
        { "PapayaWhip", System.Drawing.Color.FromArgb(255, 255, 239, 213) },
        { "PeachPuff", System.Drawing.Color.FromArgb(255, 255, 218, 185) },
        { "Peru", System.Drawing.Color.FromArgb(255, 205, 133, 63) },
        { "Pink", System.Drawing.Color.FromArgb(255, 255, 192, 203) },
        { "Plum", System.Drawing.Color.FromArgb(255, 221, 160, 221) },
        { "PowderBlue", System.Drawing.Color.FromArgb(255, 176, 224, 230) },
        { "Purple", System.Drawing.Color.FromArgb(255, 128, 0, 128) },
        { "RebeccaPurple", System.Drawing.Color.FromArgb(255, 102, 51, 153) },
        { "Red", System.Drawing.Color.FromArgb(255, 255, 0, 0) },
        { "RosyBrown", System.Drawing.Color.FromArgb(255, 188, 143, 143) },
        { "RoyalBlue", System.Drawing.Color.FromArgb(255, 65, 105, 225) },
        { "SaddleBrown", System.Drawing.Color.FromArgb(255, 139, 69, 19) },
        { "Salmon", System.Drawing.Color.FromArgb(255, 250, 128, 114) },
        { "SandyBrown", System.Drawing.Color.FromArgb(255, 244, 164, 96) },
        { "SeaGreen", System.Drawing.Color.FromArgb(255, 46, 139, 87) },
        { "SeaShell", System.Drawing.Color.FromArgb(255, 255, 245, 238) },
        { "Sienna", System.Drawing.Color.FromArgb(255, 160, 82, 45) },
        { "Silver", System.Drawing.Color.FromArgb(255, 192, 192, 192) },
        { "SkyBlue", System.Drawing.Color.FromArgb(255, 135, 206, 235) },
        { "SlateBlue", System.Drawing.Color.FromArgb(255, 106, 90, 205) },
        { "SlateGray", System.Drawing.Color.FromArgb(255, 112, 128, 144) },
        { "SlateGrey", System.Drawing.Color.FromArgb(255, 112, 128, 144) },
        { "Snow", System.Drawing.Color.FromArgb(255, 255, 250, 250) },
        { "SpringGreen", System.Drawing.Color.FromArgb(255, 0, 255, 127) },
        { "SteelBlue", System.Drawing.Color.FromArgb(255, 70, 130, 180) },
        { "Tan", System.Drawing.Color.FromArgb(255, 210, 180, 140) },
        { "Teal", System.Drawing.Color.FromArgb(255, 0, 128, 128) },
        { "Thistle", System.Drawing.Color.FromArgb(255, 216, 191, 216) },
        { "Tomato", System.Drawing.Color.FromArgb(255, 255, 99, 71) },
        { "Transparent", System.Drawing.Color.FromArgb(0, 255, 255, 255) },
        { "Turquoise", System.Drawing.Color.FromArgb(255, 64, 224, 208) },
        { "Violet", System.Drawing.Color.FromArgb(255, 238, 130, 238) },
        { "Wheat", System.Drawing.Color.FromArgb(255, 245, 222, 179) },
        { "White", System.Drawing.Color.FromArgb(255, 255, 255, 255) },
        { "WhiteSmoke", System.Drawing.Color.FromArgb(255, 245, 245, 245) },
        { "Yellow", System.Drawing.Color.FromArgb(255, 255, 255, 0) },
        { "YellowGreen", System.Drawing.Color.FromArgb(255, 154, 205, 50) }
    };

    /// <summary>
    /// Translates an HTML color representation to a System.Drawing.Color structure.
    /// Supports hex format (#RRGGBB or #AARRGGBB) and named colors (case-insensitive).
    /// </summary>
    /// <param name="htmlColor">The string representation of the HTML color to translate.</param>
    /// <returns>The System.Drawing.Color structure that represents the translated HTML color.</returns>
    /// <exception cref="ArgumentException">Thrown when htmlColor is not a valid HTML color string.</exception>
    public static System.Drawing.Color FromHtml(string htmlColor)
    {
        if (string.IsNullOrWhiteSpace(htmlColor))
            throw new ArgumentException("HTML color string cannot be null or empty.", nameof(htmlColor));

        htmlColor = htmlColor.Trim();

        // Check if it's a named color first
        if (_knownColors.TryGetValue(htmlColor, out var namedColor))
            return namedColor;

        // Remove leading '#' if present
        if (htmlColor.StartsWith('#'))
            htmlColor = htmlColor.Substring(1);

        // Validate hex string
        if (htmlColor.Length != 6 && htmlColor.Length != 8)
            throw new ArgumentException($"Invalid HTML color format: #{htmlColor}. Expected format: #RRGGBB, #AARRGGBB, or a named color.", nameof(htmlColor));

        // Parse hex values
        try
        {
            if (htmlColor.Length == 6)
            {
                // #RRGGBB format
                int r = Convert.ToInt32(htmlColor.Substring(0, 2), 16);
                int g = Convert.ToInt32(htmlColor.Substring(2, 2), 16);
                int b = Convert.ToInt32(htmlColor.Substring(4, 2), 16);
                return System.Drawing.Color.FromArgb(255, r, g, b);
            }
            else
            {
                // #AARRGGBB format
                int a = Convert.ToInt32(htmlColor.Substring(0, 2), 16);
                int r = Convert.ToInt32(htmlColor.Substring(2, 2), 16);
                int g = Convert.ToInt32(htmlColor.Substring(4, 2), 16);
                int b = Convert.ToInt32(htmlColor.Substring(6, 2), 16);
                return System.Drawing.Color.FromArgb(a, r, g, b);
            }
        }
        catch (FormatException)
        {
            throw new ArgumentException($"Invalid HTML color format: #{htmlColor}. Contains non-hexadecimal characters.", nameof(htmlColor));
        }
    }
}

#endif
