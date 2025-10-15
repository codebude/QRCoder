namespace QRCoderTests.PayloadGeneratorTests;

public class IbanTests
{
    [Fact]
    public void iban_validator_validate_german_iban()
    {
        var iban = "DE15268500010154131577";

        var result = PayloadGenerator.IsValidIban(iban);

        result.ShouldBe<bool>(true);
    }

    [Fact]
    public void iban_validator_validate_swiss_iban()
    {
        var iban = "CH1900767000U00121977";

        var result = PayloadGenerator.IsValidIban(iban);

        result.ShouldBe<bool>(true);
    }

    [Fact]
    public void iban_validator_invalidates_iban()
    {
        var iban = "DE29268500010154131577";

        var result = PayloadGenerator.IsValidIban(iban);

        result.ShouldBe<bool>(false);
    }

    [Fact]
    public void qriban_validator_validates_iban()
    {
        var iban = "CH2430043000000789012";

        var result = PayloadGenerator.IsValidQRIban(iban);

        result.ShouldBe<bool>(true);
    }

    [Fact]
    public void qriban_validator_invalidates_iban()
    {
        var iban = "CH3908704016075473007";

        var result = PayloadGenerator.IsValidQRIban(iban);

        result.ShouldBe<bool>(false);
    }
}
