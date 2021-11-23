using Xunit;
using QRCoder;
using Shouldly;
using QRCoderTests.Helpers.XUnitExtenstions;
using QRCoderTests.Helpers;
#if !NETCOREAPP1_1
using System.Drawing;
using System.IO;
#endif

namespace QRCoderTests
{
    /****************************************************************************************************
     * Note: Test cases compare the outcome visually even if it's slower than a byte-wise compare.
     *       This is necessary, because the Deflate implementation differs on the different target
     *       platforms and thus the outcome, even if visually identical, differs. Thus only a visual
     *       test method makes sense. In addition bytewise differences shouldn't be important, if the
     *       visual outcome is identical and thus the qr code is identical/scannable.
     ****************************************************************************************************/
    public class PngByteQRCodeRendererTests
    {


        [Fact]
        [Category("QRRenderer/PngByteQRCode")]
        public void can_render_pngbyte_qrcode_blackwhite()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
            var pngCodeGfx = new PngByteQRCode(data).GetGraphic(5);

#if NETCOREAPP1_1
            var result = HelperFunctions.ByteArrayToHash(pngCodeGfx);
            result.ShouldBe("1fc35c3bea6fad47427143ce716c83b8");
#else
            using (var mStream = new MemoryStream(pngCodeGfx))
            {
                var bmp = (Bitmap)Image.FromStream(mStream);
                var result = HelperFunctions.BitmapToHash(bmp);
                result.ShouldBe("18b19e6037cff06ae995d8d487b0e46e");
            }
#endif        
        }

        [Fact]
        [Category("QRRenderer/PngByteQRCode")]
        public void can_render_pngbyte_qrcode_color()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
            var pngCodeGfx = new PngByteQRCode(data).GetGraphic(5, new byte[] { 255, 0, 0 }, new byte[] { 0, 0, 255 });

#if NETCOREAPP1_1
            var result = HelperFunctions.ByteArrayToHash(pngCodeGfx);
            result.ShouldBe("0144b1d40aa6eeb6cb07df42822ea0a7");
#else
            using (var mStream = new MemoryStream(pngCodeGfx))
            {
                var bmp = (Bitmap)Image.FromStream(mStream);
                var result = HelperFunctions.BitmapToHash(bmp);
                result.ShouldBe("37ae73e90b66beac317b790be3db24cc");
            }
#endif   
        }


        [Fact]
        [Category("QRRenderer/PngByteQRCode")]
        public void can_render_pngbyte_qrcode_color_with_alpha()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
            var pngCodeGfx = new PngByteQRCode(data).GetGraphic(5, new byte[] { 255, 255, 255, 127 }, new byte[] { 0, 0, 255 });

#if NETCOREAPP1_1
            var result = HelperFunctions.ByteArrayToHash(pngCodeGfx);
            result.ShouldBe("627ce564fb5e17be42e4a85e907a17b5");
#else
            using (var mStream = new MemoryStream(pngCodeGfx))
            {
                var bmp = (Bitmap)Image.FromStream(mStream);
                var result = HelperFunctions.BitmapToHash(bmp);
                result.ShouldBe("c56c2a9535fd8e9a92a6ac9709d21e67");
            }
#endif   
        }

        [Fact]
        [Category("QRRenderer/PngByteQRCode")]
        public void can_render_pngbyte_qrcode_color_without_quietzones()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
            var pngCodeGfx = new PngByteQRCode(data).GetGraphic(5, new byte[] { 255, 255, 255, 127 }, new byte[] { 0, 0, 255 }, false);

#if NETCOREAPP1_1
            var result = HelperFunctions.ByteArrayToHash(pngCodeGfx);
            result.ShouldBe("07f760b3eb54901840b094d31e299713");
#else
            File.WriteAllBytes(@"C:\Temp\pngbyte_35.png", pngCodeGfx);
            using (var mStream = new MemoryStream(pngCodeGfx))
            {
                var bmp = (Bitmap)Image.FromStream(mStream);
                bmp.MakeTransparent(Color.Transparent);
                var result = HelperFunctions.BitmapToHash(bmp);
#if NET35_OR_GREATER || NET40_OR_GREATER
                result.ShouldBe("75be11d582575617d2490c54b69e844e");
#else
                result.ShouldBe("fbbc8255ebf3e4f4a1d21f0dd15f76f8");
#endif
            }
#endif
        }

        [Fact]
        [Category("QRRenderer/PngByteQRCode")]
        public void can_instantate_pngbyte_qrcode_parameterless()
        {
            var pngCode = new PngByteQRCode();
            pngCode.ShouldNotBeNull();
            pngCode.ShouldBeOfType<PngByteQRCode>();
        }

        [Fact]
        [Category("QRRenderer/PngByteQRCode")]
        public void can_render_pngbyte_qrcode_from_helper()
        {
            //Create QR code                   
            var pngCodeGfx = PngByteQRCodeHelper.GetQRCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L, 10);

#if NETCOREAPP1_1
            var result = HelperFunctions.ByteArrayToHash(pngCodeGfx);
            result.ShouldBe("c562388f4f3cf13a299b469a3e3b852f");
#else
            using (var mStream = new MemoryStream(pngCodeGfx))
            {
                var bmp = (Bitmap)Image.FromStream(mStream);
                var result = HelperFunctions.BitmapToHash(bmp);
                result.ShouldBe("1978fb11ce26acf9b6cb7490b4c44ef2");
            }
#endif  
        }

        [Fact]
        [Category("QRRenderer/PngByteQRCode")]
        public void can_render_pngbyte_qrcode_from_helper_2()
        {
            //Create QR code                   
            var pngCodeGfx = PngByteQRCodeHelper.GetQRCode("This is a quick test! 123#?", 5, new byte[] { 255, 255, 255, 127 }, new byte[] { 0, 0, 255 }, QRCodeGenerator.ECCLevel.L);

#if NETCOREAPP1_1
            var result = HelperFunctions.ByteArrayToHash(pngCodeGfx);
            result.ShouldBe("627ce564fb5e17be42e4a85e907a17b5");
#else
            using (var mStream = new MemoryStream(pngCodeGfx))
            {
                var bmp = (Bitmap)Image.FromStream(mStream);
                var result = HelperFunctions.BitmapToHash(bmp);
                result.ShouldBe("c56c2a9535fd8e9a92a6ac9709d21e67");
            }
#endif  
        }

    }
}



