using System.Globalization;
using System.Threading;
using QRCoder;
using Shouldly;
using Xunit;

namespace QRCoderTests.PayloadGeneratorTests;

public class LitecoinAddressTests
{
    [Fact]
    public void litecoin_address_generator_can_generate_address()
    {
        var address = "LY1t7iLnwtPCb1DPZP38FA835XzFqXBq54";
        var amount = .123;
        var label = "Some Label to Encode";
        var message = "Some Message to Encode";

        var generator = new PayloadGenerator.LitecoinAddress(address, amount, label, message);

        generator
            .ToString()
            .ShouldBe("litecoin:LY1t7iLnwtPCb1DPZP38FA835XzFqXBq54?label=Some%20Label%20to%20Encode&message=Some%20Message%20to%20Encode&amount=.123");
    }

    [Fact]
    public void litecoin_address_generator_should_skip_missing_label()
    {
        var address = "LY1t7iLnwtPCb1DPZP38FA835XzFqXBq54";
        var amount = .123;
        var message = "Some Message to Encode";


        var generator = new PayloadGenerator.LitecoinAddress(address, amount, null, message);

        generator
            .ToString()
            .ShouldNotContain("label");
    }

    [Fact]
    public void litecoin_address_generator_should_skip_missing_message()
    {
        var address = "LY1t7iLnwtPCb1DPZP38FA835XzFqXBq54";
        var amount = .123;


        var generator = new PayloadGenerator.LitecoinAddress(address, amount);

        generator
            .ToString()
            .ShouldNotContain("message");
    }

    [Fact]
    public void litecoin_address_generator_should_round_to_satoshi()
    {
        var address = "LY1t7iLnwtPCb1DPZP38FA835XzFqXBq54";
        var amount = .123456789;


        var generator = new PayloadGenerator.LitecoinAddress(address, amount);

        generator
            .ToString()
            .ShouldContain("amount=.12345679");
    }

    [Fact]
    public void litecoin_address_generator_disregards_current_culture()
    {
#if NETCOREAPP1_1
        var currentCulture = CultureInfo.DefaultThreadCurrentCulture;
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("de-DE");
#else
        var currentCulture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
#endif

        var address = "LY1t7iLnwtPCb1DPZP38FA835XzFqXBq54";
        var amount = .123;


        var generator = new PayloadGenerator.LitecoinAddress(address, amount);

        generator
            .ToString()
            .ShouldBe("litecoin:LY1t7iLnwtPCb1DPZP38FA835XzFqXBq54?amount=.123");

#if NETCOREAPP1_1
        CultureInfo.DefaultThreadCurrentCulture = currentCulture;
#else
        Thread.CurrentThread.CurrentCulture = currentCulture;
#endif
    }
}
