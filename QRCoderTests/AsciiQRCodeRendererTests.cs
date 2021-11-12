using Xunit;
using QRCoder;
using Shouldly;
using QRCoderTests.Helpers.XUnitExtenstions;


namespace QRCoderTests
{

    public class AsciiQRCodeRendererTests
    {                        

        [Fact]
        [Category("QRRenderer/AsciiQRCode")]
        public void can_render_ascii_qrcode()
        {
            var targetCode = "                                                          \n                                                          \n                                                          \n                                                          \n        ██████████████    ██  ██    ██████████████        \n        ██          ██  ██    ████  ██          ██        \n        ██  ██████  ██  ██  ██  ██  ██  ██████  ██        \n        ██  ██████  ██      ██      ██  ██████  ██        \n        ██  ██████  ██  ██          ██  ██████  ██        \n        ██          ██    ████████  ██          ██        \n        ██████████████  ██  ██  ██  ██████████████        \n                        ██  ████                          \n        ██████████  ████      ████████  ██  ████          \n        ████    ██    ██    ████      ████████  ██        \n            ██  ██  ██████████  ██  ██  ██  ████          \n        ██      ██    ████  ████  ████                    \n          ████████  ██████            ████  ██  ██        \n                                  ████████                \n        ██████████████  ████  ████  ██  ████  ████        \n        ██          ██            ████████                \n        ██  ██████  ██  ██  ██  ██    ██    ██  ██        \n        ██  ██████  ██  ██████    ██  ██                  \n        ██  ██████  ██  ██  ██  ██  ██  ████  ████        \n        ██          ██  ████  ████        ██  ██          \n        ██████████████  ██████          ██  ██████        \n                                                          \n                                                          \n                                                          \n                                                          ";

            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
            var asciiCode = new AsciiQRCode(data).GetGraphic(1);

            asciiCode.ShouldBe(targetCode);
        }

        [Fact]
        [Category("QRRenderer/AsciiQRCode")]
        public void can_render_ascii_qrcode_without_quietzones()
        {
            var targetCode = "██████████████    ██  ██    ██████████████\n██          ██  ██    ████  ██          ██\n██  ██████  ██  ██  ██  ██  ██  ██████  ██\n██  ██████  ██      ██      ██  ██████  ██\n██  ██████  ██  ██          ██  ██████  ██\n██          ██    ████████  ██          ██\n██████████████  ██  ██  ██  ██████████████\n                ██  ████                  \n██████████  ████      ████████  ██  ████  \n████    ██    ██    ████      ████████  ██\n    ██  ██  ██████████  ██  ██  ██  ████  \n██      ██    ████  ████  ████            \n  ████████  ██████            ████  ██  ██\n                          ████████        \n██████████████  ████  ████  ██  ████  ████\n██          ██            ████████        \n██  ██████  ██  ██  ██  ██    ██    ██  ██\n██  ██████  ██  ██████    ██  ██          \n██  ██████  ██  ██  ██  ██  ██  ████  ████\n██          ██  ████  ████        ██  ██  \n██████████████  ██████          ██  ██████";

            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
            var asciiCode = new AsciiQRCode(data).GetGraphic(1, drawQuietZones : false);

            asciiCode.ShouldBe(targetCode);
        }

        [Fact]
        [Category("QRRenderer/AsciiQRCode")]
        public void can_render_ascii_qrcode_with_custom_symbols()
        {
            var targetCode = "                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n        XXXXXXXXXXXXXX        XXXX  XXXXXXXXXXXXXX        \n        XXXXXXXXXXXXXX        XXXX  XXXXXXXXXXXXXX        \n        XX          XX  XXXXXX  XX  XX          XX        \n        XX          XX  XXXXXX  XX  XX          XX        \n        XX  XXXXXX  XX    XXXXXXXX  XX  XXXXXX  XX        \n        XX  XXXXXX  XX    XXXXXXXX  XX  XXXXXX  XX        \n        XX  XXXXXX  XX    XXXX      XX  XXXXXX  XX        \n        XX  XXXXXX  XX    XXXX      XX  XXXXXX  XX        \n        XX  XXXXXX  XX  XX    XX    XX  XXXXXX  XX        \n        XX  XXXXXX  XX  XX    XX    XX  XXXXXX  XX        \n        XX          XX    XX        XX          XX        \n        XX          XX    XX        XX          XX        \n        XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        \n        XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        \n                          XXXXXXXX                        \n                          XXXXXXXX                        \n            XX  XXXXXX  XXXXXX    XX    XX    XX          \n            XX  XXXXXX  XXXXXX    XX    XX    XX          \n        XX  XXXXXX    XXXX  XXXXXXXX    XXXXXX  XX        \n        XX  XXXXXX    XXXX  XXXXXXXX    XXXXXX  XX        \n                    XX  XX  XX    XX    XX  XX            \n                    XX  XX  XX    XX    XX  XX            \n          XX          XX        XX  XX  XXXXXX            \n          XX          XX        XX  XX  XXXXXX            \n          XX  XXXXXXXX  XXXX  XX    XXXXXXXX    XX        \n          XX  XXXXXXXX  XXXX  XX    XXXXXXXX    XX        \n                        XX    XXXXXXXX        XXXX        \n                        XX    XXXXXXXX        XXXX        \n        XXXXXXXXXXXXXX      XXXXXXXX    XX  XXXXXX        \n        XXXXXXXXXXXXXX      XXXXXXXX    XX  XXXXXX        \n        XX          XX  XXXXXX        XXXXXXXX            \n        XX          XX  XXXXXX        XXXXXXXX            \n        XX  XXXXXX  XX  XX  XXXX        XX  XXXX          \n        XX  XXXXXX  XX  XX  XXXX        XX  XXXX          \n        XX  XXXXXX  XX    XXXX        XXXXXXXX            \n        XX  XXXXXX  XX    XXXX        XXXXXXXX            \n        XX  XXXXXX  XX  XX  XXXXXXXX    XX  XXXXXX        \n        XX  XXXXXX  XX  XX  XXXXXXXX    XX  XXXXXX        \n        XX          XX    XX            XXXX    XX        \n        XX          XX    XX            XXXX    XX        \n        XXXXXXXXXXXXXX    XX    XXXXXX  XXXX  XXXX        \n        XXXXXXXXXXXXXX    XX    XXXXXX  XXXX  XXXX        \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          ";

            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A", QRCodeGenerator.ECCLevel.Q);
            var asciiCode = new AsciiQRCode(data).GetGraphic(2, "X", " ");

            asciiCode.ShouldBe(targetCode);
        }

