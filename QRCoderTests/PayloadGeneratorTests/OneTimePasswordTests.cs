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

    [Fact]
    public void one_time_password_generator_totp_with_sha256_algorithm()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
            AuthAlgorithm = PayloadGenerator.OneTimePassword.OneTimePasswordAuthAlgorithm.SHA256,
        };

        pg.ToString().ShouldBe("otpauth://totp/Google:test%40google.com?secret=pwq65q55&issuer=Google&algorithm=SHA256");
    }

    [Fact]
    public void one_time_password_generator_totp_with_sha512_algorithm()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
            AuthAlgorithm = PayloadGenerator.OneTimePassword.OneTimePasswordAuthAlgorithm.SHA512,
        };

        pg.ToString().ShouldBe("otpauth://totp/Google:test%40google.com?secret=pwq65q55&issuer=Google&algorithm=SHA512");
    }

    [Fact]
    public void one_time_password_generator_hotp_with_sha256_algorithm()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
            Type = PayloadGenerator.OneTimePassword.OneTimePasswordAuthType.HOTP,
            Counter = 100,
            AuthAlgorithm = PayloadGenerator.OneTimePassword.OneTimePasswordAuthAlgorithm.SHA256,
        };

        pg.ToString().ShouldBe("otpauth://hotp/Google:test%40google.com?secret=pwq65q55&issuer=Google&algorithm=SHA256&counter=100");
    }

    [Fact]
    public void one_time_password_generator_totp_with_8_digits()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
            Digits = 8,
        };

        pg.ToString().ShouldBe("otpauth://totp/Google:test%40google.com?secret=pwq65q55&issuer=Google&digits=8");
    }

    [Fact]
    public void one_time_password_generator_hotp_with_8_digits()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
            Type = PayloadGenerator.OneTimePassword.OneTimePasswordAuthType.HOTP,
            Counter = 250,
            Digits = 8,
        };

        pg.ToString().ShouldBe("otpauth://hotp/Google:test%40google.com?secret=pwq65q55&issuer=Google&digits=8&counter=250");
    }

    [Fact]
    public void one_time_password_generator_totp_with_custom_period()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
            Period = 60,
        };

        pg.ToString().ShouldBe("otpauth://totp/Google:test%40google.com?secret=pwq65q55&issuer=Google&period=60");
    }

    [Fact]
    public void one_time_password_generator_totp_with_all_custom_parameters()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
            AuthAlgorithm = PayloadGenerator.OneTimePassword.OneTimePasswordAuthAlgorithm.SHA512,
            Digits = 8,
            Period = 60,
        };

        pg.ToString().ShouldBe("otpauth://totp/Google:test%40google.com?secret=pwq65q55&issuer=Google&algorithm=SHA512&digits=8&period=60");
    }

    [Fact]
    public void one_time_password_generator_hotp_with_all_custom_parameters()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
            Type = PayloadGenerator.OneTimePassword.OneTimePasswordAuthType.HOTP,
            Counter = 999,
            AuthAlgorithm = PayloadGenerator.OneTimePassword.OneTimePasswordAuthAlgorithm.SHA256,
            Digits = 7,
        };

        pg.ToString().ShouldBe("otpauth://hotp/Google:test%40google.com?secret=pwq65q55&issuer=Google&algorithm=SHA256&digits=7&counter=999");
    }

    [Fact]
    public void one_time_password_generator_hotp_with_default_counter()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
            Type = PayloadGenerator.OneTimePassword.OneTimePasswordAuthType.HOTP,
            // Counter not set, should default to 1
        };

        pg.ToString().ShouldBe("otpauth://hotp/Google:test%40google.com?secret=pwq65q55&issuer=Google&counter=1");
    }

    [Fact]
    public void one_time_password_generator_totp_default_period_not_included()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
            Period = 30, // Default value, should not be included in output
        };

        pg.ToString().ShouldBe("otpauth://totp/Google:test%40google.com?secret=pwq65q55&issuer=Google");
    }

    [Fact]
    public void one_time_password_generator_totp_default_digits_not_included()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
            Digits = 6, // Default value, should not be included in output
        };

        pg.ToString().ShouldBe("otpauth://totp/Google:test%40google.com?secret=pwq65q55&issuer=Google");
    }

    [Fact]
    public void one_time_password_generator_totp_default_algorithm_not_included()
    {
        var pg = new PayloadGenerator.OneTimePassword
        {
            Secret = "pwq6 5q55",
            Issuer = "Google",
            Label = "test@google.com",
            AuthAlgorithm = PayloadGenerator.OneTimePassword.OneTimePasswordAuthAlgorithm.SHA1, // Default value, should not be included in output
        };

        pg.ToString().ShouldBe("otpauth://totp/Google:test%40google.com?secret=pwq65q55&issuer=Google");
    }
}
