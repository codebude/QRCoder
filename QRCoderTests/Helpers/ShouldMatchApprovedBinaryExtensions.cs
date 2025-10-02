#nullable enable

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using KGySoft.Drawing;
using KGySoft.Drawing.Imaging;

namespace Shouldly;

[ShouldlyMethods]
[EditorBrowsable(EditorBrowsableState.Never)]
public static partial class ShouldMatchApprovedExtensions
{
    public static void ShouldMatchApprovedImage(this byte[] imageBytes, string? discriminator = null, string? customMessage = null, bool asMonochrome = false)
    {
        using var ms = new MemoryStream(imageBytes);
        using var image = (Bitmap)Image.FromStream(ms);
        image.ShouldMatchApproved(discriminator, customMessage, asMonochrome);
    }
    public static void ShouldMatchApproved(this Bitmap image, string? discriminator = null, string? customMessage = null, bool asMonochrome = false)
    {
        // encode to gif first for easier visual verification, and using a third party lib to avoid platform-specific compression differences
        var readableBitmapData = image.GetReadableBitmapData();
        if (asMonochrome)
            readableBitmapData = readableBitmapData.Clone(KnownPixelFormat.Format1bppIndexed);
        var ms = new MemoryStream();
        GifEncoder.EncodeImage(readableBitmapData, ms);
        ms.ToArray().ShouldMatchApproved("gif", discriminator, customMessage);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ShouldMatchApproved(this byte[] actual, string fileExtension, string? discriminator = null, string? customMessage = null)
    {
        var stackTrace = new StackTrace(true);

        // Find the first frame that is NOT in a class marked with [ShouldlyMethods]
        StackFrame? frame = null;
        MethodBase? method = null;

        for (int i = 1; i < stackTrace.FrameCount; i++)
        {
            var currentFrame = stackTrace.GetFrame(i);
            var currentMethod = currentFrame?.GetMethod();
            var declaringType = currentMethod?.DeclaringType;

            // Check if the declaring type has the ShouldlyMethods attribute
            var hasShouldlyAttribute = declaringType?.GetCustomAttributes(typeof(ShouldlyMethodsAttribute), false).Any() ?? false;

            // If this method is NOT in a ShouldlyMethods class, use it
            if (!hasShouldlyAttribute)
            {
                frame = currentFrame;
                method = currentMethod;
                break;
            }
        }

        if (frame == null)
            throw new Exception("Unable to get stack frame information");
        if (method == null)
            throw new Exception("Unable to get method information from stack frame");

        var fileName = frame.GetFileName();
        if (string.IsNullOrEmpty(fileName))
            throw new Exception($"Source information not available, make sure you are compiling with full debug information. Method: {method.DeclaringType?.Name}.{method.Name}");

        var outputFolder = Path.GetDirectoryName(fileName);
        if (string.IsNullOrEmpty(outputFolder))
            throw new Exception($"Unable to determine output folder from file: {fileName}");

        var className = method.DeclaringType?.Name ?? throw new Exception("Unable to get class name from method information");
        var testMethodName = method.Name;
        var discriminatorPart = string.IsNullOrEmpty(discriminator) ? "" : $".{discriminator}";

        var approvedFile = Path.Combine(outputFolder, $"{className}.{testMethodName}{discriminatorPart}.approved.{fileExtension}");
        var receivedFile = Path.Combine(outputFolder, $"{className}.{testMethodName}{discriminatorPart}.received.{fileExtension}");

        File.WriteAllBytes(receivedFile, actual);

        if (!File.Exists(approvedFile))
        {
            throw new ShouldAssertException($"""
                Approval file {approvedFile}
                    does not exist

                To approve the received file, run:

                copy /Y "{receivedFile}" "{approvedFile}"

                """);
        }

        var approvedFileContents = File.ReadAllBytes(approvedFile);
        var receivedFileContents = File.ReadAllBytes(receivedFile);

        var contentsMatch = approvedFileContents.AsSpan().SequenceEqual(receivedFileContents.AsSpan());

        if (!contentsMatch)
        {
            var baseMessage = $"Binary files do not match. Expected length: {approvedFileContents.Length}, Actual length: {receivedFileContents.Length}";
            var copyCommand = $"""


                To approve the received file, run:

                copy /Y "{receivedFile}" "{approvedFile}"

                """;

            var message = customMessage != null
                ? $"{customMessage}\n\n{baseMessage}{copyCommand}"
                : $"{baseMessage}{copyCommand}";

            throw new ShouldAssertException(message);
        }

        File.Delete(receivedFile);
    }
}
