using QRCoder;
using Shouldly;
using Xunit;

namespace QRCoderTests.PayloadGeneratorTests;

public class MMSTests
{
    [Fact]
    public void mms_should_build_type_MMS()
    {
        var number = "01601234567";
        var message = "A tiny MMS";
        var encoding = PayloadGenerator.MMS.MMSEncoding.MMS;

        var generator = new PayloadGenerator.MMS(number, message, encoding);

        generator.ToString().ShouldBe("mms:01601234567?body=A%20tiny%20MMS");
    }


    [Fact]
    public void mms_should_build_type_MMSTO()
    {
        var number = "01601234567";
        var message = "A tiny SMS";
        var encoding = PayloadGenerator.MMS.MMSEncoding.MMSTO;

        var generator = new PayloadGenerator.MMS(number, message, encoding);

        generator.ToString().ShouldBe("mmsto:01601234567?subject=A%20tiny%20SMS");
    }


    [Fact]
    public void mms_should_not_add_unused_params()
    {
        var number = "01601234567";

        var generator = new PayloadGenerator.MMS(number);

        generator.ToString().ShouldBe("mms:01601234567");
    }
}
