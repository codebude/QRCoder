# QRCoder

## Info 

QRCoder is a simple library, written in C#.NET, which enables you to create QR Codes. It's licensed under the MIT-license.

Feel free to grab-up/fork the project and make it better!

For more information visit:

(German) => http://code-bude.net/2013/10/17/qrcoder-eine-open-source-qr-code-implementierung-in-csharp/

(English) => http://en.code-bude.net/2013/10/17/qrcoder-an-open-source-qr-code-generator-implementation-in-csharp/
 

## Legal information and credits

QRCoder is project by [Raffael Herrmann](http://raffaelherrmann.de) and was first released 
in 10/2013. It's licensed under the MIT license.


##Usage

After referencing the QRCoder.dll in your project, you only need four lines of code, to generate and view your first QR code.

```
QRCodeGenerator qrGenerator = new QRCodeGenerator();
QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
QRCode qrCode = new QRCode(qrCodeData);
Bitmap qrCodeImage = qrCode.GetGraphic(20);
```

###Overloads
The GetGraphics-method has 3 more overloads. The first two enable you to set the color of the QR code graphic. One uses Color-class-types, the other HTML hex color notation.

```
//Set color by using Color-class types
Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.DarkRed, Color.PaleGreen);

//Set color by using HTML hex color notation
Bitmap qrCodeImage = qrCode.GetGraphic(20, "#000ff0", "#0ff000");
```

The third overload enables you to render a logo/image in the center of the QR code.

```
Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, "C:\\Path\\to\\logo.jpg", 20);
```

###QR code in Unity

If you want to render QR codes in Unity, just use UnityQRCode instead of QRCode class.

```
QRCodeGenerator qrGenerator = new QRCodeGenerator();
QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
UnityQRCode qrCode = new UnityQRCode(qrCodeData);
Bitmap qrCodeImage = qrCode.GetGraphic(20);
```

##