        [Fact]
        [Category("QRRenderer/AsciiQRCode")]
        public void can_instantate_parameterless()
        {
            var asciiCode = new AsciiQRCode();
            asciiCode.ShouldNotBeNull();
            asciiCode.ShouldBeOfType<AsciiQRCode>();
        }

        [Fact]
        [Category("QRRenderer/AsciiQRCode")]
        public void can_render_ascii_qrcode_from_helper()
        {
            var targetCode = "                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n        XXXXXXXXXXXXXX        XXXX  XXXXXXXXXXXXXX        \n        XXXXXXXXXXXXXX        XXXX  XXXXXXXXXXXXXX        \n        XX          XX  XXXXXX  XX  XX          XX        \n        XX          XX  XXXXXX  XX  XX          XX        \n        XX  XXXXXX  XX    XXXXXXXX  XX  XXXXXX  XX        \n        XX  XXXXXX  XX    XXXXXXXX  XX  XXXXXX  XX        \n        XX  XXXXXX  XX    XXXX      XX  XXXXXX  XX        \n        XX  XXXXXX  XX    XXXX      XX  XXXXXX  XX        \n        XX  XXXXXX  XX  XX    XX    XX  XXXXXX  XX        \n        XX  XXXXXX  XX  XX    XX    XX  XXXXXX  XX        \n        XX          XX    XX        XX          XX        \n        XX          XX    XX        XX          XX        \n        XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        \n        XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        \n                          XXXXXXXX                        \n                          XXXXXXXX                        \n            XX  XXXXXX  XXXXXX    XX    XX    XX          \n            XX  XXXXXX  XXXXXX    XX    XX    XX          \n        XX  XXXXXX    XXXX  XXXXXXXX    XXXXXX  XX        \n        XX  XXXXXX    XXXX  XXXXXXXX    XXXXXX  XX        \n                    XX  XX  XX    XX    XX  XX            \n                    XX  XX  XX    XX    XX  XX            \n          XX          XX        XX  XX  XXXXXX            \n          XX          XX        XX  XX  XXXXXX            \n          XX  XXXXXXXX  XXXX  XX    XXXXXXXX    XX        \n          XX  XXXXXXXX  XXXX  XX    XXXXXXXX    XX        \n                        XX    XXXXXXXX        XXXX        \n                        XX    XXXXXXXX        XXXX        \n        XXXXXXXXXXXXXX      XXXXXXXX    XX  XXXXXX        \n        XXXXXXXXXXXXXX      XXXXXXXX    XX  XXXXXX        \n        XX          XX  XXXXXX        XXXXXXXX            \n        XX          XX  XXXXXX        XXXXXXXX            \n        XX  XXXXXX  XX  XX  XXXX        XX  XXXX          \n        XX  XXXXXX  XX  XX  XXXX        XX  XXXX          \n        XX  XXXXXX  XX    XXXX        XXXXXXXX            \n        XX  XXXXXX  XX    XXXX        XXXXXXXX            \n        XX  XXXXXX  XX  XX  XXXXXXXX    XX  XXXXXX        \n        XX  XXXXXX  XX  XX  XXXXXXXX    XX  XXXXXX        \n        XX          XX    XX            XXXX    XX        \n        XX          XX    XX            XXXX    XX        \n        XXXXXXXXXXXXXX    XX    XXXXXX  XXXX  XXXX        \n        XXXXXXXXXXXXXX    XX    XXXXXX  XXXX  XXXX        \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          ";

            //Create QR code                   
            var asciiCode = AsciiQRCodeHelper.GetQRCode("A", 2, "X", " ", QRCodeGenerator.ECCLevel.Q);
            asciiCode.ShouldBe(targetCode);
        }
    }
}



