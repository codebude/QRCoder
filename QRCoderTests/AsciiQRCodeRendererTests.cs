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
            var targetCode = "                                                          \n                                                          \n                                                          \n                                                          \n        ██████████████  ████  ██    ██████████████        \n        ██          ██  ████    ██  ██          ██        \n        ██  ██████  ██  ██  ██  ██  ██  ██████  ██        \n        ██  ██████  ██  ██      ██  ██  ██████  ██        \n        ██  ██████  ██  ██████████  ██  ██████  ██        \n        ██          ██              ██          ██        \n        ██████████████  ██  ██  ██  ██████████████        \n                        ██████████                        \n          ████  ██  ████    ██████  ██  ██████████        \n        ██        ██        ██      ██    ██  ████        \n            ████  ██████  ██████        ██████  ██        \n        ████      ██  ██████  ██    ██        ██          \n          ████    ████  ██  ██      ██  ██  ████          \n                        ██    ██  ██  ██  ██              \n        ██████████████  ██  ████  ██████    ██            \n        ██          ██    ██    ████  ██████              \n        ██  ██████  ██  ██████  ████████    ██  ██        \n        ██  ██████  ██    ██        ██      ████          \n        ██  ██████  ██  ██████  ██      ██      ██        \n        ██          ██  ██  ██      ██      ██████        \n        ██████████████    ██    ██  ██  ██  ██  ██        \n                                                          \n                                                          \n                                                          \n                                                          ";
            
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
            var asciiCode = new AsciiQRCode(data).GetGraphic(1);

            asciiCode.ShouldBe(targetCode);
        }

        [Fact]
        [Category("QRRenderer/AsciiQRCode")]
        public void can_render_small_ascii_qrcode()
        {
            var targetCode = "█████████████████████████████\n█████████████████████████████\n████ ▄▄▄▄▄ █▀▄█ ▀█ ▄▄▄▄▄ ████\n████ █   █ █▄█ █▄█ █   █ ████\n████ █▄▄▄█ █▄▀▀▀▀█ █▄▄▄█ ████\n████▄▄▄▄▄▄▄█ █ ▀▄█▄▄▄▄▄▄▄████\n████  ▄▄ █▄ ██▀ ▄▄▄▀ ▀ ▄▀████\n████▀█▄█ █▄  ▄ ▀▄▀ █▄█▄▄█████\n█████▄▄▄▄█▄▄▄████▀▀  █▄█▄████\n████ ▄▄▄▄▄ █▄▄█▄▄▀ ▀ ▄█▄▄████\n████ █   █ █ ▀ █▄▀█ ██▄█▄████\n████ █▄▄▄█ █ ▀▄▀ █▄█▄ █ ▄████\n████▄▄▄▄▄▄▄█▄▄▄█████▄█▄▄▄████\n█████████████████████████████\n▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀";

            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
            var asciiCode = new AsciiQRCode(data).GetGraphicSmall();

            asciiCode.ShouldBe(targetCode);
        }

        [Fact]
        [Category("QRRenderer/AsciiQRCode")]
        public void can_render_small_ascii_qrcode_without_quietzones()
        {
            var targetCode = " ▄▄▄▄▄ █▀▄█ ▀█ ▄▄▄▄▄ \n █   █ █▄█ █▄█ █   █ \n █▄▄▄█ █▄▀▀▀▀█ █▄▄▄█ \n▄▄▄▄▄▄▄█ █ ▀▄█▄▄▄▄▄▄▄\n  ▄▄ █▄ ██▀ ▄▄▄▀ ▀ ▄▀\n▀█▄█ █▄  ▄ ▀▄▀ █▄█▄▄█\n█▄▄▄▄█▄▄▄████▀▀  █▄█▄\n ▄▄▄▄▄ █▄▄█▄▄▀ ▀ ▄█▄▄\n █   █ █ ▀ █▄▀█ ██▄█▄\n █▄▄▄█ █ ▀▄▀ █▄█▄ █ ▄\n▄▄▄▄▄▄▄█▄▄▄█████▄█▄▄▄";

            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
            var asciiCode = new AsciiQRCode(data).GetGraphicSmall(drawQuietZones: false);

            asciiCode.ShouldBe(targetCode);
        }

        [Fact]
        [Category("QRRenderer/AsciiQRCode")]
        public void can_render_small_ascii_qrcode_inverted()
        {
            var targetCode = "                             \n                             \n    █▀▀▀▀▀█ ▄▀ █▄ █▀▀▀▀▀█    \n    █ ███ █ ▀ █ ▀ █ ███ █    \n    █ ▀▀▀ █ ▀▄▄▄▄ █ ▀▀▀ █    \n    ▀▀▀▀▀▀▀ █ █▄▀ ▀▀▀▀▀▀▀    \n    ██▀▀█ ▀█  ▄█▀▀▀▄█▄█▀▄    \n    ▄ ▀ █ ▀██▀█▄▀▄█ ▀ ▀▀     \n     ▀▀▀▀ ▀▀▀    ▄▄██ ▀ ▀    \n    █▀▀▀▀▀█ ▀▀ ▀▀▄█▄█▀ ▀▀    \n    █ ███ █ █▄█ ▀▄ █  ▀ ▀    \n    █ ▀▀▀ █ █▄▀▄█ ▀ ▀█ █▀    \n    ▀▀▀▀▀▀▀ ▀▀▀     ▀ ▀▀▀    \n                             \n                             ";

            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
            var asciiCode = new AsciiQRCode(data).GetGraphicSmall(invert: true);

            asciiCode.ShouldBe(targetCode);
        }

        [Fact]
        [Category("QRRenderer/AsciiQRCode")]
        public void can_render_small_ascii_qrcode_with_custom_eol()
        {
            var targetCode = "█████████████████████████████\r\n█████████████████████████████\r\n████ ▄▄▄▄▄ █▀▄█ ▀█ ▄▄▄▄▄ ████\r\n████ █   █ █▄█ █▄█ █   █ ████\r\n████ █▄▄▄█ █▄▀▀▀▀█ █▄▄▄█ ████\r\n████▄▄▄▄▄▄▄█ █ ▀▄█▄▄▄▄▄▄▄████\r\n████  ▄▄ █▄ ██▀ ▄▄▄▀ ▀ ▄▀████\r\n████▀█▄█ █▄  ▄ ▀▄▀ █▄█▄▄█████\r\n█████▄▄▄▄█▄▄▄████▀▀  █▄█▄████\r\n████ ▄▄▄▄▄ █▄▄█▄▄▀ ▀ ▄█▄▄████\r\n████ █   █ █ ▀ █▄▀█ ██▄█▄████\r\n████ █▄▄▄█ █ ▀▄▀ █▄█▄ █ ▄████\r\n████▄▄▄▄▄▄▄█▄▄▄█████▄█▄▄▄████\r\n█████████████████████████████\r\n▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀";

            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
            var asciiCode = new AsciiQRCode(data).GetGraphicSmall(endOfLine: "\r\n");

            asciiCode.ShouldBe(targetCode);
        }

        [Fact]
        [Category("QRRenderer/AsciiQRCode")]
        public void can_render_ascii_qrcode_without_quietzones()
        {
            var targetCode = "██████████████  ████  ██    ██████████████\n██          ██  ████    ██  ██          ██\n██  ██████  ██  ██  ██  ██  ██  ██████  ██\n██  ██████  ██  ██      ██  ██  ██████  ██\n██  ██████  ██  ██████████  ██  ██████  ██\n██          ██              ██          ██\n██████████████  ██  ██  ██  ██████████████\n                ██████████                \n  ████  ██  ████    ██████  ██  ██████████\n██        ██        ██      ██    ██  ████\n    ████  ██████  ██████        ██████  ██\n████      ██  ██████  ██    ██        ██  \n  ████    ████  ██  ██      ██  ██  ████  \n                ██    ██  ██  ██  ██      \n██████████████  ██  ████  ██████    ██    \n██          ██    ██    ████  ██████      \n██  ██████  ██  ██████  ████████    ██  ██\n██  ██████  ██    ██        ██      ████  \n██  ██████  ██  ██████  ██      ██      ██\n██          ██  ██  ██      ██      ██████\n██████████████    ██    ██  ██  ██  ██  ██";
            
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
            var targetCode = "                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n        XXXXXXXXXXXXXX    XX        XXXXXXXXXXXXXX        \n        XXXXXXXXXXXXXX    XX        XXXXXXXXXXXXXX        \n        XX          XX        XXXX  XX          XX        \n        XX          XX        XXXX  XX          XX        \n        XX  XXXXXX  XX  XXXX        XX  XXXXXX  XX        \n        XX  XXXXXX  XX  XXXX        XX  XXXXXX  XX        \n        XX  XXXXXX  XX    XX    XX  XX  XXXXXX  XX        \n        XX  XXXXXX  XX    XX    XX  XX  XXXXXX  XX        \n        XX  XXXXXX  XX  XXXX    XX  XX  XXXXXX  XX        \n        XX  XXXXXX  XX  XXXX    XX  XX  XXXXXX  XX        \n        XX          XX  XX      XX  XX          XX        \n        XX          XX  XX      XX  XX          XX        \n        XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        \n        XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        \n                          XX  XX                          \n                          XX  XX                          \n          XX    XX  XX  XXXXXX  XXXX  XXXX  XX            \n          XX    XX  XX  XXXXXX  XXXX  XXXX  XX            \n          XXXXXX  XX  XXXX      XX    XX  XX  XXXX        \n          XXXXXX  XX  XXXX      XX    XX  XX  XXXX        \n          XXXXXX    XXXXXXXXXX      XXXXXXXXXX            \n          XXXXXX    XXXXXXXXXX      XXXXXXXXXX            \n        XX  XX  XX    XX  XX    XXXXXX  XX  XX            \n        XX  XX  XX    XX  XX    XXXXXX  XX  XX            \n        XXXXXX      XXXX  XX  XX  XXXX      XX  XX        \n        XXXXXX      XXXX  XX  XX  XXXX      XX  XX        \n                        XXXXXX    XXXX      XX  XX        \n                        XXXXXX    XXXX      XX  XX        \n        XXXXXXXXXXXXXX        XXXXXX            XX        \n        XXXXXXXXXXXXXX        XXXXXX            XX        \n        XX          XX          XX    XX  XX              \n        XX          XX          XX    XX  XX              \n        XX  XXXXXX  XX  XXXXXXXXXX  XXXXXXXXXXXXXX        \n        XX  XXXXXX  XX  XXXXXXXXXX  XXXXXXXXXXXXXX        \n        XX  XXXXXX  XX    XX  XXXX    XX  XX  XXXX        \n        XX  XXXXXX  XX    XX  XXXX    XX  XX  XXXX        \n        XX  XXXXXX  XX    XXXXXX    XXXXXXXXXX            \n        XX  XXXXXX  XX    XXXXXX    XXXXXXXXXX            \n        XX          XX  XX        XXXX  XX  XX  XX        \n        XX          XX  XX        XXXX  XX  XX  XX        \n        XXXXXXXXXXXXXX    XX    XXXXXX      XXXXXX        \n        XXXXXXXXXXXXXX    XX    XXXXXX      XXXXXX        \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          ";

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
            var targetCode = "                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n        XXXXXXXXXXXXXX    XX        XXXXXXXXXXXXXX        \n        XXXXXXXXXXXXXX    XX        XXXXXXXXXXXXXX        \n        XX          XX        XXXX  XX          XX        \n        XX          XX        XXXX  XX          XX        \n        XX  XXXXXX  XX  XXXX        XX  XXXXXX  XX        \n        XX  XXXXXX  XX  XXXX        XX  XXXXXX  XX        \n        XX  XXXXXX  XX    XX    XX  XX  XXXXXX  XX        \n        XX  XXXXXX  XX    XX    XX  XX  XXXXXX  XX        \n        XX  XXXXXX  XX  XXXX    XX  XX  XXXXXX  XX        \n        XX  XXXXXX  XX  XXXX    XX  XX  XXXXXX  XX        \n        XX          XX  XX      XX  XX          XX        \n        XX          XX  XX      XX  XX          XX        \n        XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        \n        XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        \n                          XX  XX                          \n                          XX  XX                          \n          XX    XX  XX  XXXXXX  XXXX  XXXX  XX            \n          XX    XX  XX  XXXXXX  XXXX  XXXX  XX            \n          XXXXXX  XX  XXXX      XX    XX  XX  XXXX        \n          XXXXXX  XX  XXXX      XX    XX  XX  XXXX        \n          XXXXXX    XXXXXXXXXX      XXXXXXXXXX            \n          XXXXXX    XXXXXXXXXX      XXXXXXXXXX            \n        XX  XX  XX    XX  XX    XXXXXX  XX  XX            \n        XX  XX  XX    XX  XX    XXXXXX  XX  XX            \n        XXXXXX      XXXX  XX  XX  XXXX      XX  XX        \n        XXXXXX      XXXX  XX  XX  XXXX      XX  XX        \n                        XXXXXX    XXXX      XX  XX        \n                        XXXXXX    XXXX      XX  XX        \n        XXXXXXXXXXXXXX        XXXXXX            XX        \n        XXXXXXXXXXXXXX        XXXXXX            XX        \n        XX          XX          XX    XX  XX              \n        XX          XX          XX    XX  XX              \n        XX  XXXXXX  XX  XXXXXXXXXX  XXXXXXXXXXXXXX        \n        XX  XXXXXX  XX  XXXXXXXXXX  XXXXXXXXXXXXXX        \n        XX  XXXXXX  XX    XX  XXXX    XX  XX  XXXX        \n        XX  XXXXXX  XX    XX  XXXX    XX  XX  XXXX        \n        XX  XXXXXX  XX    XXXXXX    XXXXXXXXXX            \n        XX  XXXXXX  XX    XXXXXX    XXXXXXXXXX            \n        XX          XX  XX        XXXX  XX  XX  XX        \n        XX          XX  XX        XXXX  XX  XX  XX        \n        XXXXXXXXXXXXXX    XX    XXXXXX      XXXXXX        \n        XXXXXXXXXXXXXX    XX    XXXXXX      XXXXXX        \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          \n                                                          ";

            //Create QR code                   
            var asciiCode = AsciiQRCodeHelper.GetQRCode("A", 2, "X", " ", QRCodeGenerator.ECCLevel.Q);
            asciiCode.ShouldBe(targetCode);
        }
    }
}



