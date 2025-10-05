namespace QRCoderTests.PayloadGeneratorTests;

public class WiFiTests
{
    [Fact]
    public void wifi_should_build_wep()
    {
        var ssid = "MyWiFiSSID";
        var password = "7heP4assw0rd";
        var authmode = PayloadGenerator.WiFi.Authentication.WEP;
        var hideSSID = false;

        var generator = new PayloadGenerator.WiFi(ssid, password, authmode, hideSSID);

        generator.ToString().ShouldBe($"WIFI:T:WEP;S:MyWiFiSSID;P:7heP4assw0rd;;");
    }

    [Fact]
    public void wifi_should_build_wpa()
    {
        var ssid = "MyWiFiSSID";
        var password = "7heP4assw0rd";
        var authmode = PayloadGenerator.WiFi.Authentication.WPA;
        var hideSSID = false;

        var generator = new PayloadGenerator.WiFi(ssid, password, authmode, hideSSID);

        generator.ToString().ShouldBe($"WIFI:T:WPA;S:MyWiFiSSID;P:7heP4assw0rd;;");
    }

    [Fact]
    public void wifi_should_build_wpa2()
    {
        var ssid = "MyWiFiSSID";
        var password = "7heP4assw0rd";
        var authmode = PayloadGenerator.WiFi.Authentication.WPA2;
        var hideSSID = false;

        var generator = new PayloadGenerator.WiFi(ssid, password, authmode, hideSSID);

        generator.ToString().ShouldBe($"WIFI:T:WPA2;S:MyWiFiSSID;P:7heP4assw0rd;;");
    }

    [Fact]
    public void wifi_should_ignore_hiddenSSID_param()
    {
        var ssid = "MyWiFiSSID";
        var password = "7heP4assw0rd";
        var authmode = PayloadGenerator.WiFi.Authentication.WPA;

        var generator = new PayloadGenerator.WiFi(ssid, password, authmode);

        generator.ToString().ShouldBe($"WIFI:T:WPA;S:MyWiFiSSID;P:7heP4assw0rd;;");
    }


    [Fact]
    public void wifi_should_add_hiddenSSID_param()
    {
        var ssid = "M\\y;W,i:FiSSID";
        var password = "7heP4assw0rd\\;:,";
        var authmode = PayloadGenerator.WiFi.Authentication.WPA;
        var hideSSID = true;

        var generator = new PayloadGenerator.WiFi(ssid, password, authmode, hideSSID);

        generator.ToString().ShouldBe($"WIFI:T:WPA;S:M\\\\y\\;W\\,i\\:FiSSID;P:7heP4assw0rd\\\\\\;\\:\\,;H:true;");
    }


    [Fact]
    public void wifi_should_escape_input()
    {
        var ssid = "MyWiFiSSID";
        var password = "7heP4assw0rd";
        var authmode = PayloadGenerator.WiFi.Authentication.WPA;
        var hideSSID = true;

        var generator = new PayloadGenerator.WiFi(ssid, password, authmode, hideSSID);

        generator.ToString().ShouldBe($"WIFI:T:WPA;S:MyWiFiSSID;P:7heP4assw0rd;H:true;");
    }


    [Fact]
    public void wifi_should_escape_hex_style1()
    {
        var ssid = "A9B7F18CCE";
        var password = "00105F0E6";
        var authmode = PayloadGenerator.WiFi.Authentication.WPA;
        var hideSSID = true;

        var generator = new PayloadGenerator.WiFi(ssid, password, authmode, hideSSID);

        generator.ToString().ShouldBe($"WIFI:T:WPA;S:\"A9B7F18CCE\";P:\"00105F0E6\";H:true;");
    }


    [Fact]
    public void wifi_should_escape_hex_style2()
    {
        var ssid = "a9b7f18cce";
        var password = "00105f0Ee6";
        var authmode = PayloadGenerator.WiFi.Authentication.WPA;
        var hideSSID = true;

        var generator = new PayloadGenerator.WiFi(ssid, password, authmode, hideSSID);

        generator.ToString().ShouldBe($"WIFI:T:WPA;S:\"a9b7f18cce\";P:\"00105f0Ee6\";H:true;");
    }


    [Fact]
    public void wifi_should_escape_hex_style3()
    {
        var ssid = "0xA9B7F18CCE";
        var password = "0x00105F0E6";
        var authmode = PayloadGenerator.WiFi.Authentication.WPA;
        var hideSSID = true;

        var generator = new PayloadGenerator.WiFi(ssid, password, authmode, hideSSID);

        generator.ToString().ShouldBe($"WIFI:T:WPA;S:\"0xA9B7F18CCE\";P:\"0x00105F0E6\";H:true;");
    }


    [Fact]
    public void wifi_should_escape_hex_style4()
    {
        var ssid = "0XA9B7F18CCE";
        var password = "0X00105F0E6";
        var authmode = PayloadGenerator.WiFi.Authentication.WPA;
        var hideSSID = true;

        var generator = new PayloadGenerator.WiFi(ssid, password, authmode, hideSSID);

        generator.ToString().ShouldBe($"WIFI:T:WPA;S:\"0XA9B7F18CCE\";P:\"0X00105F0E6\";H:true;");
    }
}
