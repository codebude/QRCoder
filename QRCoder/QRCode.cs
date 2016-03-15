using System.Drawing;
using System.Drawing.Drawing2D;

namespace QRCoder
{
    using System;

    public class QRCode : AbstractQRCode<Bitmap>, IDisposable
    {
        public QRCode(QRCodeData data) : base(data) {}
        
        public override Bitmap GetGraphic(int pixelsPerModule)
        {
            return GetGraphic(pixelsPerModule, Color.Black, Color.White, true);
        }
    
        public Bitmap GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, bool drawQuietZones = true)
        {
            return GetGraphic(pixelsPerModule, ColorTranslator.FromHtml(darkColorHtmlHex), ColorTranslator.FromHtml(lightColorHtmlHex), true);
        }
    
        public Bitmap GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true)
        {
            var size = (qrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8)) * pixelsPerModule;
            int offset = drawQuietZones ? 0 : 4 * pixelsPerModule;

            Bitmap bmp = new Bitmap(size, size);
            Graphics gfx = Graphics.FromImage(bmp);
            for (int x = 0; x < size + offset; x = x + pixelsPerModule)
            {
                for (int y = 0; y < size + offset; y = y + pixelsPerModule)
                {
                    var module = qrCodeData.ModuleMatrix[(y + pixelsPerModule)/pixelsPerModule - 1][(x + pixelsPerModule)/pixelsPerModule - 1];
                    if (module)
                    {
                        gfx.FillRectangle(new SolidBrush(darkColor), new Rectangle(x - offset, y - offset, pixelsPerModule, pixelsPerModule));
                    }
                    else
                    {
                        gfx.FillRectangle(new SolidBrush(lightColor), new Rectangle(x - offset, y - offset, pixelsPerModule, pixelsPerModule));
                    }
                }
            }
    
            gfx.Save();
            return bmp;
        }
    
        public Bitmap GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Bitmap icon=null, int iconSizePercent=15, int iconBorderWidth = 6, bool drawQuietZones = true)
        {
            var size = (qrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8)) * pixelsPerModule;
            int offset = drawQuietZones ? 0 : 4 * pixelsPerModule;

            Bitmap bmp = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics gfx = Graphics.FromImage(bmp);
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gfx.CompositingQuality = CompositingQuality.HighQuality;
            gfx.Clear(lightColor);
    
            bool drawIconFlag = icon != null && iconSizePercent>0 && iconSizePercent<=100;
    
            GraphicsPath iconPath = null;
            float iconDestWidth=0, iconDestHeight=0, iconX=0, iconY=0;
    
            if (drawIconFlag)
            {
                iconDestWidth = iconSizePercent * bmp.Width / 100f;
                iconDestHeight = drawIconFlag ? iconDestWidth * icon.Height / icon.Width : 0;
                iconX = (bmp.Width - iconDestWidth) / 2;
                iconY = (bmp.Height - iconDestHeight) / 2;
    
                RectangleF centerDest = new RectangleF(iconX - iconBorderWidth, iconY - iconBorderWidth, iconDestWidth + iconBorderWidth * 2, iconDestHeight + iconBorderWidth * 2);
                iconPath = CreateRoundedRectanglePath(centerDest, iconBorderWidth * 2);
            }
    
            SolidBrush lightBrush = new SolidBrush(lightColor);
            SolidBrush darkBrush = new SolidBrush(darkColor);

            
            for (int x = 0; x < size+offset; x = x + pixelsPerModule)
            {
                for (int y = 0; y < size + offset; y = y + pixelsPerModule)
                {
                    
                    var module = qrCodeData.ModuleMatrix[(y + pixelsPerModule)/pixelsPerModule - 1][(x + pixelsPerModule)/pixelsPerModule - 1];
                    if (module)
                    {
                        Rectangle r = new Rectangle(x-offset, y-offset, pixelsPerModule, pixelsPerModule);

                        if (drawIconFlag)
                        {
                            Region region = new Region(r);
                            region.Exclude(iconPath);
                            gfx.FillRegion(darkBrush, region);
                        }
                        else
                        {
                            gfx.FillRectangle(darkBrush, r);
                        }
                    }
                    else
                        gfx.FillRectangle(lightBrush, new Rectangle(x-offset, y-offset, pixelsPerModule, pixelsPerModule));
                    
                }
            }
        
            if (drawIconFlag)
            {
                RectangleF iconDestRect = new RectangleF(iconX, iconY, iconDestWidth, iconDestHeight);
                gfx.DrawImage(icon, iconDestRect, new RectangleF(0, 0, icon.Width, icon.Height), GraphicsUnit.Pixel);
            }

            gfx.Save();
            return bmp;
        }
    
        internal GraphicsPath CreateRoundedRectanglePath(RectangleF rect, int cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }

        public void Dispose()
        {
            this.qrCodeData = null;
        }
    }
}
