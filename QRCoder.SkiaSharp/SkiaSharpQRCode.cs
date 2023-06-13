using QRCoder.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCoder.SkiaSharp
{
    public class SkiaSharpQRCode : AbstractQRCode, IDisposable
    {
        public SkiaSharpQRCode()
        {
        }

        public SkiaSharpQRCode(QRCodeData data) : base(data)
        {
        }

        /// <summary>
        /// Renders an art-style QR code with dots as modules. (With default settings: DarkColor=Black, LightColor=White, Background=Transparent, QuietZone=true)
        /// </summary>
        /// <param name="pixelsPerModule">Amount of px each dark/light module of the QR code shall take place in the final QR code image</param>
        /// <returns>QRCode graphic as bitmap</returns>
        public SKImage GetGraphic(int pixelsPerModule, SKImage? logoImage = null, LogoLocation logoLocation = LogoLocation.BottomRight, LogoBackgroundShape logoBackgroundShape = LogoBackgroundShape.Rectangle)
        {
            return this.GetGraphic(pixelsPerModule, SKColors.Black, SKColors.White, SKColors.Transparent, logoImage, logoLocation, logoBackgroundShape);
        }

        /// <summary>
        /// Renders an art-style QR code with dots as modules and a background image (With default settings: DarkColor=Black, LightColor=White, Background=Transparent, QuietZone=true)
        /// </summary>
        /// <param name="backgroundImage">A bitmap object that will be used as background picture</param>
        /// <returns>QRCode graphic as bitmap</returns>
        public SKImage GetGraphic(SKImage? logoImage = null)
        {
            return this.GetGraphic(10, SKColors.Black, SKColors.White, SKColors.Transparent, logoImage);
        }

        /// <summary>
        /// Renders an art-style QR code with dots as modules and various user settings
        /// </summary>
        /// <param name="pixelsPerModule">Amount of px each dark/light module of the QR code shall take place in the final QR code image</param>
        /// <param name="darkColor">Color of the dark modules</param>
        /// <param name="lightColor">Color of the light modules</param>
        /// <param name="backgroundColor">Color of the background</param>
        /// <param name="logoImage">A bitmap object that will be used as background picture</param>
        /// <param name="pixelSizeFactor">Value between 0.0 to 1.0 that defines how big the module dots are. The bigger the value, the less round the dots will be.</param>
        /// <param name="drawQuietZones">If true a white border is drawn around the whole QR Code</param>
        /// <param name="quietZoneRenderingStyle">Style of the quiet zones</param>
        /// <param name="backgroundImageStyle">Style of the background image (if set). Fill=spanning complete graphic; DataAreaOnly=Don't paint background into quietzone</param>
        /// <param name="finderPatternImage">Optional image that should be used instead of the default finder patterns</param>
        /// <returns>QRCode graphic as bitmap</returns>
        public SKImage GetGraphic(int pixelsPerModule, SKColor darkColor, SKColor lightColor, SKColor backgroundColor, SKImage? logoImage = null, LogoLocation logoLocation = LogoLocation.BottomRight, LogoBackgroundShape logoBackgroundShape = LogoBackgroundShape.Circle, double pixelSizeFactor = 0.8,
                                 bool drawQuietZones = true)
        {
            if (pixelSizeFactor > 1)
                throw new ArgumentException("The parameter pixelSize must be between 0 and 1. (0-100%)");

            int pixelSize = (int)Math.Min(pixelsPerModule, Math.Floor(pixelsPerModule / pixelSizeFactor));

            var numModules = QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8);
            var offset = (drawQuietZones ? 0 : 4);
            var size = numModules * pixelsPerModule;

            var imageInfo = new SKImageInfo(size, size, SKColorType.RgbaF32);

            using var surface = SKSurface.Create(imageInfo);

            var canvas = surface.Canvas;

            var lightBrush = new SKPaint
            {
                Color = lightColor,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            var darkBrush = new SKPaint
            {
                Color = darkColor,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            var backgroundBrush = new SKPaint
            {
                Color = backgroundColor,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };


            //background rectangle:

            canvas.DrawRect(0, 0, size, size, backgroundBrush);

            //if (backgroundImage != null)
            //{
            //    switch (backgroundImageStyle)
            //    {
            //        case BackgroundImageStyle.Fill:
            //            backgroundImage = backgroundImage.Clone(x => x.Resize(size, size));
            //            image.Mutate(x => x.DrawImage(backgroundImage, new Point(0, 0), 1));
            //            break;
            //        case BackgroundImageStyle.DataAreaOnly:
            //            var bgOffset = 4 - offset;
            //            backgroundImage = backgroundImage.Clone(x => x.Resize(size - (2 * bgOffset * pixelsPerModule), size - (2 * bgOffset * pixelsPerModule)));
            //            image.Mutate(x => x.DrawImage(backgroundImage, new Point(bgOffset * pixelsPerModule, bgOffset * pixelsPerModule), 1));
            //            break;
            //    }
            //}

            for (var x = 0; x < numModules; x += 1)
            {
                for (var y = 0; y < numModules; y += 1)
                {
                    var rectangleF = SKRect.Create(x * pixelsPerModule, y * pixelsPerModule, pixelsPerModule, pixelsPerModule); //creates a rectangle in positions x * pixelsPerModule, y * pixelsPerModule and with the width, height pixelsPerModule. Do not use the constructor since that uses the 4 point location.

                    var pixelIsDark = this.QrCodeData.ModuleMatrix[offset + y][offset + x];
                    var solidBrush = pixelIsDark ? darkBrush : lightBrush;
                    //var pixelImage = pixelIsDark ? darkModulePixel : lightModulePixel;

                    if (!IsPartOfFinderPattern(x, y, numModules, offset))
                        if (drawQuietZones && IsPartOfQuietZone(x, y, numModules))
                            canvas.DrawRect(rectangleF, solidBrush); // .Mutate(im => im.Fill(options, solidBrush, rectangleF));
                        else
                            canvas.DrawRect(rectangleF, solidBrush); // .Mutate(im => im.Fill(options, solidBrush, rectangleF));
                    else
                        canvas.DrawRect(rectangleF, solidBrush); // .Mutate(im => im.Fill(options, solidBrush, rectangleF));
                }
            }

            if (logoImage != null)
            {
                var logoSize = (int)(size * 0.15);
                var logoOffset = drawQuietZones ? 4 : 0;

                var locationPadding = logoLocation switch
                {
                    LogoLocation.Center => new SKPoint(size / 2, size / 2),
                    _ => new SKPoint(size - logoSize / 2 - logoOffset * pixelsPerModule, size - logoSize / 2 - logoOffset * pixelsPerModule),
                };

                var locationLogo = logoLocation switch
                {
                    LogoLocation.Center => new SKPoint(size / 2 - logoSize / 2, size / 2 - logoSize / 2),
                    _ => new SKPoint(size - logoSize - logoOffset * pixelsPerModule, size - logoSize - logoOffset * pixelsPerModule),
                };

                

                switch (logoBackgroundShape)
                {
                    case LogoBackgroundShape.Circle:
                        canvas.DrawOval(locationPadding, new SKSize(1.20f*logoSize / 2, 1.20f * logoSize/2), backgroundBrush);

                        break;
                    case LogoBackgroundShape.Rectangle:
                        var paddingRectangle = logoLocation switch
                        {
                            LogoLocation.Center => SKRect.Create(size / 2 - logoSize / 2, size / 2 - logoSize / 2, logoSize, logoSize),
                            _ => SKRect.Create(size - logoSize - logoOffset * pixelsPerModule, size - logoSize - logoOffset * pixelsPerModule, logoSize, logoSize)
                        };
                        paddingRectangle.Inflate(1.05f, 1.05f);
                        canvas.DrawRect(paddingRectangle, backgroundBrush);
                        break;
                }


                var destinationArea = logoLocation switch
                {
                    LogoLocation.Center => SKRect.Create(size / 2 - logoSize / 2, size / 2 - logoSize / 2, logoSize, logoSize),
                    _ => SKRect.Create(size - logoSize - logoOffset * pixelsPerModule, size - logoSize - logoOffset * pixelsPerModule, logoSize, logoSize)
                };
                //var sourceArea = SKRect.Create(logoImage.Info.Width, logoImage.Info.Height);

                canvas.DrawImage(logoImage, destinationArea);
            }

            return surface.Snapshot();
        }

        /// <summary>
        /// Checks if a given module(-position) is part of the quietzone of a QR code
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="numModules">Total number of modules per row</param>
        /// <returns>true, if position is part of quiet zone</returns>
        private bool IsPartOfQuietZone(int x, int y, int numModules)
        {
            return
                x < 4 || //left 
                y < 4 || //top
                x > numModules - 5 || //right
                y > numModules - 5; //bottom                
        }


        /// <summary>
        /// Checks if a given module(-position) is part of one of the three finder patterns of a QR code
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="numModules">Total number of modules per row</param>
        /// <param name="offset">Offset in modules (usually depending on drawQuietZones parameter)</param>
        /// <returns>true, if position is part of any finder pattern</returns>
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
    }
}
