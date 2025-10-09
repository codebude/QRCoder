# QRCoder

[![License](https://img.shields.io/github/license/Shane32/QRCoder)](LICENSE.txt)
[![NuGet](https://img.shields.io/nuget/v/QRCoder)](https://www.nuget.org/packages/QRCoder/)
[![Nuget](https://img.shields.io/nuget/dt/QRCoder)](https://www.nuget.org/packages/QRCoder)
[![Coverage](https://codecov.io/gh/Shane32/QRCoder/branch/master/graph/badge.svg?token=3yNs88KD8S)](https://codecov.io/gh/Shane32/QRCoder)
[![GitHub contributors](https://img.shields.io/github/contributors/Shane32/QRCoder)](https://github.com/Shane32/QRCoder/graphs/contributors)

QRCoder is a simple C# library originally created by [Raffael Herrmann](https://raffaelherrmann.de) for generating QR codes and Micro QR codes.

- 📚 [Documentation & Wiki](https://github.com/Shane32/QRCoder/wiki)
- 📋 [Release notes / Changelog](https://github.com/Shane32/QRCoder/releases)
- 🚀 [Upcoming features](https://github.com/Shane32/QRCoder/milestones)

## ✨ Features

- 🚀 **Zero dependencies** - No external libraries required (framework dependencies only)
- ⚡ **Fast performance** - Optimized QR code generation with low memory footprint
- 🎨 **Multiple output formats** - PNG, SVG, PDF, ASCII, Bitmap, PostScript, and more
- 📱 **23+ payload generators** - WiFi, vCard, URLs, payments, and many more
- 🔧 **Highly configurable** - Error correction levels, custom colors, logos, and styling
- 🌐 **Cross-platform** - Supports .NET 5+, .NET Framework 3.5+, .NET Core 1.0+, and .NET Standard 1.3+
- 📦 **Micro QR codes** - Smaller QR codes for space-constrained applications

## 📦 Installation

Install via NuGet Package Manager:

```bash
PM> Install-Package QRCoder
```

## 🚀 Quick Start

Generate a QR code with just a few lines of code, either using a renderer's static helper method, or by creating a QR code first and then passing it to a renderer:

```csharp
using QRCoder;

// Generate a simple black and white PNG QR code
byte[] qrCodeImage = PngByteQRCodeHelper.GetQRCode("Hello World", QRCodeGenerator.ECCLevel.Q, 20);

// Generate a scalable black and white SVG QR code
using var qrCodeData = QRCodeGenerator.GenerateQrCode("Hello World", QRCodeGenerator.ECCLevel.Q);
using var svgRenderer = new SvgQRCode(qrCodeData);
string svg = svgRenderer.GetGraphic();
```

For more examples and detailed usage instructions, see: [Wiki: How to use QRCoder](https://github.com/Shane32/QRCoder/wiki/How-to-use-QRCoder)

## 📱 Payload Generators

QR codes can encode structured data that triggers specific actions when scanned (e.g., WiFi credentials, contact information, URLs). QRCoder includes payload generators that create properly formatted strings for these common use cases.

### Usage Example

```csharp
using QRCoder;

// Create a bookmark payload
var bookmarkPayload = new PayloadGenerator.Bookmark("https://github.com/Shane32/QRCoder", "QRCoder Repository");

// Generate the QR code data from the payload
using var qrCodeData = QRCodeGenerator.GenerateQrCode(bookmarkPayload);

// Or override the ECC level
using var qrCodeData2 = QRCodeGenerator.GenerateQrCode(bookmarkPayload, QRCodeGenerator.ECCLevel.H);

// Render the QR code
using var pngRenderer = new PngByteQRCode(qrCodeData);
byte[] qrCodeImage = pngRenderer.GetGraphic(20);
```

### Available Payload Types

| Payload Type | Usage Example | Description |
|--------------|---------------|-------------|
| [**WiFi**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#321-wifi) | `new PayloadGenerator.WiFi(ssid, password, auth)` | WiFi network credentials |
| [**URL**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#319-url) | `new PayloadGenerator.Url("https://example.com")` | Website URL |
| [**Bookmark**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#33-bookmark) | `new PayloadGenerator.Bookmark(url, title)` | Browser bookmark |
| [**Mail**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#38-mail) | `new PayloadGenerator.Mail(email, subject, body)` | Email with pre-filled fields |
| [**SMS**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#317-sms) | `new PayloadGenerator.SMS(number, message)` | SMS message |
| [**MMS**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#39-mms) | `new PayloadGenerator.MMS(number, subject)` | MMS message |
| [**Geolocation**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#36-geolocation) | `new PayloadGenerator.Geolocation(lat, lng)` | GPS coordinates |
| [**PhoneNumber**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#312-phonenumber) | `new PayloadGenerator.PhoneNumber(number)` | Phone number for calling |
| [**SkypeCall**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#315-skype-call) | `new PayloadGenerator.SkypeCall(username)` | Skype call |
| [**WhatsAppMessage**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#320-whatsappmessage) | `new PayloadGenerator.WhatsAppMessage(number, msg)` | WhatsApp message |
| [**ContactData**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#35-contactdata-mecardvcard) | `new PayloadGenerator.ContactData(...)` | vCard/MeCard contact |
| [**CalendarEvent**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#34-calendar-events-icalvevent) | `new PayloadGenerator.CalendarEvent(...)` | iCal/vEvent |
| [**OneTimePassword**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#311-one-time-password) | `new PayloadGenerator.OneTimePassword(...)` | TOTP/HOTP for 2FA |
| [**BitcoinAddress**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#32-bitcoin-like-crypto-currency-payment-address) | `new PayloadGenerator.BitcoinAddress(address)` | Bitcoin payment |
| [**BitcoinCashAddress**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#32-bitcoin-like-crypto-currency-payment-address) | `new PayloadGenerator.BitcoinCashAddress(address)` | Bitcoin Cash payment |
| [**LitecoinAddress**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#32-bitcoin-like-crypto-currency-payment-address) | `new PayloadGenerator.LitecoinAddress(address)` | Litecoin payment |
| [**MoneroTransaction**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#310-monero-addresspayment) | `new PayloadGenerator.MoneroTransaction(...)` | Monero payment |
| [**SwissQrCode**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#318-swissqrcode-iso-20022) | `new PayloadGenerator.SwissQrCode(...)` | Swiss QR bill (ISO-20022) |
| [**Girocode**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#37-girocode) | `new PayloadGenerator.Girocode(...)` | SEPA payment (EPC QR) |
| [**BezahlCode**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#31-bezahlcode) | `new PayloadGenerator.BezahlCode(...)` | German payment code |
| [**RussiaPaymentOrder**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#313-russiapaymentorder) | `new PayloadGenerator.RussiaPaymentOrder(...)` | Russian payment (ГОСТ Р 56042-2014) |
| [**SlovenianUpnQr**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#316-slovenianupnqr) | `new PayloadGenerator.SlovenianUpnQr(...)` | Slovenian UPN payment |
| [**ShadowSocksConfig**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators#314-shadowsocks-configuration) | `new PayloadGenerator.ShadowSocksConfig(...)` | Shadowsocks proxy config |

For detailed information about payload generators, see: [Wiki: Advanced usage - Payload generators](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---Payload-generators)

## 🎨 QR Code Renderers

QRCoder provides multiple renderers for different output formats and use cases. Each renderer has specific capabilities and framework requirements.

| Renderer | Output Format | Requires | Usage Example |
|----------|---------------|----------|---------------|
| [**PngByteQRCode**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---QR-Code-renderers#25-pngbyteqrcode-renderer-in-detail) | PNG byte array | — | `new PngByteQRCode(data).GetGraphic(20)` |
| [**SvgQRCode**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---QR-Code-renderers#26-svgqrcode-renderer-in-detail) | SVG string | — | `new SvgQRCode(data).GetGraphic(20)` |
| [**QRCode**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---QR-Code-renderers#21-qrcode-renderer-in-detail) | System.Drawing.Bitmap | Windows¹ | `new QRCode(data).GetGraphic(20)` |
| [**ArtQRCode**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---QR-Code-renderers#211-artqrcode-renderer-in-detail) | Artistic bitmap with custom images | Windows¹ | `new ArtQRCode(data).GetGraphic(20)` |
| [**AsciiQRCode**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---QR-Code-renderers#22-asciiqrcode-renderer-in-detail) | ASCII art string | — | `new AsciiQRCode(data).GetGraphic(1)`<br/>`new AsciiQRCode(data).GetGraphicSmall()` |
| [**Base64QRCode**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---QR-Code-renderers#23-base64qrcode-renderer-in-detail) | Base64 encoded image | — | `new Base64QRCode(data).GetGraphic(20)` |
| [**BitmapByteQRCode**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---QR-Code-renderers#24-bitmapbyteqrcode-renderer-in-detail) | BMP byte array | — | `new BitmapByteQRCode(data).GetGraphic(20)` |
| [**PdfByteQRCode**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---QR-Code-renderers#210-pdfbyteqrcode-renderer-in-detail) | PDF byte array | — | `new PdfByteQRCode(data).GetGraphic(20)` |
| [**PostscriptQRCode**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---QR-Code-renderers#29-postscriptqrcode-renderer-in-detail) | PostScript/EPS string | — | `new PostscriptQRCode(data).GetGraphic(20)` |
| [**XamlQRCode**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---QR-Code-renderers#28-xamlqrcode-renderer-in-detail) | XAML DrawingImage | XAML² | `new XamlQRCode(data).GetGraphic(20)` |
| [**UnityQRCode**](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---QR-Code-renderers#27-unityqrcode-renderer-in-detail) | Unity Texture2D | Unity³ | `new UnityQRCode(data).GetGraphic(20)` |

**Notes:**
- ¹ Requires Windows or System.Drawing.Common package (uses GDI+)
- ² Requires the [QRCoder.Xaml](https://www.nuget.org/packages/QRCoder.Xaml) package
- ³ Requires the [QRCoder.Unity](https://www.nuget.org/packages/QRCoder.Unity) package

**Framework Compatibility:** Not all renderers are available on all target frameworks. Check the [compatibility table](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---QR-Code-renderers#2-overview-of-the-different-renderers) for details.

For comprehensive information about renderers, see: [Wiki: Advanced usage - QR Code renderers](https://github.com/Shane32/QRCoder/wiki/Advanced-usage---QR-Code-renderers)

## 🔧 Advanced Features

### Micro QR Codes

QRCoder supports Micro QR codes, which are smaller versions of standard QR codes suitable for applications with limited space. Micro QR codes have significantly limited storage capacity—as few as 5 numeric digits (M1) or as many as 35 numeric digits (M4), with alphanumeric and byte data storing considerably less.

```csharp
using QRCoder;

// Generate a Micro QR code (versions M1-M4, represented as -1 to -4)
using var qrCodeData = QRCodeGenerator.GenerateMicroQrCode("Hello", QRCodeGenerator.ECCLevel.L, requestedVersion: -2);
using var qrCode = new PngByteQRCode(qrCodeData);
byte[] qrCodeImage = qrCode.GetGraphic(20);
```

**Note:** Micro QR codes have limitations on data capacity and error correction levels. They support versions M1 through M4 (specified as -1 to -4), and not all ECC levels are available for all versions. M1 only supports detection (no ECC), M2 and M3 support L and M levels, and M4 supports L, M, and Q levels. For detailed capacity tables, see the [Micro QR Code specification](https://www.qrcode.com/en/codes/microqr.html).

### Working with QRCodeData

`QRCodeData` is the core data structure that represents a QR code's module matrix. It contains a `List<BitArray>` called `ModuleMatrix`, where each `BitArray` represents a row of modules in the QR code. A module is set to `true` for dark/black modules and `false` for light/white modules.

You can access the `ModuleMatrix` directly to read or manipulate the QR code data at the module level. This is useful for custom rendering implementations or analyzing QR code structure.

```csharp
using QRCoder;

// Generate QR code data
using var qrCodeData = QRCodeGenerator.GenerateQrCode("Hello World", QRCodeGenerator.ECCLevel.Q);

// Access the module matrix
var moduleMatrix = qrCodeData.ModuleMatrix;
int size = moduleMatrix.Count; // Size of the QR code (includes quiet zone)

// Manually render as ASCII (versus the included ASCII renderer)
for (int y = 0; y < size; y++)
{
    for (int x = 0; x < size; x++)
    {
        // Check if module is dark (true) or light (false)
        bool isDark = moduleMatrix[y][x];
        Console.Write(isDark ? "██" : "  ");
    }
    Console.WriteLine();
}
```

## 🚀 CI Builds

The NuGet feed contains only **major/stable** releases. If you want the latest functions and features, you can use the CI builds [via Github packages](https://github.com/Shane32/qrcoder/packages).

_(More information on how to use Github Packages in Nuget Package Manager can be [found here](https://samlearnsazure.blog/2021/08/08/consuming-a-nuget-package-from-github-packages/).)_

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

## 📄 License

QRCoder is a project originally by [Raffael Herrmann](https://raffaelherrmann.de) and was first released in 10/2013. It's licensed under the [MIT license](https://github.com/Shane32/QRCoder/blob/master/LICENSE.txt).

Since 2025, QRCoder has been maintained by [Shane32](https://github.com/Shane32) with contributions from the community.

## 🙏 Credits

Glory to Jehovah, Lord of Lords and King of Kings, creator of Heaven and Earth, who through his Son Jesus Christ, has redeemed me to become a child of God. -[Shane32](https://github.com/Shane32)
