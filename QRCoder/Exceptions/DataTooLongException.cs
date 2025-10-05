namespace QRCoder.Exceptions;

/// <summary>
/// Exception thrown when the given payload exceeds the maximum size of the QR code standard.
/// </summary>
public class DataTooLongException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataTooLongException"/> class with a specified error message.
    /// </summary>
    /// <param name="eccLevel">The error correction level of the QR code.</param>
    /// <param name="encodingMode">The encoding mode of the QR code.</param>
    /// <param name="maxSizeByte">The maximum size allowed for the given parameters in bytes.</param>
    public DataTooLongException(string eccLevel, string encodingMode, int maxSizeByte) : base(
        $"The given payload exceeds the maximum size of the QR code standard. The maximum size allowed for the chosen parameters (ECC level={eccLevel}, EncodingMode={encodingMode}) is {maxSizeByte} bytes."
    )
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataTooLongException"/> class with a specified error message.
    /// </summary>
    /// <param name="eccLevel">The error correction level of the QR code.</param>
    /// <param name="encodingMode">The encoding mode of the QR code.</param>
    /// <param name="version">The fixed version of the QR code.</param>
    /// <param name="maxSizeByte">The maximum size allowed for the given parameters in bytes.</param>
    public DataTooLongException(string eccLevel, string encodingMode, int version, int maxSizeByte) : base(
        $"The given payload exceeds the maximum size of the QR code standard. The maximum size allowed for the chosen parameters (ECC level={eccLevel}, EncodingMode={encodingMode}, FixedVersion={version}) is {maxSizeByte} bytes."
    )
    { }
}
