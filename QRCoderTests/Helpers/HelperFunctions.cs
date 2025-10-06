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
        => Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)?.Replace("file:\\", "") ?? "";
#endif

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
