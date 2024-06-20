#if NET35
namespace QRCoder;

internal static class StreamExtensions
{
    /// <summary>
    /// Copies a stream to another stream.
    /// </summary>
    public static void CopyTo(this System.IO.Stream input, System.IO.Stream output)
    {
        byte[] buffer = new byte[16 * 1024];
        int bytesRead;
        while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            output.Write(buffer, 0, bytesRead);
        }
    }
}
#endif
