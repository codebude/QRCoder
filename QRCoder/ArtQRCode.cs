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
            return this.GetGraphic(pixelsPerModule, (pixelsPerModule * 8) / 10, Color.Black, Color.White);
        }

        public Bitmap GetGraphic(Bitmap backgroundImage = null)
        {
            return this.GetGraphic(10, 7, Color.Black, Color.White, backgroundImage: backgroundImage);
        }

        public Bitmap GetGraphic(
            int pixelsPerModule,
            int pixelSize,
            Color darkColor,
            Color lightColor,
            bool drawQuietZones = false,
            Bitmap reticleImage = null,
            Bitmap backgroundImage = null)
        {
            var numModules = this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8);
            var offset = (drawQuietZones ? 0 : 4);
            var size = numModules * pixelsPerModule;

            var bitmap = Resize(backgroundImage,size) ?? new Bitmap(size, size);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                using (var lightBrush = new SolidBrush(lightColor))
                {
                    using (var darkBrush = new SolidBrush(darkColor))
                    {
                        // make background transparent if you don't have an image -- not sure this is needed
                        if (backgroundImage == null)
                        {
                            using (var brush = new SolidBrush(Color.Transparent))
                            {
                                graphics.FillRectangle(brush, new Rectangle(0, 0, size, size));
                            }
                        }

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

                                if (!IsPartOfReticle(x, y, numModules, offset))
                                    graphics.DrawImage(pixelImage, rectangleF);
                                else if (reticleImage == null)
                                    graphics.FillRectangle(solidBrush, rectangleF);
                            }
                        }

                        if (reticleImage != null)
                        {
                            var reticleSize = 7 * pixelsPerModule;
                            graphics.DrawImage(reticleImage, new Rectangle(0, 0, reticleSize, reticleSize));
                            graphics.DrawImage(reticleImage, new Rectangle(size - reticleSize, 0, reticleSize, reticleSize));
                            graphics.DrawImage(reticleImage, new Rectangle(0, size - reticleSize, reticleSize, reticleSize));
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

        private bool IsPartOfReticle(int x, int y, int numModules, int offset)
        {
            var cornerSize = 11 - offset;
            return
                (x < cornerSize && y < cornerSize) ||
                (x > (numModules - cornerSize - 1) && y < cornerSize) ||
                (x < cornerSize && y > (numModules - cornerSize - 1));
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
    }
}

#endif