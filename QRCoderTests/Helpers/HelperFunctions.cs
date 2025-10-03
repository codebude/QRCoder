using System.IO;
using System.Security.Cryptography;
using System.Reflection;
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
        var drawingVisual = new DrawingVisual();
        var drawingContext = drawingVisual.RenderOpen();
        drawingContext.DrawImage(source, new SW.Rect(new SW.Point(0, 0), new SW.Size(source.Width, source.Height)));
        drawingContext.Close();

        var bmp = new RenderTargetBitmap((int)source.Width, (int)source.Height, 96, 96, PixelFormats.Pbgra32);
        bmp.Render(drawingVisual);
        return bmp;
    }

    public static Bitmap BitmapSourceToBitmap(DrawingImage xamlImg)
    {
        using var ms = new MemoryStream();
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(ToBitmapSource(xamlImg)));
        encoder.Save(ms);

        using var bmp = new Bitmap(ms);
        return new Bitmap(bmp);
    }
#endif

    public static string GetAssemblyPath()
#if NET5_0_OR_GREATER
        => AppDomain.CurrentDomain.BaseDirectory;
#else
        => Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Replace("file:\\", "");
#endif

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

    public static string ByteArrayToHash(byte[] data)
    {
        var md5 = MD5.Create();
        var hash = md5.ComputeHash(data);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    public static string StringToHash(string data)
        => ByteArrayToHash(Encoding.UTF8.GetBytes(data));

    /// <summary>
    /// Gets the embedded PNG icon as a Bitmap.
    /// </summary>
    public static Bitmap GetIconBitmap()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "QRCoderTests.assets.noun_software engineer_2909346.png";
        using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");
        return new Bitmap(stream);
    }

    /// <summary>
    /// Gets the embedded PNG icon as a byte array.
    /// </summary>
    public static byte[] GetIconBytes()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "QRCoderTests.assets.noun_software engineer_2909346.png";
        using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Gets the embedded SVG icon as a string.
    /// </summary>
    public static string GetIconSvg()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "QRCoderTests.assets.noun_Scientist_2909361.svg";
        using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
