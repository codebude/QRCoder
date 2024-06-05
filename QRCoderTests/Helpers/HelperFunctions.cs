using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Reflection;
#if !NETCOREAPP1_1
using System.Drawing;
#endif
#if TEST_XAML
using SW = System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif


namespace QRCoderTests.Helpers;

public static class HelperFunctions
{

#if TEST_XAML
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
        using MemoryStream ms = new MemoryStream();
        PngBitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(ToBitmapSource(xamlImg)));
        encoder.Save(ms);

        using Bitmap bmp = new Bitmap(ms);
        return new Bitmap(bmp);
    }
#endif

    public static string GetAssemblyPath()
    {
        return
#if NET5_0_OR_GREATER
            AppDomain.CurrentDomain.BaseDirectory;
#elif NETFRAMEWORK
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "");
#elif NETCOREAPP1_1
            Path.GetDirectoryName(typeof(HelperFunctions).GetTypeInfo().Assembly.Location).Replace("file:\\", "");
#else
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Replace("file:\\", "");
#endif
    }


#if !NETCOREAPP1_1
    /// <summary>
    /// Converts a bitmap to a hash string based on the pixel data
    /// using a deterministic algorithm that ignores compression algorithm
    /// differences across platforms.
    /// </summary>
    public static string BitmapToHash(Bitmap bitmap)
    {
        // Lock the bitmap's bits.
        var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        var bitmapData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        byte[] rgbValues;
        try
        {
            // Create an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bitmapData.Stride) * bitmap.Height;
            rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, rgbValues, 0, bytes);
        }
        finally
        {
            // Unlock the bits.
            bitmap.UnlockBits(bitmapData);
        }

        // Hash the resulting byte array
        return ByteArrayToHash(rgbValues);
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
