using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
#if !NETCOREAPP1_1
using System.Drawing;
#endif
#if NETFRAMEWORK || NET5_0_WINDOWS || NET6_0_WINDOWS
using SW = System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif


namespace QRCoderTests.Helpers
{
    public static class HelperFunctions
    {

#if NETFRAMEWORK || NET5_0_WINDOWS || NET6_0_WINDOWS
        public static BitmapSource ToBitmapSource(DrawingImage source)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawImage(source, new SW.Rect(new SW.Point(0, 0), new SW.Size(source.Width, source.Height)));
            drawingContext.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)source.Width, (int)source.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            return bmp;
        }

        public static Bitmap BitmapSourceToBitmap(DrawingImage xamlImg)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(ToBitmapSource(xamlImg)));
                encoder.Save(ms);

                using (Bitmap bmp = new Bitmap(ms))
                {
                    return new Bitmap(bmp);
                }
            }
        }
#endif 

#if !NETCOREAPP1_1
        public static string GetAssemblyPath()
        {
            return
#if NET5_0 || NET6_0
            AppDomain.CurrentDomain.BaseDirectory;
#elif NET35 || NET452
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "");
#else
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Replace("file:\\", "");
#endif
        }
#endif


#if !NETCOREAPP1_1
        public static string BitmapToHash(Bitmap bmp)
        {
            byte[] imgBytes = null;
            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imgBytes = ms.ToArray();
                ms.Dispose();
            }
            return ByteArrayToHash(imgBytes);
        }
#endif

        public static string ByteArrayToHash(byte[] data)
        {
#if !NETCOREAPP1_1
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(data);
#else
            var hash = new SshNet.Security.Cryptography.MD5().ComputeHash(data);
#endif
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public static string StringToHash(string data)
        {
            return ByteArrayToHash(Encoding.UTF8.GetBytes(data));
        }
    }
}
