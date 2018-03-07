using System.Collections.Generic;
using UnityEngine;

namespace QRCoder
{
    public class UnityQRCode : AbstractQRCode
    {
        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public UnityQRCode() { }
        public UnityQRCode(QRCodeData data) : base(data) {}
        
        public Texture2D GetGraphic(int pixelsPerModule)
        {
            return this.GetGraphic(pixelsPerModule, Color.black, Color.white);
        }

        public Texture2D GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex)
        {
            return this.GetGraphic(pixelsPerModule, HexToColor(darkColorHtmlHex), HexToColor(lightColorHtmlHex));
        }

        public static Color HexToColor(string hexColor)
        {
            hexColor = hexColor.Replace("0x", "").Replace("#", "").Trim();
            byte a = 255;
            var r = byte.Parse(hexColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            var g = byte.Parse(hexColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            var b = byte.Parse(hexColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            if (hexColor.Length == 8)
            {
                a = byte.Parse(hexColor.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }

        public Texture2D GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor)
        {
            var size = this.QrCodeData.ModuleMatrix.Count * pixelsPerModule;
            var gfx = new Texture2D(size, size, TextureFormat.ARGB32, false);
            var darkBrush = this.GetBrush(pixelsPerModule, pixelsPerModule, darkColor);
            var lightBrush = this.GetBrush(pixelsPerModule, pixelsPerModule, lightColor);
            for (var x = 0; x < size; x = x + pixelsPerModule)
            {
                for (var y = 0; y < size; y = y + pixelsPerModule)
                {
                    var module = this.QrCodeData.ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][(x + pixelsPerModule) / pixelsPerModule - 1];
                    if (module)
                        gfx.SetPixels(x, y, pixelsPerModule, pixelsPerModule, darkBrush);
                    else
                        gfx.SetPixels(x, y, pixelsPerModule, pixelsPerModule, lightBrush);
                }
            }

            gfx.Apply();
            return gfx;
        }

        internal Color[] GetBrush(int sizeX, int sizeY, Color defaultColor) {
            var len = sizeX * sizeY;
            var brush = new List<Color>(len);
            for(var i = 0; i < len; i++){
                brush.Add(defaultColor);
            }

            return brush.ToArray();
        }
    }
}