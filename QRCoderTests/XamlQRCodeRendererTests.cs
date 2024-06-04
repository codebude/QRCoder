#if TEST_XAML
using Xunit;
using QRCoder;
using QRCoder.Xaml;
using Shouldly;
using QRCoderTests.Helpers.XUnitExtenstions;
using QRCoderTests.Helpers;

namespace QRCoderTests;


public class XamlQRCodeRendererTests
{

    [Fact]
    [Category("QRRenderer/XamlQRCode")]
    public void can_create_xaml_qrcode_standard_graphic()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var xCode = new XamlQRCode(data).GetGraphic(10);

        var bmp = HelperFunctions.BitmapSourceToBitmap(xCode);
        var result = HelperFunctions.BitmapToHash(bmp);
        result.ShouldBe("f2ed5073bd42dc012e442c0f750e9dae");
    }


    [Fact]
    [Category("QRRenderer/XamlQRCode")]
    public void can_instantate_qrcode_parameterless()
    {
        var svgCode = new XamlQRCode();
        svgCode.ShouldNotBeNull();
        svgCode.ShouldBeOfType<XamlQRCode>();
    }

    /*
    [Fact]
    [Category("QRRenderer/XamlQRCode")]
    public void can_render_qrcode_from_helper()
    {
        //Create QR code                   
        var bmp = QRCodeHelper.GetQRCode("This is a quick test! 123#?", 10, Color.Black, Color.White, QRCodeGenerator.ECCLevel.H);

        var result = HelperFunctions.BitmapToHash(bmp);
        result.ShouldBe("e8c61b8f0455924fe08ba68686d0d296");
    }
    */
}
#endif
