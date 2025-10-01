using QRCoder;
using Shouldly;
using Xunit;

namespace QRCoderTests.PayloadGeneratorTests;

public class MoneroTransactionTests
{
    [Fact]
    public void monero_generator_can_generate_payload_simple()
    {
        var address = "46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em";
        var generator = new PayloadGenerator.MoneroTransaction(address);

        generator
            .ToString()
            .ShouldBe("monero://46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em");
    }

    [Fact]
    public void monero_generator_can_generate_payload_first_param()
    {
        var address = "46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em";
        var amount = 1.3f;
        var generator = new PayloadGenerator.MoneroTransaction(address, amount);

        generator
            .ToString()
            .ShouldBe("monero://46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em?tx_amount=1.3");
    }


    [Fact]
    public void monero_generator_can_generate_payload_named_param()
    {
        var address = "46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em";
        var recipient = "Raffael Herrmann";
        var generator = new PayloadGenerator.MoneroTransaction(address, recipientName: recipient);

        generator
            .ToString()
            .ShouldBe("monero://46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em?recipient_name=Raffael%20Herrmann");
    }


    [Fact]
    public void monero_generator_can_generate_payload_full_param()
    {
        var address = "46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em";
        var amount = 1.3f;
        var paymentId = "1234567890123456789012345678901234567890123456789012345678901234";
        var recipient = "Raffael Herrmann";
        var description = "Monero transaction via QrCoder.NET.";
        var generator = new PayloadGenerator.MoneroTransaction(address, amount, paymentId, recipient, description);

        generator
            .ToString()
            .ShouldBe("monero://46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em?tx_payment_id=1234567890123456789012345678901234567890123456789012345678901234&recipient_name=Raffael%20Herrmann&tx_amount=1.3&tx_description=Monero%20transaction%20via%20QrCoder.NET.");
    }


    [Fact]
    public void monero_generator_should_throw_wrong_amount_exception()
    {
        var address = "46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em";
        var amount = -1f;

        var exception = Record.Exception(() => new PayloadGenerator.MoneroTransaction(address, amount));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.MoneroTransaction.MoneroTransactionException>(exception);
        exception.Message.ShouldBe("Value of 'txAmount' must be greater than 0.");
    }


    [Fact]
    public void monero_generator_should_throw_no_address_exception()
    {
        var address = "";

        var exception = Record.Exception(() => new PayloadGenerator.MoneroTransaction(address));

        Assert.NotNull(exception);
        Assert.IsType<PayloadGenerator.MoneroTransaction.MoneroTransactionException>(exception);
        exception.Message.ShouldBe("The address is mandatory and has to be set.");
    }
}
