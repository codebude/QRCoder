using System.Collections.Generic;
using UnityEngine;

namespace QRCoder
{
    public class UnityQRCode : AbstractQRCode<Texture2D>
    {
        public UnityQRCode(QRCodeData data) : base(data) {}

        public override Texture2D GetGraphic(int pixelsPerModule)
        {
            return GetGraphic(pixelsPerModule, Color.black, Color.white);
        }

        public Texture2D GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor)
        {
            var size = qrCodeData.ModuleMatrix.Count * pixelsPerModule;
            Texture2D gfx = new Texture2D(size, size, TextureFormat.ARGB32, false);
            Color[] darkBrush = GetBrush(pixelsPerModule, pixelsPerModule, darkColor);
            Color[] lightBrush = GetBrush(pixelsPerModule, pixelsPerModule, lightColor);
            for (int x = 0; x < size; x = x + pixelsPerModule)
            {
                for (int y = 0; y < size; y = y + pixelsPerModule)
                {
                    var module = qrCodeData.ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][(x + pixelsPerModule) / pixelsPerModule - 1];
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
            int len = sizeX * sizeY;
            List<Color> brush = new List<Color>(len);
            for(int i = 0; i < len; i++){
                brush.Add(defaultColor);
            }

            return brush.ToArray();
        }
    }
}