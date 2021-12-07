## About

QRCoder is a simple library, written in C#.NET, which enables you to create QR codes. It hasn't any dependencies to other libraries and is available as .NET Framework and .NET Core PCL version on NuGet.

***

## Documentation

👉 *Your first place to go should be our wiki. Here you can find a detailed documentation of the QRCoder and its functions.*
* [**QRCode Wiki**](https://github.com/codebude/QRCoder/wiki)
* [Creator's blog (english)](http://en.code-bude.net/2013/10/17/qrcoder-an-open-source-qr-code-generator-implementation-in-csharp/)
* [Creator's blog (german)](http://code-bude.net/2013/10/17/qrcoder-eine-open-source-qr-code-implementierung-in-csharp/)

### Release Notes
The release notes for the current and all past releases can be read here: [📄 Release Notes](https://github.com/codebude/QRCoder/wiki/Release-notes)

## Usage / Quick start

You only need four lines of code, to generate and view your first QR code.

```csharp
using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
using (QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q))
using (QRCode qrCode = new QRCode(qrCodeData))
{
    Bitmap qrCodeImage = qrCode.GetGraphic(20);
}
```

### Optional parameters and overloads

The GetGraphics-method has some more overloads. The first two enable you to set the color of the QR code graphic. One uses Color-class-types, the other HTML hex color notation.

```csharp
//Set color by using Color-class types
Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.DarkRed, Color.PaleGreen, true);

//Set color by using HTML hex color notation
Bitmap qrCodeImage = qrCode.GetGraphic(20, "#000ff0", "#0ff000");
```

The other overload enables you to render a logo/image in the center of the QR code.

```csharp
Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, (Bitmap)Bitmap.FromFile("C:\\myimage.png"));
```

There are a plenty of other options. So feel free to read more on that in our wiki: [Wiki: How to use QRCoder](https://github.com/codebude/QRCoder/wiki/How-to-use-QRCoder)

## Help & Issues

If you think you have found a bug or have new ideas or feature requests, then feel free to open a new issue: https://github.com/codebude/QRCoder/issues 

In case you have a question about using the library (and couldn't find an answer in our wiki), feel free to open a new question/discussion: https://github.com/codebude/QRCoder/discussions


## Legal information and credits

QRCoder is a project by [Raffael Herrmann](https://raffaelherrmann.de) and was first released in 10/2013. It's licensed under the [MIT license](https://github.com/codebude/QRCoder/blob/master/LICENSE.txt).