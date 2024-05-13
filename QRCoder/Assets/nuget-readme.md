## About

QRCoder is a simple library, written in C#.NET, which enables you to create QR codes. It hasn't any dependencies to external libraries(\*), is available as package on NuGet and supports .NET Framework, .NET Core, .NET Standard and .NET. A full list of supported target frameworks can be [found here](https://www.nuget.org/packages/QRCoder/#supportedframeworks-body-tab).

***

## Documentation

👉 *Your first place to go should be our wiki. Here you can find a detailed documentation of the QRCoder and its functions.*
* [**QRCode Wiki**](https://github.com/codebude/QRCoder/wiki)
* [Creator's blog (english)](http://en.code-bude.net/2013/10/17/qrcoder-an-open-source-qr-code-generator-implementation-in-csharp/)
* [Creator's blog (german)](http://code-bude.net/2013/10/17/qrcoder-eine-open-source-qr-code-implementierung-in-csharp/)

### Release Notes
The release notes for the current and all past releases can be read here: [📄 Release Notes](https://github.com/codebude/QRCoder/wiki/Release-notes)

## Usage / Quick start

You only need a couple lines of code, to generate your first QR code.

```csharp
using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
using (QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q))
using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
{
    byte[] qrCodeImage = qrCode.GetGraphic(20);
}
```

There are a plenty of other options. So feel free to read more on that in our wiki: [Wiki: How to use QRCoder](https://github.com/codebude/QRCoder/wiki/How-to-use-QRCoder)

## Help & Issues

If you think you have found a bug or have new ideas or feature requests, then feel free to open a new issue: https://github.com/codebude/QRCoder/issues 

In case you have a question about using the library (and couldn't find an answer in our wiki), feel free to open a new question/discussion: https://github.com/codebude/QRCoder/discussions


## Legal information and credits

QRCoder is a project by [Raffael Herrmann](https://raffaelherrmann.de) and was first released in 10/2013. It's licensed under the [MIT license](https://github.com/codebude/QRCoder/blob/master/LICENSE.txt).

***
(\*) *Depending on the targeted framework the .NET libraries System.Drawing.Common and System.Text.Encoding.CodePages will used as package dependencies.*