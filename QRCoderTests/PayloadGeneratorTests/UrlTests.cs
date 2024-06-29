using QRCoder;
using Shouldly;
using Xunit;

namespace QRCoderTests.PayloadGeneratorTests;

public class UrlTests
{
    [Fact]
    public void url_should_build_http()
    {
        var url = "http://code-bude.net";

        var generator = new PayloadGenerator.Url(url);

        generator.ToString().ShouldBe("http://code-bude.net");
    }


    [Fact]
    public void url_should_build_https()
    {
        var url = "https://code-bude.net";

        var generator = new PayloadGenerator.Url(url);

        generator.ToString().ShouldBe("https://code-bude.net");
    }


    [Fact]
    public void url_should_build_https_all_caps()
    {
        var url = "HTTPS://CODE-BUDE.NET";

        var generator = new PayloadGenerator.Url(url);

        generator.ToString().ShouldBe("HTTPS://CODE-BUDE.NET");
    }


    [Fact]
    public void url_should_add_http()
    {
        var url = "code-bude.net";

        var generator = new PayloadGenerator.Url(url);

        generator.ToString().ShouldBe("http://code-bude.net");
    }
}
