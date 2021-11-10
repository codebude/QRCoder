#if NETFRAMEWORK || NETSTANDARD2_0 || NET5_0

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

// pull request raised to extend library used. 
namespace QRCoder
{
    public class ArtQRCode : AbstractQRCode, IDisposable
    {
        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public ArtQRCode() { }

        public ArtQRCode(QRCodeData data) : base(data) { }

        public Bitmap GetGraphic(int pixelsPerModule)
        {
            return this.GetGraphic(pixelsPerModule, Color.Black, Color.White, Color.Transparent);
        }

        public Bitmap GetGraphic(Bitmap backgroundImage = null)
        {
            return this.GetGraphic(10, Color.Black, Color.White, Color.Transparent, backgroundImage: backgroundImage);
        }

        public Bitmap GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Color backgroundColor, Bitmap backgroundImage = null, double pixelSizeFactor = 0.8, 
                                 bool drawQuietZones = true, QuietZoneStyle quietZoneRenderingStyle = QuietZoneStyle.Flat, Bitmap finderPatternImage = null)
        {
            if (pixelSizeFactor > 1)
                throw new Exception("pixelSize must be between 0 and 1. (0-100%)");
            int pixelSize = (int)Math.Min(pixelsPerModule, Math.Floor(pixelsPerModule / pixelSizeFactor));

            var numModules = QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8);
            var offset = (drawQuietZones ? 0 : 4);
            var size = numModules * pixelsPerModule;

            var bitmap = new Bitmap(size, size);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                using (var lightBrush = new SolidBrush(lightColor))
                {
                    using (var darkBrush = new SolidBrush(darkColor))
                    {
                        // make background transparent
                        using (var brush = new SolidBrush(backgroundColor))                        
                            graphics.FillRectangle(brush, new Rectangle(0, 0, size, size));
                        //Render background if set
                        if (backgroundImage != null)
                            graphics.DrawImage(Resize(backgroundImage, size), 0, 0);

                        var darkModulePixel = MakeDotPixel(pixelsPerModule, pixelSize, darkBrush);
                        var lightModulePixel = MakeDotPixel(pixelsPerModule, pixelSize, lightBrush);

                        for (var x = 0; x < numModules; x += 1)
                        {
                            for (var y = 0; y < numModules; y += 1)
                            {
                                var rectangleF = new Rectangle(x * pixelsPerModule, y * pixelsPerModule, pixelsPerModule, pixelsPerModule);

                                var pixelIsDark = this.QrCodeData.ModuleMatrix[offset + y][offset + x];
                                var solidBrush = pixelIsDark ? darkBrush : lightBrush;
                                var pixelImage = pixelIsDark ? darkModulePixel : lightModulePixel;

                                if (!IsPartOfFinderPattern(x, y, numModules, offset))
                                    if (drawQuietZones && quietZoneRenderingStyle == QuietZoneStyle.Flat && IsPartOfQuietZone(x, y, numModules))
                                        graphics.FillRectangle(solidBrush, rectangleF);
                                    else
                                        graphics.DrawImage(pixelImage, rectangleF);
                                else if (finderPatternImage == null)
                                    graphics.FillRectangle(solidBrush, rectangleF);
                            }
                        }

                        if (finderPatternImage != null)
                        {
                            var finderPatternSize = 7 * pixelsPerModule;
                            graphics.DrawImage(finderPatternImage, new Rectangle(0, 0, finderPatternSize, finderPatternSize));
                            graphics.DrawImage(finderPatternImage, new Rectangle(size - finderPatternSize, 0, finderPatternSize, finderPatternSize));
                            graphics.DrawImage(finderPatternImage, new Rectangle(0, size - finderPatternSize, finderPatternSize, finderPatternSize));
                        }

                        graphics.Save();
                    }
                }
            }
            return bitmap;
        }

        /// <summary>
        /// If the pixelSize is bigger than the pixelsPerModule or may end up filling the Module making a traditional QR code.
        /// </summary>
        /// <param name="pixelsPerModule"></param>
        /// <param name="pixelSize"></param>
        /// <param name="brush"></param>
        /// <returns></returns>
        private Bitmap MakeDotPixel(int pixelsPerModule, int pixelSize, SolidBrush brush)
        {            
            // draw a dot
            var bitmap = new Bitmap(pixelSize, pixelSize);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillEllipse(brush, new Rectangle(0, 0, pixelSize, pixelSize));
                graphics.Save();
            }

            var pixelWidth = Math.Min(pixelsPerModule, pixelSize);
            var margin = Math.Max((pixelsPerModule - pixelWidth) / 2, 0);

            // center the dot in the module and crop to stay the right size.
            var cropped = new Bitmap(pixelsPerModule, pixelsPerModule);
            using (var graphics = Graphics.FromImage(cropped))
            {
                graphics.DrawImage(bitmap, new Rectangle(margin, margin, pixelWidth, pixelWidth),
                    new RectangleF(((float)pixelSize - pixelWidth) / 2, ((float)pixelSize - pixelWidth) / 2, pixelWidth, pixelWidth),
                    GraphicsUnit.Pixel);
                graphics.Save();
            }

            return cropped;
        }


        private bool IsPartOfQuietZone(int x, int y, int numModules)
        {
            return
                x < 4 || //left 
                y < 4 || //top
                x > numModules - 5 || //right
                y > numModules - 5; //bottom                
        }


        private bool IsPartOfFinderPattern(int x, int y, int numModules, int offset)
        {
            var cornerSize = 11 - offset;
            var outerLimitLow = (numModules - cornerSize - 1);
            var outerLimitHigh = outerLimitLow + 8;
            var invertedOffset = 4 - offset;
            return
                (x >= invertedOffset && x < cornerSize && y >= invertedOffset && y < cornerSize) || //Top-left finder pattern
                (x > outerLimitLow && x < outerLimitHigh && y >= invertedOffset && y < cornerSize) || //Top-right finder pattern
                (x >= invertedOffset && x < cornerSize && y > outerLimitLow && y < outerLimitHigh); //Bottom-left finder pattern
        }

        /// <summary>
        /// Resize to a square bitmap, but maintain the aspect ratio by padding transparently.
        /// </summary>
        /// 
        /// <param name="image"></param>
        /// <param name="newSize"></param>
        /// <returns></returns>
        private Bitmap Resize(Bitmap image, int newSize)
        {
            if (image == null) return null;

            float scale = Math.Min((float)newSize / image.Width, (float)newSize / image.Height);
            var scaledWidth = (int)(image.Width * scale);
            var scaledHeight = (int)(image.Height * scale);
            var offsetX = (newSize - scaledWidth) / 2;
            var offsetY = (newSize - scaledHeight) / 2;

            var scaledImage = new Bitmap(image, new Size(scaledWidth, scaledHeight));

            var bm = new Bitmap(newSize, newSize);

            using (Graphics graphics = Graphics.FromImage(bm))
            {
                using (var brush = new SolidBrush(Color.Transparent))
                {
                    graphics.FillRectangle(brush, new Rectangle(0, 0, newSize, newSize));

                    graphics.InterpolationMode = InterpolationMode.High;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    graphics.DrawImage(scaledImage, new Rectangle(offsetX, offsetY, scaledWidth, scaledHeight));
                }
            }
            return bm;
        }

        public enum QuietZoneStyle
        {
            Dotted,
            Flat
        }
    }
}

#endif