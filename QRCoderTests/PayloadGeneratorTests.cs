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

        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_can_generate_payload_minimal()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;

            var generator = new PayloadGenerator.Girocode(iban, bic, name, amount);

            generator
                .ToString()
                .ShouldBe("BCD\n001\n2\nSCT\nBFSWDE33BER\nWikimedia Fördergesellschaft\nDE33100205000001194700\nEUR10.00\n\n\n\n");
        }

        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_can_generate_payload_full()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;
            var remittanceInformation = "Donation to Wikipedia.";
            var purposeOfCreditTransfer = "1234";
            var messageToGirocodeUser = "Thanks for using Girocode";

            var generator = new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, purposeOfCreditTransfer, messageToGirocodeUser,
                PayloadGenerator.Girocode.GirocodeVersion.Version1,
                PayloadGenerator.Girocode.GirocodeEncoding.ISO_8859_1);

            generator
                .ToString()
                .ShouldBe("BCD\n001\n2\nSCT\nBFSWDE33BER\nWikimedia Fördergesellschaft\nDE33100205000001194700\nEUR10.00\n1234\n\nDonation to Wikipedia.\nThanks for using Girocode");
        }

        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_should_handle_version()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;
            var remittanceInformation = "Donation to Wikipedia.";
            var purposeOfCreditTransfer = "1234";
            var messageToGirocodeUser = "Thanks for using Girocode";

            var generator = new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, purposeOfCreditTransfer, messageToGirocodeUser,
                PayloadGenerator.Girocode.GirocodeVersion.Version2,
                PayloadGenerator.Girocode.GirocodeEncoding.ISO_8859_1);

            generator
                .ToString()
                .ShouldBe("BCD\n002\n2\nSCT\nBFSWDE33BER\nWikimedia Fördergesellschaft\nDE33100205000001194700\nEUR10.00\n1234\n\nDonation to Wikipedia.\nThanks for using Girocode");
        }


        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_should_handle_iban_whitespaces()
        {
            var iban = "DE33 1002 0500 0001 1947 00";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;
            var remittanceInformation = "Donation to Wikipedia.";
            var purposeOfCreditTransfer = "1234";
            var messageToGirocodeUser = "Thanks for using Girocode";

            var generator = new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, purposeOfCreditTransfer, messageToGirocodeUser);

            generator
                .ToString()
                .ShouldBe("BCD\n001\n2\nSCT\nBFSWDE33BER\nWikimedia Fördergesellschaft\nDE33100205000001194700\nEUR10.00\n1234\n\nDonation to Wikipedia.\nThanks for using Girocode");
        }

        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_should_handle_bic_whitespaces()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSW DE 33 BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;
            var remittanceInformation = "Donation to Wikipedia.";
            var purposeOfCreditTransfer = "1234";
            var messageToGirocodeUser = "Thanks for using Girocode";

            var generator = new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, purposeOfCreditTransfer, messageToGirocodeUser);

            generator
                .ToString()
                .ShouldBe("BCD\n001\n2\nSCT\nBFSWDE33BER\nWikimedia Fördergesellschaft\nDE33100205000001194700\nEUR10.00\n1234\n\nDonation to Wikipedia.\nThanks for using Girocode");
        }


        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_should_fill_amount_decimals()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSW DE 33 BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 12m;
            var remittanceInformation = "Donation to Wikipedia.";
            var purposeOfCreditTransfer = "1234";
            var messageToGirocodeUser = "Thanks for using Girocode";

            var generator = new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, purposeOfCreditTransfer, messageToGirocodeUser);

            generator
                .ToString()
                .ShouldBe("BCD\n001\n2\nSCT\nBFSWDE33BER\nWikimedia Fördergesellschaft\nDE33100205000001194700\nEUR12.00\n1234\n\nDonation to Wikipedia.\nThanks for using Girocode");
        }


        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_should_throw_iban_exception()
        {
            var iban = "33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;
            var remittanceInformation = "Donation to Wikipedia.";
            var purposeOfCreditTransfer = "1234";
            var messageToGirocodeUser = "Thanks for using Girocode";

          
            var exception = Record.Exception(() => new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, purposeOfCreditTransfer, messageToGirocodeUser));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.Girocode.GirocodeException>(exception);
            exception.Message.ShouldBe("The IBAN entered isn't valid.");
        }


        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_should_throw_bic_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "DWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;
            var remittanceInformation = "Donation to Wikipedia.";
            var purposeOfCreditTransfer = "1234";
            var messageToGirocodeUser = "Thanks for using Girocode";


            var exception = Record.Exception(() => new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, purposeOfCreditTransfer, messageToGirocodeUser));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.Girocode.GirocodeException>(exception);
            exception.Message.ShouldBe("The BIC entered isn't valid.");
        }


        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_should_throw_name_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "A company with a name which is exactly 71 chars - and for that to long.";
            var amount = 10.00m;
            var remittanceInformation = "Donation to Wikipedia.";
            var purposeOfCreditTransfer = "1234";
            var messageToGirocodeUser = "Thanks for using Girocode";


            var exception = Record.Exception(() => new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, purposeOfCreditTransfer, messageToGirocodeUser));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.Girocode.GirocodeException>(exception);
            exception.Message.ShouldBe("(Payee-)Name must be shorter than 71 chars.");
        }


        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_should_throw_amount_decimals_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.521m;
            var remittanceInformation = "Donation to Wikipedia.";
            var purposeOfCreditTransfer = "1234";
            var messageToGirocodeUser = "Thanks for using Girocode";


            var exception = Record.Exception(() => new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, purposeOfCreditTransfer, messageToGirocodeUser));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.Girocode.GirocodeException>(exception);
            exception.Message.ShouldBe("Amount must have less than 3 digits after decimal point.");
        }

        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_should_throw_amount_min_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 0.00m;
            var remittanceInformation = "Donation to Wikipedia.";
            var purposeOfCreditTransfer = "1234";
            var messageToGirocodeUser = "Thanks for using Girocode";


            var exception = Record.Exception(() => new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, purposeOfCreditTransfer, messageToGirocodeUser));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.Girocode.GirocodeException>(exception);
            exception.Message.ShouldBe("Amount has to at least 0.01 and must be smaller or equal to 999999999.99.");
        }



        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_should_throw_amount_max_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 1999999999.99m;
            var remittanceInformation = "Donation to Wikipedia.";
            var purposeOfCreditTransfer = "1234";
            var messageToGirocodeUser = "Thanks for using Girocode";


            var exception = Record.Exception(() => new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, purposeOfCreditTransfer, messageToGirocodeUser));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.Girocode.GirocodeException>(exception);
            exception.Message.ShouldBe("Amount has to at least 0.01 and must be smaller or equal to 999999999.99.");
        }


        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_should_throw_purpose_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;
            var remittanceInformation = "Donation to Wikipedia.";
            var purposeOfCreditTransfer = "12345";
            var messageToGirocodeUser = "Thanks for using Girocode";


            var exception = Record.Exception(() => new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, purposeOfCreditTransfer, messageToGirocodeUser));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.Girocode.GirocodeException>(exception);
            exception.Message.ShouldBe("Purpose of credit transfer can only have 4 chars at maximum.");
        }

        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_should_throw_remittance_unstructured_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;
            var remittanceInformation = "An unstructured remittance information which is longer than a tweet. This means that this unstructures remittance info has more than 140 chars.";
            var purposeOfCreditTransfer = "1234";
            var messageToGirocodeUser = "Thanks for using Girocode";


            var exception = Record.Exception(() => new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, purposeOfCreditTransfer, messageToGirocodeUser));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.Girocode.GirocodeException>(exception);
            exception.Message.ShouldBe("Unstructured reference texts have to shorter than 141 chars.");
        }

        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_should_throw_remittance_structured_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;
            var remittanceInformation = "Structured remittance infos have to be shorter than 36 chars.";
            var purposeOfCreditTransfer = "1234";
            var messageToGirocodeUser = "Thanks for using Girocode";


            var exception = Record.Exception(() => new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Structured, purposeOfCreditTransfer, messageToGirocodeUser));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.Girocode.GirocodeException>(exception);
            exception.Message.ShouldBe("Structured reference texts have to shorter than 36 chars.");
        }

        [Fact]
        [Category("PayloadGenerator/Girocode")]
        public void girocode_generator_should_throw_usermessage_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;
            var remittanceInformation = "Donation to Wikipedia.";
            var purposeOfCreditTransfer = "1234";
            var messageToGirocodeUser = "The usermessage is shown to the user which scans the Girocode. It has to be shorter than 71 chars.";


            var exception = Record.Exception(() => new PayloadGenerator.Girocode(iban, bic, name, amount, remittanceInformation,
                PayloadGenerator.Girocode.TypeOfRemittance.Unstructured, purposeOfCreditTransfer, messageToGirocodeUser));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.Girocode.GirocodeException>(exception);
            exception.Message.ShouldBe("Message to the Girocode-User reader texts have to shorter than 71 chars.");
        }

        [Fact]
        [Category("PayloadGenerator/OneTimePassword")]
        public void one_time_password_generator_time_based_generates_with_standard_options()
        {
            var pg = new PayloadGenerator.OneTimePassword
            {
                Secret = "pwq6 5q55",
                Issuer = "Google",
                Label = "test@google.com",
            };

            pg.ToString().ShouldBe("otpauth://totp/Google:test@google.com?secret=pwq65q55&issuer=Google");
        }

        [Fact]
        [Category("PayloadGenerator/OneTimePassword")]
        public void one_time_password_generator_hmac_based_generates_with_standard_options()
        {
            var pg = new PayloadGenerator.OneTimePassword
            {
                Secret = "pwq6 5q55",
                Issuer = "Google",
                Label = "test@google.com",
                Type = PayloadGenerator.OneTimePassword.OneTimePasswordAuthType.HOTP,
                Counter = 500,
            };

            pg.ToString().ShouldBe("otpauth://hotp/Google:test@google.com?secret=pwq65q55&issuer=Google&counter=500");
        }
        //TODO: Include more tests for the one time password payload generator
        

        [Fact]
        [Category("PayloadGenerator/ShadowSocksConfig")]
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
        [Category("PayloadGenerator/ShadowSocksConfig")]
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
        [Category("PayloadGenerator/ShadowSocksConfig")]
        public void shadowsocks_generator_should_throw_portrange_low_exception()
        {
            var host = "192.168.2.5";
            var port = 0;
            var password = "s3cr3t";
            var method = PayloadGenerator.ShadowSocksConfig.Method.Rc4Md5;
            
            var exception = Record.Exception(() => new PayloadGenerator.ShadowSocksConfig(host, port, password, method));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.ShadowSocksConfig.ShadowSocksConfigException>(exception);
            exception.Message.ShouldBe("Value of 'port' must be within 0 and 65535.");
        }


        [Fact]
        [Category("PayloadGenerator/ShadowSocksConfig")]
        public void shadowsocks_generator_should_throw_portrange_high_exception()
        {
            var host = "192.168.2.5";
            var port = 65536;
            var password = "s3cr3t";
            var method = PayloadGenerator.ShadowSocksConfig.Method.Rc4Md5;

            var exception = Record.Exception(() => new PayloadGenerator.ShadowSocksConfig(host, port, password, method));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.ShadowSocksConfig.ShadowSocksConfigException>(exception);
            exception.Message.ShouldBe("Value of 'port' must be within 0 and 65535.");
        }


        [Fact]
        [Category("PayloadGenerator/Monero")]
        public void monero_generator_can_generate_payload_simple()
        {
            var address = "46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em";
            var generator = new PayloadGenerator.MoneroTransaction(address);

            generator
                .ToString()
                .ShouldBe("monero://46BeWrHpwXmHDpDEUmZBWZfoQpdc6HaERCNmx1pEYL2rAcuwufPN9rXHHtyUA4QVy66qeFQkn6sfK8aHYjA3jk3o1Bv16em");
        }


        [Fact]
        [Category("PayloadGenerator/Monero")]
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
        [Category("PayloadGenerator/Monero")]
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
        [Category("PayloadGenerator/Monero")]
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
        [Category("PayloadGenerator/Monero")]
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
        [Category("PayloadGenerator/Monero")]
        public void monero_generator_should_throw_no_address_exception()
        {
            var address = "";

            var exception = Record.Exception(() => new PayloadGenerator.MoneroTransaction(address));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.MoneroTransaction.MoneroTransactionException>(exception);
            exception.Message.ShouldBe("The address is mandatory and has to be set.");
        }
    }
}



