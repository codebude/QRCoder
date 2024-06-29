using QRCoder;
using Shouldly;
using Xunit;

namespace QRCoderTests.PayloadGeneratorTests;

public class BookmarkTests
{
    [Fact]
    public void bookmark_should_build()
    {
        var url = "http://code-bude.net";
        var title = "A nerd's blog";

        var generator = new PayloadGenerator.Bookmark(url, title);

        generator.ToString().ShouldBe("MEBKM:TITLE:A nerd's blog;URL:http\\://code-bude.net;;");
    }


    [Fact]
    public void bookmark_should_escape_input()
    {
        var url = "http://code-bude.net/fake,url.html";
        var title = "A nerd's blog: \\All;the;things\\";

        var generator = new PayloadGenerator.Bookmark(url, title);

        generator.ToString().ShouldBe("MEBKM:TITLE:A nerd's blog\\: \\\\All\\;the\\;things\\\\;URL:http\\://code-bude.net/fake\\,url.html;;");
    }
}
