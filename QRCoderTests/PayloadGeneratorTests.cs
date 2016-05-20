using System;
using Xunit;
using QRCoder;
using Shouldly;
using System.Globalization;
using System.Threading;
using QRCoderTests.XUnitExtenstions;

namespace QRCoderTests
{
    public class PayloadGeneratorTests
    {
        
        [Fact]
        [Category("BitcoinAddress")]
        public void bitcoin_address_generator_can_generate_address()
        {
            var address = "175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W";
            var amount = .123;
            var label = "Some Label to Encode";
            var message = "Some Message to Encode";

            var generator = new PayloadGenerator.BitcoinAddress(address, amount, label, message);

            generator
                .ToString()
                .ShouldBe("bitcoin:175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W?label=Some%20Label%20to%20Encode&message=Some%20Message%20to%20Encode&amount=.123");
        }

        [Fact]
        [Category("BitcoinAddress")]
        public void bitcoin_address_generator_should_skip_missing_label()
        {
            var address = "175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W";
            var amount = .123;
            var message = "Some Message to Encode";


            var generator = new PayloadGenerator.BitcoinAddress(address, amount, null, message);

            generator
                .ToString()
                .ShouldNotContain("label");
        }

        [Fact]
        [Category("BitcoinAddress")]
        public void bitcoin_address_generator_should_skip_missing_message()
        {
            var address = "175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W";
            var amount = .123;


            var generator = new PayloadGenerator.BitcoinAddress(address, amount);

            generator
                .ToString()
                .ShouldNotContain("message");
        }

        [Fact]
        [Category("BitcoinAddress")]
        public void bitcoin_address_generator_should_round_to_satoshi()
        {
            var address = "175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W";
            var amount = .123456789;


            var generator = new PayloadGenerator.BitcoinAddress(address, amount);

            generator
                .ToString()
                .ShouldContain("amount=.12345679");
        }

        [Fact]
        [Category("BitcoinAddress")]
        public void bitcoin_address_generator_disregards_current_culture()
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

            var address = "175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W";
            var amount = .123;


            var generator = new PayloadGenerator.BitcoinAddress(address, amount);

            generator
                .ToString()
                .ShouldBe("bitcoin:175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W?amount=.123");
                        
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }


        [Fact]
        [Category("WiFi")]
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
        [Category("WiFi")]
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
        [Category("WiFi")]
        public void wifi_should_ignore_hiddenSSID_param()
        {
            var ssid = "MyWiFiSSID";
            var password = "7heP4assw0rd";
            var authmode = PayloadGenerator.WiFi.Authentication.WPA;

            var generator = new PayloadGenerator.WiFi(ssid, password, authmode);

            generator.ToString().ShouldBe($"WIFI:T:WPA;S:MyWiFiSSID;P:7heP4assw0rd;;");
        }


        [Fact]
        [Category("WiFi")]
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
        [Category("WiFi")]
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
        [Category("WiFi")]
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
        [Category("WiFi")]
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
        [Category("WiFi")]
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
        [Category("WiFi")]
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
}

