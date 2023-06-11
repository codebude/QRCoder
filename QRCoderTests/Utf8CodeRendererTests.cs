using QRCoder;
using QRCoderTests.Helpers.XUnitExtenstions;
using Shouldly;
using Xunit;


namespace QRCoderTests
{

    public class Utf8CodeRendererTests
    {
        [Fact]
        [Category("QRRenderer/Utf8QRCode")]
        public void can_render_utf8_qrcode()
        {
            var targetCode = "                             \n                             \n    █▀▀▀▀▀█ ▄▀ █▄ █▀▀▀▀▀█    \n    █ ███ █ ▀ █ ▀ █ ███ █    \n    █ ▀▀▀ █ ▀▄▄▄▄ █ ▀▀▀ █    \n    ▀▀▀▀▀▀▀ █ █▄▀ ▀▀▀▀▀▀▀    \n    ██▀▀█ ▀█  ▄█▀▀▀▄█▄█▀▄    \n    ▄ ▀ █ ▀██▀█▄▀▄█ ▀ ▀▀     \n     ▀▀▀▀ ▀▀▀    ▄▄██ ▀ ▀    \n    █▀▀▀▀▀█ ▀▀ ▀▀▄█▄█▀ ▀▀    \n    █ ███ █ █▄█ ▀▄ █  ▀ ▀    \n    █ ▀▀▀ █ █▄▀▄█ ▀ ▀█ █▀    \n    ▀▀▀▀▀▀▀ ▀▀▀     ▀ ▀▀▀    \n                             \n                             ";

            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
            var utf8Code = new Utf8QRCode(data).GetGraphic();

            utf8Code.ShouldBe(targetCode);
        }

        [Fact]
        [Category("QRRenderer/Utf8QRCode")]
        public void can_render_utf8_qrcode_without_quietzones()
        {
            var targetCode = "█▀▀▀▀▀█ ▄▀ █▄ █▀▀▀▀▀█\n█ ███ █ ▀ █ ▀ █ ███ █\n█ ▀▀▀ █ ▀▄▄▄▄ █ ▀▀▀ █\n▀▀▀▀▀▀▀ █ █▄▀ ▀▀▀▀▀▀▀\n██▀▀█ ▀█  ▄█▀▀▀▄█▄█▀▄\n▄ ▀ █ ▀██▀█▄▀▄█ ▀ ▀▀ \n ▀▀▀▀ ▀▀▀    ▄▄██ ▀ ▀\n█▀▀▀▀▀█ ▀▀ ▀▀▄█▄█▀ ▀▀\n█ ███ █ █▄█ ▀▄ █  ▀ ▀\n█ ▀▀▀ █ █▄▀▄█ ▀ ▀█ █▀\n▀▀▀▀▀▀▀ ▀▀▀     ▀ ▀▀▀";

            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
            var utf8Code = new Utf8QRCode(data).GetGraphic(drawQuietZones: false);

            utf8Code.ShouldBe(targetCode);
        }

        [Fact]
        [Category("QRRenderer/Utf8QRCode")]
        public void can_render_utf8_qrcode_inverted()
        {
            var targetCode = "█████████████████████████████\n█████████████████████████████\n████ ▄▄▄▄▄ █▀▀▀▄ █ ▄▄▄▄▄ ████\n████ █   █ ██  ▄▄█ █   █ ████\n████ █▄▄▄█ █▄▀█▄██ █▄▄▄█ ████\n████▄▄▄▄▄▄▄█▄▀ ▀ █▄▄▄▄▄▄▄████\n████▀█ ▀ ▄▄▀ ▄ ▀▀ ██ ▀▀▄▀████\n█████▀████▄▀▄█▄█▀▄▀█ ▀ ██████\n█████▄█▄▄▄▄█ ▄█ ▀▀ ▄▄▄█▀ ████\n████ ▄▄▄▄▄ █▀▀ ▄▄▄█▀ ▀ ▄▄████\n████ █   █ █▄▀ ▄███▀ ▀ ▄█████\n████ █▄▄▄█ █▄▀▄▄▄▄██ ▀▄▄ ████\n████▄▄▄▄▄▄▄██▄██▄▄▄█▄▄█▄▄████\n█████████████████████████████\n█████████████████████████████";

            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A", QRCodeGenerator.ECCLevel.Q);
            var utf8Code = new Utf8QRCode(data).GetGraphic(invert: true);

            utf8Code.ShouldBe(targetCode);
        }

        [Fact]
        [Category("QRRenderer/Utf8QRCode")]
        public void can_instantate_parameterless()
        {
            var utf8Code = new Utf8QRCode();
            utf8Code.ShouldNotBeNull();
            utf8Code.ShouldBeOfType<Utf8QRCode>();
        }

        [Fact]
        [Category("QRRenderer/Utf8QRCode")]
        public void can_render_utf8_qrcode_from_helper()
        {
            var targetCode = "                             \n                             \n    █▀▀▀▀▀█ ▄▄▄▀█ █▀▀▀▀▀█    \n    █ ███ █  ██▀▀ █ ███ █    \n    █ ▀▀▀ █ ▀▄ ▀  █ ▀▀▀ █    \n    ▀▀▀▀▀▀▀ ▀▄█▄█ ▀▀▀▀▀▀▀    \n    ▄ █▄█▀▀▄█▀█▄▄█  █▄▄▀▄    \n     ▄    ▀▄▀ ▀ ▄▀▄ █▄█      \n     ▀ ▀▀▀▀ █▀ █▄▄█▀▀▀ ▄█    \n    █▀▀▀▀▀█ ▄▄█▀▀▀ ▄█▄█▀▀    \n    █ ███ █ ▀▄█▀   ▄█▄█▀     \n    █ ▀▀▀ █ ▀▄▀▀▀▀  █▄▀▀█    \n    ▀▀▀▀▀▀▀  ▀  ▀▀▀ ▀▀ ▀▀    \n                             \n                             ";

            //Create QR code                   
            var utf8Code = Utf8QRCodeHelper.GetQRCode("A", QRCodeGenerator.ECCLevel.Q);
            utf8Code.ShouldBe(targetCode);
        }
    }
}