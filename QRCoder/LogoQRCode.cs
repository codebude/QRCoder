using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace QRCoder
{
    public class LogoQRCode : AbstractQRCode, IDisposable
    {
        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public LogoQRCode() { }

        public LogoQRCode(QRCodeData data) : base(data) { }

        public Bitmap GetGraphic(QRCodeData qrCodeData,
            int pixelsPerModule,
            int pixelSize,
            Color darkColor,
            Color lightColor,
            bool drawQuietZones = false,
            Bitmap reticalImage = null,
            Base64QRCode.ImageType imgType = Base64QRCode.ImageType.Png,
            Bitmap backgroundImage = null)
        {
            int numModules = qrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8);
            var offset = (drawQuietZones ? 0 : 4);
            int size = numModules * pixelsPerModule;
            var moduleMargin = pixelsPerModule - pixelSize;

            Bitmap bitmap = backgroundImage ?? new Bitmap(size, size);
            bitmap = Resize(bitmap, size);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (SolidBrush lightBrush = new SolidBrush(lightColor))
                {
                    using (SolidBrush darkBrush = new SolidBrush(darkColor))
                    {
                        // make background transparent if you don't have an image
                        if (backgroundImage == null)
                        {
                            using (var brush = new SolidBrush(Color.Transparent))
                            {
                                graphics.FillRectangle(brush, new Rectangle(0, 0, size, size));
                            }
                        }

                        for (int x = 0; x < numModules; x += 1)
                        {
                            for (int y = 0; y < numModules; y += 1)
                            {
                                var solidBrush = (Brush)(qrCodeData.ModuleMatrix[offset + y][offset + x] ? darkBrush : lightBrush);

                                if (IsPartOfRetical(x, y, numModules, offset))
                                    if (reticalImage == null)
                                        graphics.FillRectangle(solidBrush, new Rectangle(x * pixelsPerModule, y * pixelsPerModule, pixelsPerModule, pixelsPerModule));
                                    else
                                        graphics.FillEllipse(solidBrush, new Rectangle(x * pixelsPerModule + moduleMargin, y * pixelsPerModule + moduleMargin, pixelSize, pixelSize));
                            }
                        }

                        if (reticalImage != null)
                        {
                            var reticleSize = 7 * pixelsPerModule;
                            graphics.DrawImage(reticalImage, new Rectangle(0, 0, reticleSize, reticleSize));
                            graphics.DrawImage(reticalImage, new Rectangle(size - reticleSize, 0, reticleSize, reticleSize));
                            graphics.DrawImage(reticalImage, new Rectangle(0, size - reticleSize, reticleSize, reticleSize));
                        }

                        graphics.Save();
                    }
                }
            }
            return bitmap;
        }

        private bool IsPartOfRetical(int x, int y, int numModules, int offset)
        {
            var cornerSize = 7 + offset;
            return
                (x < cornerSize && y < cornerSize) ||
                (x > (numModules - cornerSize) && y < cornerSize) ||
                (x < cornerSize && y < (numModules - cornerSize));
        }

        private Bitmap Resize(Bitmap image, int size)
        {
            float scale = Math.Min((float)size / image.Width, (float)size / image.Height);
            var scaleWidth = (int)(image.Width * scale);
            var scaleHeight = (int)(image.Height * scale);
            var scaledImage = new Bitmap(image, new Size(scaleWidth, scaleHeight));

            var bm = new Bitmap(size, size);

            using (Graphics graphics = Graphics.FromImage(bm))
            {
                using (var brush = new SolidBrush(Color.Transparent))
                {
                    graphics.FillRectangle(brush, new Rectangle(0, 0, size, size));

                    graphics.InterpolationMode = InterpolationMode.High;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.DrawImage(scaledImage, new Rectangle((size / 2) - (scaledImage.Width / 2), (size / 2) - (scaledImage.Height / 2), scaledImage.Width, scaledImage.Height));
                }
            }

            return bm;
        }
    }
}