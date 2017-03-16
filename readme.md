# QRCoder
[![qrcoder MyGet Build Status](https://www.myget.org/BuildSource/Badge/qrcoder?identifier=10cbdaa5-2dd9-460b-b424-be44e75258ec)](https://www.myget.org/feed/qrcoder/package/nuget/QRCoder)
## Info 

QRCoder is a simple library, written in C#.NET, which enables you to create QR Codes. It's licensed under the MIT-license and available as .NET Framework and .NET Core PCL version on NuGet.

Feel free to grab-up/fork the project and make it better!

For more information visit:

(German) => http://code-bude.net/2013/10/17/qrcoder-eine-open-source-qr-code-implementierung-in-csharp/

(English) => http://en.code-bude.net/2013/10/17/qrcoder-an-open-source-qr-code-generator-implementation-in-csharp/
 

## Legal information and credits

QRCoder is project by [Raffael Herrmann](http://raffaelherrmann.de) and was first released 
in 10/2013. It's licensed under the MIT license.


* * *


##Installation

Either checkout this Github repository or install QRCoder via NuGet Package Manager. If you want to use NuGet just search for "QRCoder" or run the following command in the NuGet Package Manager console:
```
PM> Install-Package QRCoder
```

*Note: The NuGet feed contains only **stable** releases. If you wan't the latest build add one of the following urls to the "Package Sources" of Visual Studio's NuGet Package Manager options.*

*NuGet V3 feed URL (Visual Studio 2015+):* `https://www.myget.org/F/qrcoder/api/v3/index.json`

*NuGet V2 feed URL (Visual Studio 2012+):* `https://www.myget.org/F/qrcoder/api/v2`



##Usage

After referencing the QRCoder.dll in your project, you only need five lines of code, to generate and view your first QR code.

```
QRCodeGenerator qrGenerator = new QRCodeGenerator();
QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
QRCode qrCode = new QRCode(qrCodeData);
Bitmap qrCodeImage = qrCode.GetGraphic(20);
```

###Optional parameters

The CreateQrCode-method has two optional parameters which affect the encoding of the payload. Both have default values. Only use them, if you know what you're doing.

- **forceUtf8** *(bool)*: This parameter enables you to force text encoding in UTF-8. Be default (and as required by QR code ISO/IEC standard) text in Byte mode will be encoded in ISO-8859-1. Only if chars are detected, which can't be encoded in ISO-8859-1, QRCoder will switch to UTF-8. With the parameter you can force the usage of UTF-8 in any case.
- **utf8BOM** *(bool)*: This parameter enables you to set ByteOrderMark (BOM) when QRCoder uses UTF-8 for text-encoding.

###Overloads - Graphics generation
The GetGraphics-method has some more overloads. The first two enable you to set the color of the QR code graphic. One uses Color-class-types, the other HTML hex color notation.

```
//Set color by using Color-class types
Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.DarkRed, Color.PaleGreen);

//Set color by using HTML hex color notation
Bitmap qrCodeImage = qrCode.GetGraphic(20, "#000ff0", "#0ff000");
```

The another overload enables you to render a logo/image in the center of the QR code.

```
Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, "C:\\Path\\to\\logo.jpg", 20);
```

###Special rendering types

Besides the normal QRCode class (which is shown in the example above) for creating QR codes in Bitmap format, there are some more QR code rendering classes, each for another special purpose.

* QRCode<sup>*</sup>
* Base64QRCode<sup>*</sup>
* BitmapByteQRCode<sup></sup>
* SvgQRCode<sup>*</sup>
* UnityQRCode<sup>*</sup>


*(&ast;) - Those classes are only available in the .NET Framework version. If you use the PCL version (e.g. for Universal apps), you have to use BitmapByteQRCode.*

####QRCode.cs - standard QR code

For an example see the snippet right at the beginning of this documentation.

####Base64QRCode.cs - QR code as base64 string

This class helps you to create a QR code as base64 string. It is useful for web- or web-based applications, where you embed the QR code as inline base64.

```
QRCodeGenerator qrGenerator = new QRCodeGenerator();
QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
Base64QRCode qrCode = new Base64QRCode(qrCodeData);
string qrCodeImageAsBase64 = qrCode.GetGraphic(20);
```

####BitmapByteQRCode.cs - QR codes as Bitmap byte-Array

This class helps you to create a QRCode as byte[] which contains a Bitmap. It was implemted for the PCL version of QRCoder because in .NET Core the System.Drawing namespace is missing and so none of the Graphics sugar, which is used in the other QR Code types is available.

```
QRCodeGenerator qrGenerator = new QRCodeGenerator();
QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
byte[] qrCodeImage = qrCode.GetGraphic(20);
```

A byte array was used, since in every target platform there are ways to create a image from that array. Following a quick example for a Windows 10 Universal App.

```
[...]
byte[] qrCodeImage = qrCode.GetGraphic(20);

using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
{
	using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
    {
    	writer.WriteBytes(qrCodeImage);
        await writer.StoreAsync();
    }
    var image = new BitmapImage();
    await image.SetSourceAsync(stream);

	imageViewer.Source = image;
}
```


####SvgQRCode.cs - vectorized QR code as SVG file

If you want to render QR code in vector format as SVG file, just use SvgQRCode instead of QRCode class.

```
QRCodeGenerator qrGenerator = new QRCodeGenerator();
QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
SvgQRCode qrCode = new SvgQRCode(qrCodeData);
string qrCodeImageAsSVG = qrCode.GetGraphic(20);
```

The string *qrCodeImageAsSVG* contains the QR code as SVG formatted string. Either you pass the string to a control which can render SVG or just save it with FileWriter or StreamWriter class.


####UnityQRCode.cs - QR codes in Unity

If you want to render QR codes in Unity, just use UnityQRCode instead of QRCode class. It returns the QR code as Texture2D type.

```
QRCodeGenerator qrGenerator = new QRCodeGenerator();
QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
UnityQRCode qrCode = new UnityQRCode(qrCodeData);
Texture2D qrCodeImage = qrCode.GetGraphic(20);
```




##PayloadGenerator.cs - Generate QR code payloads

Technically QR code is just a visual representation of a text/string. Nevertheless most QR code readers can read "special" QR codes which trigger different actions. For example: WiFi-QRcodes which, when scanned by smartphone, let the smartphone join an access point automatically.

This "special" QR codes are generated by using special structured payload string, when generating the QR code. The PayloadGenerator.cs class helps you to generate this payload strings. To generate a WiFi payload for example, you need just this one line of code:

```
PayloadGenerator.WiFi wifiPayload = new PayloadGenerator.WiFi("MyWiFi-SSID", "MyWiFi-Pass", PayloadGenerator.WiFi.Authentication.WPA);
```

To generate a QR code from this payload, just call the "ToString()"-method and pass it to the QRCoder.

```
//[...]
QRCodeData qrCodeData = qrGenerator.CreateQrCode(wifiPayload.ToString(), QRCodeGenerator.ECCLevel.Q);
//[...]
```

The PayloadGenerator supports the following types of payloads:

* Bitcoin Payment Address
* Bookmark
* Calendar events (iCal/vEvent)
* Geolocation
* Girocode
* Mail
* MMS
* Phonenumber
* Skype call
* SMS
* URL
* WiFi
