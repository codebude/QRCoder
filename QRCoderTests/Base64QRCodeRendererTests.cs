#if !NETCOREAPP1_1
using Xunit;
using QRCoder;
using Shouldly;
using QRCoderTests.Helpers.XUnitExtenstions;
using QRCoderTests.Helpers;
using System;

using System.Drawing;
using System.IO;
using System.Security.Policy;

namespace QRCoderTests
{
    /****************************************************************************************************
     * Note: Test cases compare the outcome visually even if it's slower than a byte-wise compare.
     *       This is necessary, because the Deflate implementation differs on the different target
     *       platforms and thus the outcome, even if visually identical, differs. Thus only a visual
     *       test method makes sense. In addition bytewise differences shouldn't be important, if the
     *       visual outcome is identical and thus the qr code is identical/scannable.
     ****************************************************************************************************/
    public class Base64QRCodeRendererTests
    {
        private readonly QRCodeData data;

        public Base64QRCodeRendererTests()
        {
            var gen = new QRCodeGenerator();
            data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        }

        [Fact]
        [Category("QRRenderer/Base64QRCode")]
        public void can_render_base64_qrcode_blackwhite()
        {
            var pngCodeGfx = new PngByteQRCode(data).GetGraphic(5);
            var base64QRCode = new Base64QRCode(data).GetGraphic(5);
            base64QRCode.ShouldBe(Convert.ToBase64String(pngCodeGfx));
        }

        [Fact]
        [Category("QRRenderer/Base64QRCode")]
        public void can_render_base64_qrcode_noquietzones()
        {
            var pngCodeGfx = new PngByteQRCode(data).GetGraphic(5, false);
            var base64QRCode = new Base64QRCode(data).GetGraphic(5, Color.Black, Color.White, false);
            base64QRCode.ShouldBe(Convert.ToBase64String(pngCodeGfx));
        }

        [Fact]
        [Category("QRRenderer/Base64QRCode")]
        public void can_render_base64_qrcode_color()
        {
            var pngCodeGfx = new PngByteQRCode(data).GetGraphic(5, new byte[] { 255, 0, 0 }, new byte[] { 0, 0, 255 });
            var base64QRCode = new Base64QRCode(data).GetGraphic(5, Color.Red, Color.Blue);
            base64QRCode.ShouldBe(Convert.ToBase64String(pngCodeGfx));
        }

        [Fact]
        [Category("QRRenderer/Base64QRCode")]
        public void can_render_base64_qrcode_transparent()
        {
            var pngCodeGfx = new PngByteQRCode(data).GetGraphic(5, new byte[] { 0, 255, 0, 255 }, new byte[] { 255, 255, 255, 0 });
            var base64QRCode = new Base64QRCode(data).GetGraphic(5, Color.Lime, Color.Transparent);
            base64QRCode.ShouldBe(Convert.ToBase64String(pngCodeGfx));
        }

#if NETFRAMEWORK || NETCOREAPP2_0 || NET5_0 || NET6_0_WINDOWS
        [Fact]
        [Category("QRRenderer/Base64QRCode")]
        public void can_render_base64_qrcode_jpeg()
        {
            var ms = new MemoryStream();
            using (var bitmap = new QRCode(data).GetGraphic(5, Color.Black, Color.White, true))
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            ms.Position = 0;
            var jpgString = Convert.ToBase64String(ms.ToArray());
            var base64QRCode = new Base64QRCode(data).GetGraphic(5, Color.Black, Color.White, true, Base64QRCode.ImageType.Jpeg);
            base64QRCode.ShouldBe(jpgString);
        }
#endif
    }
}

#endif
