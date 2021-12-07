## About

QRCoder.Xaml is an extension that provides the `XamlQRCode`-renderer for the popular [QRCoder.NET library](https://www.nuget.org/packages/QRCoder/). It allows you to render QRCodes as `DrawingImage`-objects for usage in WPF/XAML-based projects.

For usage information see the demo code below. For more general information check the [QRCoder-wiki](https://github.com/codebude/QRCoder/wiki).

***

## Documentation

👉 *Your first place to go should be our wiki. Here you can find a detailed documentation of the QRCoder and its functions.*
* [**QRCode Wiki**](https://github.com/codebude/QRCoder/wiki)
* [**QRCode Wiki > XamlQRCode**](https://github.com/codebude/QRCoder/wiki/Advanced-usage---QR-Code-renderers#28-xamlqrcode-renderer-in-detail)
* [Creator's blog (english)](http://en.code-bude.net/2013/10/17/qrcoder-an-open-source-qr-code-generator-implementation-in-csharp/)
* [Creator's blog (german)](http://code-bude.net/2013/10/17/qrcoder-eine-open-source-qr-code-implementierung-in-csharp/)

### Release Notes
The release notes for the current and all past releases can be read here: [📄 Release Notes](https://github.com/codebude/QRCoder/wiki/Release-notes)

## Usage / Quick start

You only need four lines of code, to generate and view your first QR code.

```csharp
using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
using (QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", eccLevel))
using (XamlQRCode qrCode = new XamlQRCode(qrCodeData))
{
    DrawingImage qrCodeAsXaml = qrCode.GetGraphic(20);
}
```

### Optional parameters and overloads

There are a plenty of other options. So feel free to read more on that in our wiki: [Wiki: How to use QRCoder](https://github.com/codebude/QRCoder/wiki/How-to-use-QRCoder)

## Help & Issues

If you think you have found a bug or have new ideas or feature requests, then feel free to open a new issue: https://github.com/codebude/QRCoder/issues

In case you have a question about using the library (and couldn't find an answer in our wiki), feel free to open a new question/discussion: https://github.com/codebude/QRCoder/discussions


## Legal information and credits

QRCoder is a project by [Raffael Herrmann](https://raffaelherrmann.de) and was first released in 10/2013. It's licensed under the [MIT license](https://github.com/codebude/QRCoder/blob/master/LICENSE.txt).