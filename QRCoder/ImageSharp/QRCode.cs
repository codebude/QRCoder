using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static QRCoder.QRCodeGenerator;

namespace QRCoder.ImageSharp
{
    public class QRCode : AbstractQRCode, IDisposable
    {
        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public QRCode() { }

        public QRCode(QRCodeData data)
            : base(data) { }

        public Image GetGraphic(int pixelsPerModule)
        {
            return GetGraphic(pixelsPerModule, Color.Black, Color.White, true);
        }

        public Image GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, bool drawQuietZones = true)
        {
            return GetGraphic(pixelsPerModule, Color.Parse(darkColorHtmlHex), Color.Parse(lightColorHtmlHex), drawQuietZones);
        }

        public Image GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true)
        {
            var moduleOffset = drawQuietZones ? 0 : 4;
            var size = (QrCodeData.ModuleMatrix.Count - (moduleOffset * 2)) * pixelsPerModule;

            var image = new Image<Rgba32>(size, size);
            DrawQRCode(image, pixelsPerModule, moduleOffset, darkColor, lightColor);

            return image;
        }

        public Image GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Image icon = null, int iconSizePercent = 15, int iconBorderWidth = 0, bool drawQuietZones = true, Color? iconBackgroundColor = null)
        {
            var img = GetGraphic(pixelsPerModule, darkColor, lightColor, drawQuietZones) as Image<Rgba32>;
            if (icon != null && iconSizePercent > 0 && iconSizePercent <= 100)
            {
                var iconDestWidth = iconSizePercent * img.Width / 100f;
                var iconDestHeight = iconDestWidth * icon.Height / icon.Width;
                var iconX = (img.Width - iconDestWidth) / 2;
                var iconY = (img.Height - iconDestHeight) / 2;
                var centerDest = new RectangleF(iconX - iconBorderWidth, iconY - iconBorderWidth, iconDestWidth + (iconBorderWidth * 2), iconDestHeight + (iconBorderWidth * 2));
                var iconDestRect = new RectangleF(iconX, iconY, iconDestWidth, iconDestHeight);

                if (iconBorderWidth > 0)
                {
                    if (!iconBackgroundColor.HasValue)
                    {
                        iconBackgroundColor = lightColor;
                    }

                    if (iconBackgroundColor != Color.Transparent)
                    {
                        img.ProcessPixelRows(accessor =>
                        {
                            for (var y = (int)centerDest.Top; y <= (int)centerDest.Bottom; y++)
                            {
                                var pixelRow = accessor.GetRowSpan(y);

                                for (var x = (int)centerDest.Left; x <= (int)centerDest.Right; x++)
                                {
                                    pixelRow[x] = iconBackgroundColor ?? lightColor;
                                }
                            }
                        });
                    }
                }

                var sizedIcon = icon.Clone(x => x.Resize((int)iconDestWidth, (int)iconDestHeight));
                img.Mutate(x => x.DrawImage(sizedIcon, new Point((int)iconDestRect.X, (int)iconDestRect.Y), 1));
            }

            return img;
        }

        private void DrawQRCode(Image<Rgba32> image, int pixelsPerModule, int moduleOffset, Color darkColor, Color lightColor)
        {
            var row = new Rgba32[image.Width];

            image.ProcessPixelRows(accessor =>
            {
                for (var modY = moduleOffset; modY < QrCodeData.ModuleMatrix.Count - moduleOffset; modY++)
                {
                    // Generate row for this y-Module
                    for (var modX = moduleOffset; modX < QrCodeData.ModuleMatrix.Count - moduleOffset; modX++)
                    {
                        for (var idx = 0; idx < pixelsPerModule; idx++)
                        {
                            row[((modX - moduleOffset) * pixelsPerModule) + idx] = this.QrCodeData.ModuleMatrix[modY][modX] ? darkColor : lightColor;
                        }
                    }

                    // Copy the prepared row to the image
                    for (var idx = 0; idx < pixelsPerModule; idx++)
                    {
                        var pixelRow = accessor.GetRowSpan(((modY - moduleOffset) * pixelsPerModule) + idx);
                        row.CopyTo(pixelRow);
                    }
                }
            });
        }
    }

    public static class ImageSharpQRCodeHelper
    {
        public static Image GetQRCode(string plainText, int pixelsPerModule, Color darkColor, Color lightColor, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1, Image icon = null, int iconSizePercent = 15, int iconBorderWidth = 0, bool drawQuietZones = true)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion))
            using (var qrCode = new QRCode(qrCodeData))
            {
                return qrCode.GetGraphic(pixelsPerModule,
                                         darkColor,
                                         lightColor,
                                         icon,
                                         iconSizePercent,
                                         iconBorderWidth,
                                         drawQuietZones);
            }
        }
    }
}
