# QRCoder
[![qrcoder MyGet Build Status](https://www.myget.org/BuildSource/Badge/qrcoder?identifier=10cbdaa5-2dd9-460b-b424-be44e75258ec)](https://www.myget.org/feed/qrcoder/package/nuget/QRCoder)
## Info 

QRCoder is a simple library, written in C#.NET, which enables you to create QR codes. It hasn't any dependencies to other libraries and is available as .NET Framework and .NET Core PCL version on NuGet.

Feel free to grab-up/fork the project and make it better!

For more information see:
[**QRCode Wiki**](https://github.com/codebude/QRCoder/wiki) | [Creator's blog (english)](http://en.code-bude.net/2013/10/17/qrcoder-an-open-source-qr-code-generator-implementation-in-csharp/) | [Creator's blog (german)](http://code-bude.net/2013/10/17/qrcoder-eine-open-source-qr-code-implementierung-in-csharp/)
 

## Legal information and credits

QRCoder is project by [Raffael Herrmann](http://raffaelherrmann.de) and was first released 
in 10/2013. It's licensed under the MIT license.


* * *


## Installation

Either checkout this Github repository or install QRCoder via NuGet Package Manager. If you want to use NuGet just search for "QRCoder" or run the following command in the NuGet Package Manager console:
```bash
PM> Install-Package QRCoder
```

*Note: The NuGet feed contains only **stable** releases. If you wan't the latest build add one of the following urls to the "Package Sources" of Visual Studio's NuGet Package Manager options.*

*NuGet V3 feed URL (Visual Studio 2015+):* `https://www.myget.org/F/qrcoder/api/v3/index.json`

*NuGet V2 feed URL (Visual Studio 2012+):* `https://www.myget.org/F/qrcoder/api/v2`



## Usage

You only need five lines of code, to generate and view your first QR code.

```csharp
QRCodeGenerator qrGenerator = new QRCodeGenerator();
QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
QRCode qrCode = new QRCode(qrCodeData);
Bitmap qrCodeImage = qrCode.GetGraphic(20);
```

### Optional parameters and overloads

The GetGraphics-method has some more overloads. The first two enable you to set the color of the QR code graphic. One uses Color-class-types, the other HTML hex color notation.

```csharp
//Set color by using Color-class types
Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.DarkRed, Color.PaleGreen);

//Set color by using HTML hex color notation
Bitmap qrCodeImage = qrCode.GetGraphic(20, "#000ff0", "#0ff000");
```

The another overload enables you to render a logo/image in the center of the QR code.

```csharp
Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, (Bitmap)Bitmap.FromFile("C:\\myimage.png"));
```

There are a plenty of other options. So feel free to read more on that in our wiki: [Wiki: How to use QRCoder](https://github.com/codebude/QRCoder/wiki/How-to-use-QRCoder)

### Special rendering types

Besides the normal QRCode class (which is shown in the example above) for creating QR codes in Bitmap format, there are some more QR code rendering classes, each for another special purpose.

* [QRCode<sup>*</sup>](https://github.com/codebude/QRCoder/wiki/Advanced-usage---QR-Code-renderers#21-qrcode-renderer-in-detail)
* [AsciiQRCode<sup></sup>](https://github.com/codebude/QRCoder/wiki/Advanced-usage---QR-Code-renderers#22-asciiqrcode-renderer-in-detail)
* [Base64QRCode<sup>*</sup>](https://github.com/codebude/QRCoder/wiki/Advanced-usage---QR-Code-renderers#23-base64qrcode-renderer-in-detail)
* [BitmapByteQRCode<sup></sup>](https://github.com/codebude/QRCoder/wiki/Advanced-usage---QR-Code-renderers#24-bitmapbyteqrcode-renderer-in-detail)
* [PngByteQRCode<sup></sup>](https://github.com/codebude/QRCoder/wiki/Advanced-usage---QR-Code-renderers#25-pngbyteqrcode-renderer-in-detail)
* [PostscriptQRCode<sup>*</sup>](https://github.com/codebude/QRCoder/wiki/Advanced-usage---QR-Code-renderers#29-postscriptqrcode-renderer-in-detail)
* [SvgQRCode<sup>*</sup>](https://github.com/codebude/QRCoder/wiki/Advanced-usage---QR-Code-renderers#26-svgqrcode-renderer-in-detail)
* [UnityQRCode<sup>*</sup>](https://github.com/codebude/QRCoder/wiki/Advanced-usage---QR-Code-renderers#27-unityqrcode-renderer-in-detail)
* [XamlQRCode<sup>*</sup>](https://github.com/codebude/QRCoder/wiki/Advanced-usage---QR-Code-renderers#28-xamlqrcode-renderer-in-detail)

*(&ast;) - These classes are only available in the .NET Framework/.NET Standard version. If you use the PCL version (e.g. for Universal apps), you have to use either BitmapByteQRCode or PngByteQRCode classes.*

For more information about the different rendering types click on one of the types in the list above or have a look at: [Wiki: Advanced usage - QR-Code renderers](https://github.com/codebude/QRCoder/wiki/Advanced-usage---QR-Code-renderers)

## PayloadGenerator.cs - Generate QR code payloads

Technically QR code is just a visual representation of a text/string. Nevertheless most QR code readers can read "special" QR codes which trigger different actions.

For example: WiFi-QRcodes which, when scanned by smartphone, let the smartphone join an access point automatically.

This "special" QR codes are generated by using special structured payload string, when generating the QR code. The [PayloadGenerator.cs class](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators) helps you to generate this payload strings. To generate a WiFi payload for example, you need just this one line of code:

```csharp
PayloadGenerator.WiFi wifiPayload = new PayloadGenerator.WiFi("MyWiFi-SSID", "MyWiFi-Pass", PayloadGenerator.WiFi.Authentication.WPA);
```

To generate a QR code from this payload, just call the "ToString()"-method and pass it to the QRCoder.

```csharp
//[...]
QRCodeData qrCodeData = qrGenerator.CreateQrCode(wifiPayload.ToString(), QRCodeGenerator.ECCLevel.Q);
//[...]
```

You can also use overloaded method that accepts Payload as parameter. Payload generator can have QR Code Version set (default is auto set), ECC Level (default is M) and ECI mode (default is automatic detection).

```csharp
//[...]
QRCodeData qrCodeData = qrGenerator.CreateQrCode(wifiPayload);
//[...]
```

Or if you want to override ECC Level set by Payload generator, you can use overloaded method, that allows setting ECC Level.

```csharp
//[...]
QRCodeData qrCodeData = qrGenerator.CreateQrCode(wifiPayload, QRCodeGenerator.ECCLevel.Q);
//[...]
```


You can learn more about the payload generator [in our Wiki](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators).

The PayloadGenerator supports the following types of payloads:

* [BezahlCode](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#31-bezahlcode)
* [Bitcoin Payment Address](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#32-bitcoin-payment-address)
* [Bookmark](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#33-bookmark)
* [Calendar events (iCal/vEvent)](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#34-calendar-events-icalvevent)
* [ContactData (MeCard/vCard)](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#35-contactdata-mecardvcard)
* [Geolocation](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#36-geolocation)
* [Girocode](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#37-girocode)
* [Mail](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#38-mail)
* [MMS](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#39-mms)
* [Monero address/payment](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#310-monero-addresspayment)
* [One-Time-Password](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#311-one-time-password)
* [Phonenumber](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#312-phonenumber)
* [Shadowsocks configuration](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#313-shadowsocks-configuration)
* [Skype call](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#314-skype-call)
* [SlovenianUpnQr](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#315-slovenianupnqr)
* [SMS](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#316-sms)
* [SwissQrCode (ISO-20022)](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#317-swissqrcode-iso-20022)
* [URL](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#318-url)
* [WhatsAppMessage](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#319-whatsappmessage)
* [WiFi](https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#320-wifi)
