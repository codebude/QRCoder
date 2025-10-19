# Migration Guide: QRCoder v2

This guide helps you migrate from QRCoder v1.x to v2.x.

## New Features

_Coming soon - this section will be populated as new features are added to v2._

## Breaking Changes

### 1. Minimum framework version is now .NET Standard 2.0

Upgrade your project to one of the following frameworks if required:

- .NET Framework 4.6.2 or higher
- .NET Core 2.0 or higher
- .NET 5 or higher

### 2. QRCode and ArtQRCode moved to separate package

The QRCode and ArtQRCode renderers have been moved to the [QRCoder.SystemDrawing](https://www.nuget.org/packages/QRCoder.SystemDrawing) NuGet package.

### 3. Base64QRCode only supports PNG format

Base64QRCode now only supports PNG format. To use other image formats, use a different renderer and convert to base64:

```csharp
using QRCoder;

// Generate QR code data
using var qrCodeData = QRCodeGenerator.GenerateQrCode("Hello World", QRCodeGenerator.ECCLevel.Q);

// Use QRCode renderer to get a bitmap
using var qrCode = new QRCode(qrCodeData);
using var bitmap = qrCode.GetGraphic(20);

// Convert bitmap to JPEG byte array
using var ms = new MemoryStream();
bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
byte[] jpegBytes = ms.ToArray();

// Convert to base64
string base64String = Convert.ToBase64String(jpegBytes);
```

### 4. SvgQRCode logos must be PNG byte arrays or SVG strings

SvgQRCode logos now require PNG-encoded byte arrays or SVG strings instead of Bitmap instances. Use the SvgLogo constructor with either a byte array for PNG images or a string for SVG content.

```csharp
using QRCoder;
using System.Drawing.Imaging;

// Convert a Bitmap to PNG byte array
using var bitmap = new Bitmap("logo.jpg");
using var ms = new MemoryStream();
bitmap.Save(ms, ImageFormat.Png);
byte[] pngBytes = ms.ToArray();

// Create SvgLogo with PNG byte array
var logo = new SvgQRCode.SvgLogo(pngBytes, iconSizePercent: 15);

// Generate QR code with logo
using var qrCodeData = QRCodeGenerator.GenerateQrCode("Hello World", QRCodeGenerator.ECCLevel.Q);
using var qrCode = new SvgQRCode(qrCodeData);
string svg = qrCode.GetGraphic(20, "#000000", "#ffffff", true, SvgQRCode.SizingMode.WidthHeightAttribute, logo);
```
