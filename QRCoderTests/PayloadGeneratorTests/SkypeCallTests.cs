using QRCoder;
using Shouldly;
using Xunit;

namespace QRCoderTests.PayloadGeneratorTests;

public class SkypeCallTests
{
    [Fact]
    public void skype_should_build()
    {
        var username = "johndoe123";

        var generator = new PayloadGenerator.SkypeCall(username);

        generator.ToString().ShouldBe("skype:johndoe123?call");
    }
}
