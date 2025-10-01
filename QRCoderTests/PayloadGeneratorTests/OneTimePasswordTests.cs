using QRCoder;
using Shouldly;
using Xunit;

namespace QRCoderTests.PayloadGeneratorTests;

public class OneTimePasswordTests
{
    [Fact]
    public void one_time_password_generator_time_based_generates_with_standard_options()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
        };

        pg.ToString().ShouldBe("otpauth://totp/Google:test%40google.com?secret=pwq65q55&issuer=Google");
    }


    [Fact]
    public void one_time_password_generator_time_based_generates_with_standard_options_escapes_issuer_and_label()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google Google",
            Label = "test/test@google.com",
        };

        pg.ToString().ShouldBe("otpauth://totp/Google%20Google:test%2Ftest%40google.com?secret=pwq65q55&issuer=Google%20Google");
    }


    [Fact]
    public void one_time_password_generator_hmac_based_generates_with_standard_options()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
            Type = PayloadGenerator.OneTimePassword.OneTimePasswordAuthType.HOTP,
            Counter = 500,
        };

        pg.ToString().ShouldBe("otpauth://hotp/Google:test%40google.com?secret=pwq65q55&issuer=Google&counter=500");
    }

    [Fact]
    public void one_time_password_generator_hmac_based_generates_with_standard_options_escapes_issuer_and_label()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google Google",
            Label = "test/test@google.com",
            Type = PayloadGenerator.OneTimePassword.OneTimePasswordAuthType.HOTP,
            Counter = 500,
        };

        pg.ToString().ShouldBe("otpauth://hotp/Google%20Google:test%2Ftest%40google.com?secret=pwq65q55&issuer=Google%20Google&counter=500");
    }
}
