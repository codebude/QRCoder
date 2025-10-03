namespace QRCoderTests.PayloadGeneratorTests;

public class SMSTests
{
    [Fact]
    public void sms_should_build_type_SMS()
    {
        var number = "01601234567";
        var message = "A small SMS";
        var encoding = PayloadGenerator.SMS.SMSEncoding.SMS;

        var generator = new PayloadGenerator.SMS(number, message, encoding);

        generator.ToString().ShouldBe("sms:01601234567?body=A%20small%20SMS");
    }


    [Fact]
    public void sms_should_build_type_SMS_iOS()
    {
        var number = "01601234567";
        var message = "A small SMS";
        var encoding = PayloadGenerator.SMS.SMSEncoding.SMS_iOS;

        var generator = new PayloadGenerator.SMS(number, message, encoding);

        generator.ToString().ShouldBe("sms:01601234567;body=A%20small%20SMS");
    }


    [Fact]
    public void sms_should_build_type_SMSTO()
    {
        var number = "01601234567";
        var message = "A small SMS";
        var encoding = PayloadGenerator.SMS.SMSEncoding.SMSTO;

        var generator = new PayloadGenerator.SMS(number, message, encoding);

        generator.ToString().ShouldBe("SMSTO:01601234567:A small SMS");
    }


    [Fact]
    public void sms_should_not_add_unused_params()
    {
        var number = "01601234567";

        var generator = new PayloadGenerator.SMS(number);

        generator.ToString().ShouldBe("sms:01601234567");
    }
}
