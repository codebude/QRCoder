using System;
using Xunit;
using QRCoder;
using Shouldly;
using System.IO;
using System.Security.Cryptography;
using QRCoderTests.Helpers.XUnitExtenstions;
using QRCoderTests.Helpers;
#if !NETCOREAPP1_1
using System.Drawing;
#endif

namespace QRCoderTests
{

    public class QRCodeRendererTests
    {

#if !NETCOREAPP1_1
        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_standard_qrcode_graphic()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new QRCode(data).GetGraphic(10);

            var result = HelperFunctions.BitmapToHash(bmp);
            result.ShouldBe("e8c61b8f0455924fe08ba68686d0d296");
        }
#endif 


#if !NETCOREAPP1_1 && !NETCOREAPP2_0

        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_qrcode_with_transparent_logo_graphic()
        {        
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

            var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.Transparent, icon: (Bitmap)Image.FromFile(HelperFunctions.GetAssemblyPath() + "\\assets\\noun_software engineer_2909346.png"));
            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346

            var result = HelperFunctions.BitmapToHash(bmp);
#if NET35_OR_GREATER || NET40_OR_GREATER
            result.ShouldBe("ee65d96c3013f6032b561cc768251eef");
#else
            result.ShouldBe("150f8fc7dae4487ba2887d2b2bea1c25");
#endif
        }

        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_qrcode_with_non_transparent_logo_graphic()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.White, icon: (Bitmap)Bitmap.FromFile(HelperFunctions.GetAssemblyPath() + "\\assets\\noun_software engineer_2909346.png"));
            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346

            var result = HelperFunctions.BitmapToHash(bmp);
#if NET35_OR_GREATER || NET40_OR_GREATER
            result.ShouldBe("1d718f06f904af4a46748f02af2d4eec");
#else
            result.ShouldBe("c46a7ec51bf978d7a882059c322ca69d");
#endif
        }

        /*
        private static byte[] PixelsToAveragedByteArray(Bitmap bmp)
        {
            //Re-color
            var bmpTmp = new Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            using (var gr = Graphics.FromImage(bmp))
                gr.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            
            //Downscale
            var bmpSmall = new Bitmap(bmpTmp, new Size(16, 16));

            var bytes = new System.Collections.Generic.List<byte>();
            for (int x = 0; x < bmpSmall.Width; x++)
            {
                for (int y = 0; y < bmpSmall.Height; y++)
                {
                    bytes.AddRange(new byte[] { bmpSmall.GetPixel(x, y).R, bmpSmall.GetPixel(x, y).G, bmpSmall.GetPixel(x, y).B });
                }
            }
            return bytes.ToArray();
        }
        */
#endif
    }
}