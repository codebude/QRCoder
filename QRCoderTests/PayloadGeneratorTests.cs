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
        [Category("PayloadGenerator/BitcoinAddress")]
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
        [Category("PayloadGenerator/BitcoinAddress")]
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
        [Category("PayloadGenerator/BitcoinAddress")]
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
        [Category("PayloadGenerator/BitcoinAddress")]
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
        [Category("PayloadGenerator/BitcoinAddress")]
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
        [Category("PayloadGenerator/WiFi")]
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
        [Category("PayloadGenerator/WiFi")]
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
        [Category("PayloadGenerator/WiFi")]
        public void wifi_should_ignore_hiddenSSID_param()
        {
            var ssid = "MyWiFiSSID";
            var password = "7heP4assw0rd";
            var authmode = PayloadGenerator.WiFi.Authentication.WPA;

            var generator = new PayloadGenerator.WiFi(ssid, password, authmode);

            generator.ToString().ShouldBe($"WIFI:T:WPA;S:MyWiFiSSID;P:7heP4assw0rd;;");
        }


        [Fact]
        [Category("PayloadGenerator/WiFi")]
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
        [Category("PayloadGenerator/WiFi")]
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
        [Category("PayloadGenerator/WiFi")]
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
        [Category("PayloadGenerator/WiFi")]
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
        [Category("PayloadGenerator/WiFi")]
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
        [Category("PayloadGenerator/WiFi")]
        public void wifi_should_escape_hex_style4()
        {
            var ssid = "0XA9B7F18CCE";
            var password = "0X00105F0E6";
            var authmode = PayloadGenerator.WiFi.Authentication.WPA;
            var hideSSID = true;

            var generator = new PayloadGenerator.WiFi(ssid, password, authmode, hideSSID);

            generator.ToString().ShouldBe($"WIFI:T:WPA;S:\"0XA9B7F18CCE\";P:\"0X00105F0E6\";H:true;");
        }

        
        [Fact]
        [Category("PayloadGenerator/Mail")]
        public void mail_should_build_type_mailto()
        {
            var receiver = "john@doe.com";
            var subject = "A test mail";
            var message = "Just see if it works!";
            var encoding = PayloadGenerator.Mail.MailEncoding.MAILTO;

            var generator = new PayloadGenerator.Mail(receiver, subject, message, encoding);

            generator.ToString().ShouldBe("mailto:john@doe.com?subject=A%20test%20mail&body=Just%20see%20if%20it%20works%21");
        }


        [Fact]
        [Category("PayloadGenerator/Mail")]
        public void mail_should_build_type_MATMSG()
        {
            var receiver = "john@doe.com";
            var subject = "A test mail";
            var message = "Just see if it works!";
            var encoding = PayloadGenerator.Mail.MailEncoding.MATMSG;

            var generator = new PayloadGenerator.Mail(receiver, subject, message, encoding);
            
            generator.ToString().ShouldBe("MATMSG:TO:john@doe.com;SUB:A test mail;BODY:Just see if it works!;;");
        }


        [Fact]
        [Category("PayloadGenerator/Mail")]
        public void mail_should_build_type_SMTP()
        {
            var receiver = "john@doe.com";
            var subject = "A test mail";
            var message = "Just see if it works!";
            var encoding = PayloadGenerator.Mail.MailEncoding.SMTP;

            var generator = new PayloadGenerator.Mail(receiver, subject, message, encoding);

            generator.ToString().ShouldBe("SMTP:john@doe.com:A test mail:Just see if it works!");
        }


        [Fact]
        [Category("PayloadGenerator/Mail")]
        public void mail_should_escape_input_MATMSG()
        {
            var receiver = "john@doe.com";
            var subject = "A test mail";
            var message = "Just see if \\:;, it works!";
            var encoding = PayloadGenerator.Mail.MailEncoding.MATMSG;

            var generator = new PayloadGenerator.Mail(receiver, subject, message, encoding);

            generator.ToString().ShouldBe("MATMSG:TO:john@doe.com;SUB:A test mail;BODY:Just see if \\\\\\:\\;\\, it works!;;");
        }


        [Fact]
        [Category("PayloadGenerator/Mail")]
        public void mail_should_escape_input_SMTP()
        {
            var receiver = "john@doe.com";
            var subject = "A test mail";
            var message = "Just see: if it works!";
            var encoding = PayloadGenerator.Mail.MailEncoding.SMTP;

            var generator = new PayloadGenerator.Mail(receiver, subject, message, encoding);

            generator.ToString().ShouldBe("SMTP:john@doe.com:A test mail:Just see\\: if it works!");
        }


        [Fact]
        [Category("PayloadGenerator/Mail")]
        public void mail_should_add_unused_params()
        {
            var receiver = "john@doe.com";

            var generator = new PayloadGenerator.Mail(receiver);

            generator.ToString().ShouldBe("mailto:john@doe.com?subject=&body=");
        }


        [Fact]
        [Category("PayloadGenerator/SMS")]
        public void sms_should_build_type_SMS()
        {
            var number = "01601234567";
            var message = "A small SMS";
            var encoding = PayloadGenerator.SMS.SMSEncoding.SMS;

            var generator = new PayloadGenerator.SMS(number, message, encoding);

            generator.ToString().ShouldBe("sms:01601234567?body=A%20small%20SMS");
        }


        [Fact]
        [Category("PayloadGenerator/SMS")]
        public void sms_should_build_type_SMS_iOS()
        {
            var number = "01601234567";
            var message = "A small SMS";
            var encoding = PayloadGenerator.SMS.SMSEncoding.SMS_iOS;

            var generator = new PayloadGenerator.SMS(number, message, encoding);

            generator.ToString().ShouldBe("sms:01601234567;body=A%20small%20SMS");
        }


        [Fact]
        [Category("PayloadGenerator/SMS")]
        public void sms_should_build_type_SMSTO()
        {
            var number = "01601234567";
            var message = "A small SMS";
            var encoding = PayloadGenerator.SMS.SMSEncoding.SMSTO;

            var generator = new PayloadGenerator.SMS(number, message, encoding);

            generator.ToString().ShouldBe("SMSTO:01601234567:A small SMS");
        }


        [Fact]
        [Category("PayloadGenerator/SMS")]
        public void sms_should_add_unused_params()
        {
            var number = "01601234567";

            var generator = new PayloadGenerator.SMS(number);

            generator.ToString().ShouldBe("sms:01601234567?body=");
        }


        [Fact]
        [Category("PayloadGenerator/MMS")]
        public void mms_should_build_type_MMS()
        {
            var number = "01601234567";
            var message = "A tiny MMS";
            var encoding = PayloadGenerator.MMS.MMSEncoding.MMS;

            var generator = new PayloadGenerator.MMS(number, message, encoding);

            generator.ToString().ShouldBe("mms:01601234567?body=A%20tiny%20MMS");
        }


        [Fact]
        [Category("PayloadGenerator/MMS")]
        public void mms_should_build_type_MMSTO()
        {
            var number = "01601234567";
            var message = "A tiny SMS";
            var encoding = PayloadGenerator.MMS.MMSEncoding.MMSTO;

            var generator = new PayloadGenerator.MMS(number, message, encoding);

            generator.ToString().ShouldBe("mmsto:01601234567?subject=A%20tiny%20SMS");
        }

                
        [Fact]
        [Category("PayloadGenerator/MMS")]
        public void mms_should_add_unused_params()
        {
            var number = "01601234567";

            var generator = new PayloadGenerator.MMS(number);

            generator.ToString().ShouldBe("mms:01601234567?body=");
        }


        [Fact]
        [Category("PayloadGenerator/Geolocation")]
        public void geolocation_should_build_type_GEO()
        {
            var latitude = "51.227741";
            var longitude = "6.773456";
            var encoding = PayloadGenerator.Geolocation.GeolocationEncoding.GEO;

            var generator = new PayloadGenerator.Geolocation(latitude, longitude, encoding);

            generator.ToString().ShouldBe("geo:51.227741,6.773456");
        }


        [Fact]
        [Category("PayloadGenerator/Geolocation")]
        public void geolocation_should_build_type_GoogleMaps()
        {
            var latitude = "51.227741";
            var longitude = "6.773456";
            var encoding = PayloadGenerator.Geolocation.GeolocationEncoding.GoogleMaps;

            var generator = new PayloadGenerator.Geolocation(latitude, longitude, encoding);

            generator.ToString().ShouldBe("http://maps.google.com/maps?q=51.227741,6.773456");
        }


        [Fact]
        [Category("PayloadGenerator/Geolocation")]
        public void geolocation_should_escape_input()
        {
            var latitude = "51,227741"; 
            var longitude = "6,773456";
            var encoding = PayloadGenerator.Geolocation.GeolocationEncoding.GEO;

            var generator = new PayloadGenerator.Geolocation(latitude, longitude, encoding);

            generator.ToString().ShouldBe("geo:51.227741,6.773456");
        }


        [Fact]
        [Category("PayloadGenerator/Geolocation")]
        public void geolocation_should_add_unused_params()
        {
            var latitude = "51.227741";
            var longitude = "6.773456";
         
            var generator = new PayloadGenerator.Geolocation(latitude, longitude);

            generator.ToString().ShouldBe("geo:51.227741,6.773456");
        }


        [Fact]
        [Category("PayloadGenerator/PhoneNumber")]
        public void phonenumber_should_build()
        {
            var number = "+495321123456";

            var generator = new PayloadGenerator.PhoneNumber(number);

            generator.ToString().ShouldBe("tel:+495321123456");
        }


        [Fact]
        [Category("PayloadGenerator/Skype")]
        public void skype_should_build()
        {
            var username = "johndoe123";

            var generator = new PayloadGenerator.SkypeCall(username);

            generator.ToString().ShouldBe("skype:johndoe123?call");
        }


        [Fact]
        [Category("PayloadGenerator/Url")]
        public void url_should_build_http()
        {
            var url = "http://code-bude.net";

            var generator = new PayloadGenerator.Url(url);

            generator.ToString().ShouldBe("http://code-bude.net");
        }


        [Fact]
        [Category("PayloadGenerator/Url")]
        public void url_should_build_https()
        {
            var url = "https://code-bude.net";

            var generator = new PayloadGenerator.Url(url);

            generator.ToString().ShouldBe("https://code-bude.net");
        }


        [Fact]
        [Category("PayloadGenerator/Url")]
        public void url_should_add_http()
        {
            var url = "code-bude.net";

            var generator = new PayloadGenerator.Url(url);

            generator.ToString().ShouldBe("http://code-bude.net");
        }


        [Fact]
        [Category("PayloadGenerator/Bookmark")]
        public void bookmark_should_build()
        {
            var url = "http://code-bude.net";
            var title = "A nerd's blog";

            var generator = new PayloadGenerator.Bookmark(url, title);

            generator.ToString().ShouldBe("MEBKM:TITLE:A nerd's blog;URL:http\\://code-bude.net;;");
        }


        [Fact]
        [Category("PayloadGenerator/Bookmark")]
        public void bookmark_should_escape_input()
        {
            var url = "http://code-bude.net/fake,url.html";
            var title = "A nerd's blog: \\All;the;things\\";

            var generator = new PayloadGenerator.Bookmark(url, title);

            generator.ToString().ShouldBe("MEBKM:TITLE:A nerd's blog\\: \\\\All\\;the\\;things\\\\;URL:http\\://code-bude.net/fake\\,url.html;;");
        }


        [Fact]
        [Category("PayloadGenerator/CalendarEvent")]
        public void calendarevent_should_build_universal()
        {
            var subject = "Release party";
            var description = "A small party for the new QRCoder. Bring some beer!";
            var location = "Programmer's paradise, Beachtown, Paradise";
            var alldayEvent = false;
            var begin = new DateTime(2016,01,03,12,00,00);
            var end = new DateTime(2016,01,03,14,30,0);
            var encoding = PayloadGenerator.CalendarEvent.EventEncoding.Universal;

            var generator = new PayloadGenerator.CalendarEvent(subject, description, location, begin, end, alldayEvent, encoding);

            generator.ToString().ShouldBe($"BEGIN:VEVENT{Environment.NewLine}SUMMARY:Release party{Environment.NewLine}DESCRIPTION:A small party for the new QRCoder. Bring some beer!{Environment.NewLine}LOCATION:Programmer's paradise, Beachtown, Paradise{Environment.NewLine}DTSTART:20160103T120000{Environment.NewLine}DTEND:20160103T143000{Environment.NewLine}END:VEVENT");
        }


        [Fact]
        [Category("PayloadGenerator/CalendarEvent")]
        public void calendarevent_should_build_ical()
        {
            var subject = "Release party";
            var description = "A small party for the new QRCoder. Bring some beer!";
            var location = "Programmer's paradise, Beachtown, Paradise";
            var alldayEvent = false;
            var begin = new DateTime(2016, 01, 03, 12, 00, 00);
            var end = new DateTime(2016, 01, 03, 14, 30, 0);
            var encoding = PayloadGenerator.CalendarEvent.EventEncoding.iCalComplete;

            var generator = new PayloadGenerator.CalendarEvent(subject, description, location, begin, end, alldayEvent, encoding);

            generator.ToString().ShouldBe($"BEGIN:VCALENDAR{Environment.NewLine}VERSION:2.0{Environment.NewLine}BEGIN:VEVENT{Environment.NewLine}SUMMARY:Release party{Environment.NewLine}DESCRIPTION:A small party for the new QRCoder. Bring some beer!{Environment.NewLine}LOCATION:Programmer's paradise, Beachtown, Paradise{Environment.NewLine}DTSTART:20160103T120000{Environment.NewLine}DTEND:20160103T143000{Environment.NewLine}END:VEVENT{Environment.NewLine}END:VCALENDAR");
        }


        [Fact]
        [Category("PayloadGenerator/CalendarEvent")]
        public void calendarevent_should_build_allday()
        {
            var subject = "Release party";
            var description = "A small party for the new QRCoder. Bring some beer!";
            var location = "Programmer's paradise, Beachtown, Paradise";
            var alldayEvent = true;
            var begin = new DateTime(2016, 01, 03);
            var end = new DateTime(2016, 01, 03);
            var encoding = PayloadGenerator.CalendarEvent.EventEncoding.Universal;

            var generator = new PayloadGenerator.CalendarEvent(subject, description, location, begin, end, alldayEvent, encoding);

            generator.ToString().ShouldBe($"BEGIN:VEVENT{Environment.NewLine}SUMMARY:Release party{Environment.NewLine}DESCRIPTION:A small party for the new QRCoder. Bring some beer!{Environment.NewLine}LOCATION:Programmer's paradise, Beachtown, Paradise{Environment.NewLine}DTSTART:20160103{Environment.NewLine}DTEND:20160103{Environment.NewLine}END:VEVENT");
        }


        [Fact]
        [Category("PayloadGenerator/CalendarEvent")]
        public void calendarevent_should_care_empty_fields()
        {
            var subject = "Release party";
            var description = "";
            var location = string.Empty;
            var alldayEvent = false;
            var begin = new DateTime(2016, 01, 03, 12, 00, 00);
            var end = new DateTime(2016, 01, 03, 14, 30, 0);
            var encoding = PayloadGenerator.CalendarEvent.EventEncoding.Universal;

            var generator = new PayloadGenerator.CalendarEvent(subject, description, location, begin, end, alldayEvent, encoding);

            generator.ToString().ShouldBe($"BEGIN:VEVENT{Environment.NewLine}SUMMARY:Release party{Environment.NewLine}DTSTART:20160103T120000{Environment.NewLine}DTEND:20160103T143000{Environment.NewLine}END:VEVENT");
        }


        [Fact]
        [Category("PayloadGenerator/CalendarEvent")]
        public void calendarevent_should_add_unused_params()
        {
            var subject = "Release party";
            var description = "A small party for the new QRCoder. Bring some beer!";
            var location = "Programmer's paradise, Beachtown, Paradise";
            var alldayEvent = false;
            var begin = new DateTime(2016, 01, 03, 12, 00, 00);
            var end = new DateTime(2016, 01, 03, 14, 30, 0);

            var generator = new PayloadGenerator.CalendarEvent(subject, description, location, begin, end, alldayEvent);

            generator.ToString().ShouldBe($"BEGIN:VEVENT{Environment.NewLine}SUMMARY:Release party{Environment.NewLine}DESCRIPTION:A small party for the new QRCoder. Bring some beer!{Environment.NewLine}LOCATION:Programmer's paradise, Beachtown, Paradise{Environment.NewLine}DTSTART:20160103T120000{Environment.NewLine}DTEND:20160103T143000{Environment.NewLine}END:VEVENT");
        }
    }
}


