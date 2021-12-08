namespace QRCoderConsole.DataObjects
{
    public enum SupportedImageFormat
    {
        Png,
        Jpg,
        Gif,
        Bmp,
        Tiff,
        Svg,
#if !NET5_0 || NET5_0_WINDOWS
        Xaml,
#endif
        Ps,
        Eps,
    }
}
