using QRCoder;
using Shouldly;
using Xunit;

namespace QRCoderTests.PayloadGeneratorTests;

public class ShadowSocksConfigTests
{
    [Fact]
    public void shadowsocks_generator_can_generate_payload()
    {
        var host = "192.168.2.5";
        var port = 1;
        var password = "s3cr3t";
        var method = PayloadGenerator.ShadowSocksConfig.Method.Rc4Md5;
        var generator = new PayloadGenerator.ShadowSocksConfig(host, port, password, method);

        generator
            .ToString()
            .ShouldBe("ss://cmM0LW1kNTpzM2NyM3RAMTkyLjE2OC4yLjU6MQ==");
    }

    [Fact]
    public void shadowsocks_generator_can_generate_payload_with_tag()
    {
        var host = "192.168.2.5";
        var port = 65535;
        var password = "s3cr3t";
        var method = PayloadGenerator.ShadowSocksConfig.Method.Rc4Md5;
        var tag = "server42";
        var generator = new PayloadGenerator.ShadowSocksConfig(host, port, password, method, tag);

        generator
            .ToString()
            .ShouldBe("ss://cmM0LW1kNTpzM2NyM3RAMTkyLjE2OC4yLjU6NjU1MzU=#server42");
    }


    [Fact]
    public void shadowsocks_generator_should_throw_portrange_low_exception()
    {
        var host = "192.168.2.5";
        var port = 0;
        var password = "s3cr3t";
        var method = PayloadGenerator.ShadowSocksConfig.Method.Rc4Md5;

        var exception = Record.Exception(() => new PayloadGenerator.ShadowSocksConfig(host, port, password, method));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<PayloadGenerator.ShadowSocksConfig.ShadowSocksConfigException>();
        exception.Message.ShouldBe("Value of 'port' must be within 0 and 65535.");
    }


    [Fact]
    public void shadowsocks_generator_should_throw_portrange_high_exception()
    {
        var host = "192.168.2.5";
        var port = 65536;
        var password = "s3cr3t";
        var method = PayloadGenerator.ShadowSocksConfig.Method.Rc4Md5;

        var exception = Record.Exception(() => new PayloadGenerator.ShadowSocksConfig(host, port, password, method));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<PayloadGenerator.ShadowSocksConfig.ShadowSocksConfigException>();
        exception.Message.ShouldBe("Value of 'port' must be within 0 and 65535.");
    }


    [Fact]
    public void shadowsocks_generator_can_generate_payload_with_plugin()
    {
        var host = "192.168.100.1";
        var port = 8888;
        var password = "test";
        var method = PayloadGenerator.ShadowSocksConfig.Method.BfCfb;
        var plugin = "obfs-local";
        var pluginOption = "obfs=http;obfs-host=google.com";
        var generator = new PayloadGenerator.ShadowSocksConfig(host, port, password, method, plugin, pluginOption);

        generator
            .ToString()
            .ShouldBe("ss://YmYtY2ZiOnRlc3Q@192.168.100.1:8888/?plugin=obfs-local%3bobfs%3dhttp%3bobfs-host%3dgoogle.com");
    }
}
