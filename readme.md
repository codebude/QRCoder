# QRCoder

## Info 

QRCoder is a simple library, written in C#.NET, whichs enabled you to create QR Codes. It's licensed under the MIT-license.

Feel free to grab-up/fork the project and make it better!

For more information visit:

(german) => http://code-bude.net/2013/10/17/qrcoder-eine-open-source-qr-code-implementierung-in-csharp/

(english) => http://en.code-bude.net/2013/10/17/qrcoder-an-open-source-qr-code-generator-implementation-in-csharp/
 

## Legal information and credits

QRCoder is project by [Raffael Herrmann](http://raffaelherrmann.de) and was first released 
in 10/2013. It's licensed unter MIT license.


##Usage

After referencing the QRCoder.dll in your project, you only need three lines of code, to generate and view your first QR code.

```
QRCodeGenerator qrGenerator = new QRCodeGenerator();
QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(textBoxQRCode.Text, QRCodeGenerator.ECCLevel.Q);
pictureBoxQRCode.BackgroundImage = qrCode.GetGraphic(20);
```
