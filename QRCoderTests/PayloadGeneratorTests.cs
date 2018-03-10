using System;
using Xunit;
using QRCoder;
using Shouldly;
using System.Globalization;
using System.Threading;
using QRCoderTests.XUnitExtenstions;
using static QRCoder.PayloadGenerator.BezahlCode;
using static QRCoder.PayloadGenerator.SwissQrCode.Reference;


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
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_can_generate_payload_singlepayment_minimal()
        {
            var account = "001194700";
            var bnc = "100205000";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account: account, bnc: bnc, amount: amount);

            generator
                .ToString()
                .ShouldBe("bank://singlepayment?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000&amount=10,00&currency=EUR&executiondate="+DateTime.Now.ToString("ddMMyyyy")+"");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_can_generate_payload_singlepayment_full()
        {
            var account = "001194700";
            var bnc = "100205000";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "Thanks for all your efforts";
            var amount = 10.00m;
            var postingKey = 69;
            Currency currency = Currency.USD;

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account, bnc, amount, "", 0, null, null, reason, postingKey, currency, DateTime.Now);

            generator
                .ToString()
                .ShouldBe("bank://singlepayment?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000&postingkey=69&amount=10,00&reason=Thanks%20for%20all%20your%20efforts&currency=USD&executiondate="+DateTime.Now.ToString("ddMMyyyy")+"");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_can_generate_payload_singledirectdebit()
        {
            var account = "001194700";
            var bnc = "100205000";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "Thanks for all your efforts";
            var amount = 10.00m;
            var postingKey = 69;
            Currency currency = Currency.USD;

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.singledirectdebit, name, account, bnc, amount, "", 0, null, null, reason, postingKey, currency, DateTime.Now);

            generator
                .ToString()
                .ShouldBe("bank://singledirectdebit?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000&postingkey=69&amount=10,00&reason=Thanks%20for%20all%20your%20efforts&currency=USD&executiondate="+DateTime.Now.ToString("ddMMyyyy")+"");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_can_generate_payload_periodicsinglepayment()
        {
            var account = "001194700";
            var bnc = "100205000";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "Thanks for all your efforts";
            var amount = 10.00m;
            var postingKey = 69;
            var periodicTimeunit = "W";
            var periodicTimeunitRotation = 2;
            var periodicFirstExecutionDate = DateTime.Now;
            var periodicLastExecutionDate = DateTime.Now.AddMonths(3);
            Currency currency = Currency.USD;

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.periodicsinglepayment, name, account, bnc, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, reason, postingKey, currency, DateTime.Now);

            generator
                .ToString()
                .ShouldBe("bank://periodicsinglepayment?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000&postingkey=69&amount=10,00&reason=Thanks%20for%20all%20your%20efforts&currency=USD&executiondate=" + DateTime.Now.ToString("ddMMyyyy") + "&periodictimeunit=W&periodictimeunitrotation=2&periodicfirstexecutiondate=" + periodicFirstExecutionDate.ToString("ddMMyyyy") + "&periodiclastexecutiondate=" + periodicLastExecutionDate.ToString("ddMMyyyy"));
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_can_generate_payload_singlepaymentsepa_minimal()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount);

            generator
                .ToString()
                .ShouldBe("bank://singlepaymentsepa?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER&amount=10,00&currency=EUR&executiondate="+DateTime.Now.ToString("ddMMyyyy")+"");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_can_generate_payload_singlepaymentsepa_full()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "Thanks for all your efforts";
            var sepaReference = "Fake SEPA reference";
            var amount = 10.00m;
            Currency currency = Currency.USD;

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban, bic, amount, "", 0, null, null, "", "", new DateTime(2017,03,01), reason, sepaReference, currency, DateTime.Now);

            generator
                .ToString()
                .ShouldBe("bank://singlepaymentsepa?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER&separeference=Fake%20SEPA%20reference&amount=10,00&reason=Thanks%20for%20all%20your%20efforts&currency=USD&executiondate="+DateTime.Now.ToString("ddMMyyyy")+"");
        }

        
        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_can_generate_payload_singledirectdebitsepa()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "Thanks for all your efforts";
            var creditorId = "DE 02 TSV 01234567890";
            var mandateId = "987543CB2";
            var sepaReference = "Fake SEPA reference";
            var amount = 10.00m;
            Currency currency = Currency.USD;

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.singledirectdebitsepa, name, iban, bic, amount, "", 0, null, null, creditorId, mandateId, new DateTime(2017,03,01), reason, sepaReference, currency, DateTime.Now);

            generator
                .ToString()
                .ShouldBe("bank://singledirectdebitsepa?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER&separeference=Fake%20SEPA%20reference&creditorid=DE%2002%20TSV%2001234567890&mandateid=987543CB2&dateofsignature=01032017&amount=10,00&reason=Thanks%20for%20all%20your%20efforts&currency=USD&executiondate="+DateTime.Now.ToString("ddMMyyyy")+"");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_can_generate_payload_periodicsinglepaymentsepa()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "Thanks for all your efforts";
            var sepaReference = "Fake SEPA reference";
            var amount = 10.00m;
            var periodicTimeunit = "M";
            var periodicTimeunitRotation = 1;
            var periodicFirstExecutionDate = DateTime.Now;
            var periodicLastExecutionDate = DateTime.Now.AddMonths(3);
            Currency currency = Currency.USD;

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.periodicsinglepaymentsepa, name, iban, bic, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, "", "", new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now);

            generator
                .ToString()
                .ShouldBe("bank://periodicsinglepaymentsepa?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER&separeference=Fake%20SEPA%20reference&amount=10,00&reason=Thanks%20for%20all%20your%20efforts&currency=USD&executiondate="+DateTime.Now.ToString("ddMMyyyy")+"&periodictimeunit=M&periodictimeunitrotation=1&periodicfirstexecutiondate=" + periodicFirstExecutionDate.ToString("ddMMyyyy") + "&periodiclastexecutiondate=" + periodicLastExecutionDate.ToString("ddMMyyyy"));
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_can_generate_payload_contact()
        {
            var account = "001194700";
            var bnc = "100205000";
            var name = "Wikimedia Fördergesellschaft";

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.contact, name, account: account, bnc: bnc);

            generator
                .ToString()
                .ShouldBe("bank://contact?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_can_generate_payload_contact_full()
        {
            var account = "001194700";
            var bnc = "100205000";
            var name = "Wikimedia Fördergesellschaft";

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.contact, name, account, bnc, "", "", "New business contact.");

            generator
                .ToString()
                .ShouldBe("bank://contact?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000&reason=New%20business%20contact.");
        }



        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_can_generate_payload_contactv2_classic()
        {
            var account = "001194700";
            var bnc = "100205000";
            var name = "Wikimedia Fördergesellschaft";

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.contact_v2, name, account: account, bnc: bnc);

            generator
                .ToString()
                .ShouldBe("bank://contact_v2?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_can_generate_payload_contactv2_sepa()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.contact_v2, name, iban: iban, bic: bic);

            generator
                .ToString()
                .ShouldBe("bank://contact_v2?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER");
        }



        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_can_generate_payload_contactv2_sepa_full()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.contact_v2, name, "", "", iban, bic, "A new v2 contact.");

            generator
                .ToString()
                .ShouldBe("bank://contact_v2?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER&reason=A%20new%20v2%20contact.");
        }



        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_handle_account_whitespaces()
        {
            var account = "01 194700";
            var bnc = "100205000";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account: account, bnc: bnc, amount: amount);

            generator
                .ToString()
                .ShouldBe("bank://singlepayment?name=Wikimedia%20F%C3%B6rdergesellschaft&account=01194700&bnc=100205000&amount=10,00&currency=EUR&executiondate="+DateTime.Now.ToString("ddMMyyyy")+"");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_handle_bnc_whitespaces()
        {
            var account = "001194700";
            var bnc = "10020 5000";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account: account, bnc: bnc, amount: amount);

            generator
                .ToString()
                .ShouldBe("bank://singlepayment?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000&amount=10,00&currency=EUR&executiondate="+DateTime.Now.ToString("ddMMyyyy")+"");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_handle_iban_whitespaces()
        {
            var iban = "DE33 100205000 0011947 00";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount);

            generator
                .ToString()
                .ShouldBe("bank://singlepaymentsepa?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER&amount=10,00&currency=EUR&executiondate="+DateTime.Now.ToString("ddMMyyyy")+"");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_handle_bic_whitespaces()
        {
            var iban = "DE33100205000001194700";
            var bic = "BF SWDE3 3BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount);

            generator
                .ToString()
                .ShouldBe("bank://singlepaymentsepa?name=Wikimedia%20F%C3%B6rdergesellschaft&iban=DE33100205000001194700&bic=BFSWDE33BER&amount=10,00&currency=EUR&executiondate="+DateTime.Now.ToString("ddMMyyyy")+"");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_add_decimals()
        {
            var account = "001194700";
            var bnc = "10020 5000";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10;

            var generator = new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account: account, bnc: bnc, amount: amount);

            generator
                .ToString()
                .ShouldBe("bank://singlepayment?name=Wikimedia%20F%C3%B6rdergesellschaft&account=001194700&bnc=100205000&amount=10,00&currency=EUR&executiondate="+DateTime.Now.ToString("ddMMyyyy")+"");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_wrong_contact_constructor_exception()
        {
            var account = "0001194700";
            var bnc = "10020 5000";
            var name = "Wikimedia Fördergesellschaft";

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account, bnc, "", "", "New business contact."));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("The constructor without an amount may only ne used with authority types 'contact' and 'contact_v2'.");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_wrong_contact_v2_constructor_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
                  
            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.periodicsinglepaymentsepa, name, iban: iban, bic: bic));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("The constructor without an amount may only ne used with authority types 'contact' and 'contact_v2'.");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_wrong_nonsepa_constructor_exception()
        {
            var account = "0001194700";
            var bnc = "100205000";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10;
            
            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, account: account, bnc: bnc, amount: amount));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("The constructor with 'account' and 'bnc' may only be used with 'non SEPA' authority types. Either choose another authority type or switch constructor.");
        }

        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_wrong_nonsepa_constructor_periodic_exception()
        {
            var account = "0001194700";
            var bnc = "100205000";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "Thanks for all your efforts";
            var amount = 10.00m;
            var postingKey = 69;
            var periodicTimeunit = "";
            var periodicTimeunitRotation = 2;
            var periodicFirstExecutionDate = DateTime.Now;
            var periodicLastExecutionDate = DateTime.Now.AddMonths(3);
            Currency currency = Currency.USD;

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.periodicsinglepayment, name, account, bnc, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, reason, postingKey, currency, DateTime.Now));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("When using 'periodicsinglepayment' as authority type, the parameters 'periodicTimeunit' and 'periodicTimeunitRotation' must be set.");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_wrong_sepa_constructor_exception()
        {
            var iban = "DE33 100205000 0011947 00";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;
                  
            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, iban: iban, bic: bic, amount: amount));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("The constructor with 'iban' and 'bic' may only be used with 'SEPA' authority types. Either choose another authority type or switch constructor.");

        }

        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_wrong_sepa_constructor_periodic_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "Thanks for all your efforts";
            var sepaReference = "Fake SEPA reference";
            var amount = 10.00m;
            var periodicTimeunit = "M";
            var periodicTimeunitRotation = 0;
            var periodicFirstExecutionDate = DateTime.Now;
            var periodicLastExecutionDate = DateTime.Now.AddMonths(3);
            Currency currency = Currency.USD;
            
            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.periodicsinglepaymentsepa, name, iban, bic, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, "", "", new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("When using 'periodicsinglepaymentsepa' as authority type, the parameters 'periodicTimeunit' and 'periodicTimeunitRotation' must be set.");

        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_name_too_long_exception()
        {
            var iban = "DE33 100205000 0011947 00";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft has really really really long name, over 71 chars";
            var amount = 10.00m;

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("(Payee-)Name must be shorter than 71 chars.");

        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_reason_too_long_exception()
        {
            var iban = "DE33 100205000 0011947 00";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "A long long long reason text which may resolve in an exception";
            var amount = 10.00m;

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount, reason: reason));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("Reasons texts have to be shorter than 28 chars.");

        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_invalid_account_exception()
        {
            var account = "1194700AD";
            var bnc = "100205000";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account: account, bnc: bnc, amount: amount));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("The account entered isn't valid.");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_invalid_bnc_exception()
        {
            var account = "001194700";
            var bnc = "10020500023545626226262";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account: account, bnc: bnc, amount: amount));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("The bnc entered isn't valid.");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_invalid_postingkey_exception()
        {
            var account = "001194700";
            var bnc = "100205000";
            var name = "Wikimedia Fördergesellschaft";
            var postingKey = 101;
            var amount = 10.00m;

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account: account, bnc: bnc, amount: amount, postingKey: postingKey));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("PostingKey must be within 0 and 99.");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_invalid_iban_exception()
        {
            var iban = "DE33100205AZB000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("The IBAN entered isn't valid.");

        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_invalid_bic_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "B2FSWDE33BER99871ABC99998";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.00m;

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("The BIC entered isn't valid.");

        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_separeference_too_long_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "Thanks for all your efforts";
            var creditorId = "DE 02 TSV 01234567890";
            var mandateId = "987543CB2";
            var sepaReference = "Fake SEPA reference which is also much to long for the reference field.";
            var amount = 10.00m;
            Currency currency = Currency.USD;
            
            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singledirectdebitsepa, name, iban, bic, amount, "", 0, null, null, creditorId, mandateId, new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("SEPA reference texts have to be shorter than 36 chars.");

        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_invalid_creditorid_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "Thanks for all your efforts";
            var creditorId = "12DE 02 TSV 01234567890";
            var mandateId = "987543CB2";
            var sepaReference = "Fake SEPA reference.";
            var amount = 10.00m;
            Currency currency = Currency.USD;

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singledirectdebitsepa, name, iban, bic, amount, "", 0, null, null, creditorId, mandateId, new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("The creditorId entered isn't valid.");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_invalid_mandateid_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "Thanks for all your efforts";
            var creditorId = "DE 02 TSV 01234567890";
            var mandateId = "ÄÖ987543CB2 1990 2017";
            var sepaReference = "Fake SEPA reference.";
            var amount = 10.00m;
            Currency currency = Currency.USD;

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singledirectdebitsepa, name, iban, bic, amount, "", 0, null, null, creditorId, mandateId, new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("The mandateId entered isn't valid.");
        }

        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_amount_too_much_digits_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 10.001m;

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("Amount must have less than 3 digits after decimal point.");

        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_amount_too_big_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var amount = 1000000000m;

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepaymentsepa, name, iban: iban, bic: bic, amount: amount));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("Amount has to at least 0.01 and must be smaller or equal to 999999999.99.");

        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_invalid_executiondate_exception()
        {
            var account = "001194700";
            var bnc = "100205000";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "Thanks for all your efforts";
            var amount = 10.00m;
            var postingKey = 69;
            var executionDate = new DateTime(2017, 1, 1);
            Currency currency = Currency.USD;
            

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.singlepayment, name, account, bnc, amount, "", 0, null, null, reason, postingKey, currency, executionDate));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("Execution date must be today or in future.");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_invalid_periodictimeunit_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "Thanks for all your efforts";
            var sepaReference = "Fake SEPA reference";
            var amount = 10.00m;
            var periodicTimeunit = "Z";
            var periodicTimeunitRotation = 1;
            var periodicFirstExecutionDate = DateTime.Now;
            var periodicLastExecutionDate = DateTime.Now.AddMonths(3);
            Currency currency = Currency.USD;
                        
            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.periodicsinglepaymentsepa, name, iban, bic, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, "", "", new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("The periodicTimeunit must be either 'M' (monthly) or 'W' (weekly).");
        }


        [Fact]
        [Category("PayloadGenerator/BezahlCode")]
        public void bezahlcode_generator_should_throw_invalid_periodictimeunitrotation_exception()
        {
            var iban = "DE33100205000001194700";
            var bic = "BFSWDE33BER";
            var name = "Wikimedia Fördergesellschaft";
            var reason = "Thanks for all your efforts";
            var sepaReference = "Fake SEPA reference";
            var amount = 10.00m;
            var periodicTimeunit = "M";
            var periodicTimeunitRotation = 128;
            var periodicFirstExecutionDate = DateTime.Now;
            var periodicLastExecutionDate = DateTime.Now.AddMonths(3);
            Currency currency = Currency.USD;

            var exception = Record.Exception(() => new PayloadGenerator.BezahlCode(AuthorityType.periodicsinglepaymentsepa, name, iban, bic, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, "", "", new DateTime(2017, 03, 01), reason, sepaReference, currency, DateTime.Now));

            Assert.NotNull(exception);
            Assert.IsType<BezahlCodeException>(exception);
            exception.Message.ShouldBe("The periodicTimeunitRotation must be 1 or greater. (It means repeat the payment every 'periodicTimeunitRotation' weeks/months.");
        }

        
        
        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Reference")]
        public void swissqrcode_generator_should_throw_reference_not_allowed()
        {
            var refType = ReferenceType.NON;
            var reference = "1234567890123456";
            var refTextType = ReferenceTextType.CreditorReferenceIso11649;

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Reference(refType, reference, refTextType));

            Assert.NotNull(exception);
            Assert.IsType<SwissQrCodeReferenceException>(exception);
            exception.Message.ShouldBe("Reference is only allowed when referenceType not equals \"NON\"");
        }

        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Reference")]
        public void swissqrcode_generator_should_throw_missing_reftexttype()
        {
            var refType = ReferenceType.SCOR;
            var reference = "1234567890123456";

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Reference(refType, reference));

            Assert.NotNull(exception);
            Assert.IsType<SwissQrCodeReferenceException>(exception);
            exception.Message.ShouldBe("You have to set an ReferenceTextType when using the reference text.");
        }


        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Reference")]
        public void swissqrcode_generator_should_throw_qrr_ref_too_long()
        {
            var refType = ReferenceType.QRR;
            var reference = "9900050000000003200710123031234654574398214093682164062138462089364";
            var refTextType = ReferenceTextType.QrReference;

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Reference(refType, reference, refTextType));

            Assert.NotNull(exception);
            Assert.IsType<SwissQrCodeReferenceException>(exception);
            exception.Message.ShouldBe("QR-references have to be shorter than 28 chars.");
        }


        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Reference")]
        public void swissqrcode_generator_should_throw_qrr_ref_wrong_char()
        {
            var refType = ReferenceType.QRR;
            var reference = "99000ABCDF5000032007101230";
            var refTextType = ReferenceTextType.QrReference;

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Reference(refType, reference, refTextType));

            Assert.NotNull(exception);
            Assert.IsType<SwissQrCodeReferenceException>(exception);
            exception.Message.ShouldBe("QR-reference must exist out of digits only.");
        }
        

        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Reference")]
        public void swissqrcode_generator_should_throw_qrr_ref_checksum_invalid()
        {
            var refType = ReferenceType.QRR;
            var reference = "990005000000000320071012304";
            var refTextType = ReferenceTextType.QrReference;

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Reference(refType, reference, refTextType));

            Assert.NotNull(exception);
            Assert.IsType<SwissQrCodeReferenceException>(exception);
            exception.Message.ShouldBe("QR-references is invalid. Checksum error.");
        }
        

        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Reference")]
        public void swissqrcode_generator_should_throw_iso11649_ref_too_long()
        {
            var refType = ReferenceType.QRR;
            var reference = "99000500000000032007101230312346545743982162138462089364";
            var refTextType = ReferenceTextType.CreditorReferenceIso11649;

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Reference(refType, reference, refTextType));

            Assert.NotNull(exception);
            Assert.IsType<SwissQrCodeReferenceException>(exception);
            exception.Message.ShouldBe("Creditor references (ISO 11649) have to be shorter than 26 chars.");
        }

        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Reference")]
        public void swissqrcode_generator_should_throw_unstructured_msg_too_long()
        {
            var refType = ReferenceType.QRR;
            var reference = "990005000000000320071012303";
            var refTextType = ReferenceTextType.QrReference;
            var unstructuredMessage = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et mag";

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Reference(refType, reference, refTextType, unstructuredMessage));

            Assert.NotNull(exception);
            Assert.IsType<SwissQrCodeReferenceException>(exception);
            exception.Message.ShouldBe("The unstructured message must be shorter than 141 chars.");
        }

        

        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Iban")]
        public void swissqrcode_generator_should_generate_iban()
        {
            var iban = "CH2609000000857666015";
            var ibanType = PayloadGenerator.SwissQrCode.Iban.IbanType.Iban;

            var generator = new PayloadGenerator.SwissQrCode.Iban(iban, ibanType);

            generator
                .ToString()
                .ShouldBe("CH2609000000857666015");
        }

        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Iban")]
        public void swissqrcode_generator_should_generate_iban_qr()
        {
            var iban = "CH2609000000857666015";
            var ibanType = PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban;

            var generator = new PayloadGenerator.SwissQrCode.Iban(iban, ibanType);

            generator
                .ToString()
                .ShouldBe("CH2609000000857666015");
        }

        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Iban")]
        public void swissqrcode_generator_should_remove_spaces_iban()
        {
            var iban = "CH26 0900 0000 8576 6601 5";
            var ibanType = PayloadGenerator.SwissQrCode.Iban.IbanType.Iban;

            var generator = new PayloadGenerator.SwissQrCode.Iban(iban, ibanType);

            generator
                .ToString()
                .ShouldBe("CH2609000000857666015");
        }

        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Iban")]
        public void swissqrcode_generator_should_throw_invalid_iban()
        {
            var iban = "CHC2609000000857666015";
            var ibanType = PayloadGenerator.SwissQrCode.Iban.IbanType.Iban;
                     
            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Iban(iban, ibanType));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.Iban.SwissQrCodeIbanException>(exception);
            exception.Message.ShouldBe("The IBAN entered isn't valid.");
        }

        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Iban")]
        public void swissqrcode_generator_should_throw_ivalid_iban_country()
        {
            var iban = "DE2609000000857666015";
            var ibanType = PayloadGenerator.SwissQrCode.Iban.IbanType.Iban;

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Iban(iban, ibanType));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.Iban.SwissQrCodeIbanException>(exception);
            exception.Message.ShouldBe("The IBAN must start with \"CH\" or \"LI\".");
        }



        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Contact")]
        public void swissqrcode_generator_should_generate_contact_simple()
        {
            var name = "John Doe";
            var zip = "3003";
            var city = "Bern";
            var country = "CH";
           
            var generator = new PayloadGenerator.SwissQrCode.Contact(name, zip, city, country);

            generator
                .ToString()
                .ShouldBe("John Doe\r\n\r\n\r\n3003\r\nBern\r\nCH\r\n");
        }

        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Contact")]
        public void swissqrcode_generator_should_generate_contact_full()
        {
            var name = "John Doe";
            var zip = "3003";
            var city = "Bern";
            var country = "CH";
            var street = "Parlamentsgebäude";
            var houseNumber = "1";

            var generator = new PayloadGenerator.SwissQrCode.Contact(name, zip, city, country, street, houseNumber);

            generator
                .ToString()
                .ShouldBe("John Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n");
        }


        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Contact")]
        public void swissqrcode_generator_should_throw_name_empty()
        {
            var name = "";
            var zip = "3003";
            var city = "Bern";
            var country = "CH";
            var street = "Parlamentsgebäude";
            var houseNumber = "1";

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Contact(name, zip, city, country, street, houseNumber));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
            exception.Message.ShouldBe("Name must not be empty.");
        }


        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Contact")]
        public void swissqrcode_generator_should_throw_name_too_long()
        {
            var name = "John Dorian Peter Charles Lord of the Rings and Master of Disaster Grayham";
            var zip = "3003";
            var city = "Bern";
            var country = "CH";
            var street = "Parlamentsgebäude";
            var houseNumber = "1";

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Contact(name, zip, city, country, street, houseNumber));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
            exception.Message.ShouldBe("Name must be shorter than 71 chars.");
        }


        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Contact")]
        public void swissqrcode_generator_should_throw_street_too_long()
        {
            var name = "John Doe";
            var zip = "3003";
            var city = "Bern";
            var country = "CH";
            var street = "Parlamentsgebäude in der wunderschönen aber auch ziemlich teuren Stadt Bern in der Schweiz";
            var houseNumber = "1";

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Contact(name, zip, city, country, street, houseNumber));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
            exception.Message.ShouldBe("Street must be shorter than 71 chars.");
        }


        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Contact")]
        public void swissqrcode_generator_should_throw_street_with_illegal_char()
        {
            var name = "John Doe";
            var zip = "3003";
            var city = "Bern";
            var country = "CH";
            var street = "Parlamentsgebäude 1 ♥";
            var houseNumber = "1";

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Contact(name, zip, city, country, street, houseNumber));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
            exception.Message.ShouldBe(@"Street must match the following pattern as defined in pain.001: ^([a-zA-Z0-9\.,;:'\ \-/\(\)?\*\[\]\{\}\\`´~ ]|[!""#%&<>÷=@_$£]|[àáâäçèéêëìíîïñòóôöùúûüýßÀÁÂÄÇÈÉÊËÌÍÎÏÒÓÔÖÙÚÛÜÑ])*$");
        }


        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Contact")]
        public void swissqrcode_generator_should_throw_housenumber_too_long()
        {
            var name = "John Doe";
            var zip = "3003";
            var city = "Bern";
            var country = "CH";
            var street = "Parlamentsgebäude";
            var houseNumber = "123456789123456789";

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Contact(name, zip, city, country, street, houseNumber));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
            exception.Message.ShouldBe("House number must be shorter than 17 chars.");
        }


        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Contact")]
        public void swissqrcode_generator_should_throw_zip_empty()
        {
            var name = "John Doe";
            var zip = "";
            var city = "Bern";
            var country = "CH";
            var street = "Parlamentsgebäude";
            var houseNumber = "1";

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Contact(name, zip, city, country, street, houseNumber));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
            exception.Message.ShouldBe("Zip code must not be empty.");
        }

        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Contact")]
        public void swissqrcode_generator_should_throw_zip_too_long()
        {
            var name = "John Doe";
            var zip = "30031234567891234";
            var city = "Bern";
            var country = "CH";
            var street = "Parlamentsgebäude";
            var houseNumber = "1";

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Contact(name, zip, city, country, street, houseNumber));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
            exception.Message.ShouldBe("Zip code must be shorter than 17 chars.");
        }


        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Contact")]
        public void swissqrcode_generator_should_throw_zip_has_illegal_char()
        {
            var name = "John Doe";
            var zip = "3003CHF♥";
            var city = "Bern";
            var country = "CH";
            var street = "Parlamentsgebäude";
            var houseNumber = "1";

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Contact(name, zip, city, country, street, houseNumber));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
            exception.Message.ShouldBe(@"Zip code must match the following pattern as defined in pain.001: ^([a-zA-Z0-9\.,;:'\ \-/\(\)?\*\[\]\{\}\\`´~ ]|[!""#%&<>÷=@_$£]|[àáâäçèéêëìíîïñòóôöùúûüýßÀÁÂÄÇÈÉÊËÌÍÎÏÒÓÔÖÙÚÛÜÑ])*$");
        }


        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Contact")]
        public void swissqrcode_generator_should_throw_city_empty()
        {
            var name = "John Doe";
            var zip = "3003";
            var city = "";
            var country = "CH";
            var street = "Parlamentsgebäude";
            var houseNumber = "1";

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Contact(name, zip, city, country, street, houseNumber));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
            exception.Message.ShouldBe("City must not be empty.");
        }


        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Contact")]
        public void swissqrcode_generator_should_throw_city_too_long()
        {
            var name = "John Doe";
            var zip = "3003";
            var city = "Berner-Sangerhausen-Ober-Hinter-der-Alm-Stadt-am-Unter-Über-Berg";
            var country = "CH";
            var street = "Parlamentsgebäude";
            var houseNumber = "1";

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Contact(name, zip, city, country, street, houseNumber));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
            exception.Message.ShouldBe("City name must be shorter than 36 chars.");
        }


        [Fact]
        [Category("PayloadGenerator/SwissQrCode.Contact")]
        public void swissqrcode_generator_should_throw_wrong_countrycode()
        {
            var name = "John Doe";
            var zip = "3003";
            var city = "Bern";
            var country = "CHE";
            var street = "Parlamentsgebäude";
            var houseNumber = "1";

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode.Contact(name, zip, city, country, street, houseNumber));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.Contact.SwissQrCodeContactException>(exception);
            exception.Message.ShouldBe("Country must be a valid \"two letter\" country code as defined by  ISO 3166-1, but it isn't.");
        }

           

        [Fact]
        [Category("PayloadGenerator/SwissQrCode")]
        public void swissqrcode_generator_should_generate_swisscode_simple()
        {
            var creditor = new PayloadGenerator.SwissQrCode.Contact("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
            var iban = new PayloadGenerator.SwissQrCode.Iban("CH2609000000857666015", PayloadGenerator.SwissQrCode.Iban.IbanType.Iban);
            var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.QRR, "990005000000000320071012303", ReferenceTextType.QrReference);
            var currency = PayloadGenerator.SwissQrCode.Currency.EUR;

            var generator = new PayloadGenerator.SwissQrCode(iban, currency, creditor, reference);

            generator
                .ToString()
                .ShouldBe("SPC\r\n0100\r\n1\r\nCH2609000000857666015\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nEUR\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nQRR\r\n990005000000000320071012303\r\n\r\n");
        }

        [Fact]
        [Category("PayloadGenerator/SwissQrCode")]
        public void swissqrcode_generator_should_generate_swisscode_full()
        {
            var contactGeneral = new PayloadGenerator.SwissQrCode.Contact("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
            var iban = new PayloadGenerator.SwissQrCode.Iban("CH2609000000857666015", PayloadGenerator.SwissQrCode.Iban.IbanType.Iban);
            var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.QRR, "990005000000000320071012303", ReferenceTextType.QrReference);
            var currency = PayloadGenerator.SwissQrCode.Currency.CHF;
            var amount = 100.25m;
            var reqDateOfPayment = new DateTime(2017, 03, 01);
           
            var generator = new PayloadGenerator.SwissQrCode(iban, currency, contactGeneral, reference, contactGeneral, amount, reqDateOfPayment, contactGeneral);

            generator
                .ToString()
                .ShouldBe("SPC\r\n0100\r\n1\r\nCH2609000000857666015\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n100,25\r\nCHF\r\n2017-03-01\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\nQRR\r\n990005000000000320071012303\r\n\r\n");
        }


        [Fact]
        [Category("PayloadGenerator/SwissQrCode")]
        public void swissqrcode_generator_should_generate_swisscode_full_alt()
        {
            var contactGeneral = new PayloadGenerator.SwissQrCode.Contact("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
            var iban = new PayloadGenerator.SwissQrCode.Iban("CH2609000000857666015", PayloadGenerator.SwissQrCode.Iban.IbanType.Iban);
            var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.QRR, "990005000000000320071012303", ReferenceTextType.QrReference);
            var currency = PayloadGenerator.SwissQrCode.Currency.CHF;
            var amount = 100.25m;
            var reqDateOfPayment = new DateTime(2017, 03, 01);

            var generator = new PayloadGenerator.SwissQrCode(iban, currency, contactGeneral, reference, contactGeneral, amount, reqDateOfPayment, contactGeneral, "alt1", "alt2");

            generator
                .ToString()
                .ShouldBe("SPC\r\n0100\r\n1\r\nCH2609000000857666015\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\n100,25\r\nCHF\r\n2017-03-01\r\nJohn Doe\r\nParlamentsgebäude\r\n1\r\n3003\r\nBern\r\nCH\r\nQRR\r\n990005000000000320071012303\r\n\r\nalt1\r\nalt2\r\n");
        }

        [Fact]
        [Category("PayloadGenerator/SwissQrCode")]
        public void swissqrcode_generator_should_throw_amount_too_big()
        {
            var contactGeneral = new PayloadGenerator.SwissQrCode.Contact("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
            var iban = new PayloadGenerator.SwissQrCode.Iban("CH2609000000857666015", PayloadGenerator.SwissQrCode.Iban.IbanType.Iban);
            var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.QRR, "990005000000000320071012303", ReferenceTextType.QrReference);
            var currency = PayloadGenerator.SwissQrCode.Currency.CHF;
            var amount = 1234567891.25m;
            var reqDateOfPayment = new DateTime(2017, 03, 01);

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode(iban, currency, contactGeneral, reference, contactGeneral, amount, reqDateOfPayment, contactGeneral));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.SwissQrCodeException>(exception);
            exception.Message.ShouldBe("Amount (including decimals) must be shorter than 13 places.");
        }

        [Fact]
        [Category("PayloadGenerator/SwissQrCode")]
        public void swissqrcode_generator_should_throw_incompatible_reftype()
        {
            var contactGeneral = new PayloadGenerator.SwissQrCode.Contact("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
            var iban = new PayloadGenerator.SwissQrCode.Iban("CH2609000000857666015", PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban);
            var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.NON);
            var currency = PayloadGenerator.SwissQrCode.Currency.CHF;
            var amount = 100.25m;
            var reqDateOfPayment = new DateTime(2017, 03, 01);

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode(iban, currency, contactGeneral, reference, contactGeneral, amount, reqDateOfPayment, contactGeneral));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.SwissQrCodeException>(exception);
            exception.Message.ShouldBe("If QR-IBAN is used, you have to choose \"QRR\" or \"SCOR\" as reference type!");
        }

        [Fact]
        [Category("PayloadGenerator/SwissQrCode")]
        public void swissqrcode_generator_should_throw_alt1_too_long()
        {
            var contactGeneral = new PayloadGenerator.SwissQrCode.Contact("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
            var iban = new PayloadGenerator.SwissQrCode.Iban("CH2609000000857666015", PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban);
            var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.QRR);
            var currency = PayloadGenerator.SwissQrCode.Currency.CHF;
            var amount = 100.25m;
            var reqDateOfPayment = new DateTime(2017, 03, 01);
            var alt1 = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean ma";

            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode(iban, currency, contactGeneral, reference, contactGeneral, amount, reqDateOfPayment, contactGeneral, alt1));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.SwissQrCodeException>(exception);
            exception.Message.ShouldBe("Alternative procedure information block 1 must be shorter than 101 chars.");
        }

        [Fact]
        [Category("PayloadGenerator/SwissQrCode")]
        public void swissqrcode_generator_should_throw_alt2_too_long()
        {
            var contactGeneral = new PayloadGenerator.SwissQrCode.Contact("John Doe", "3003", "Bern", "CH", "Parlamentsgebäude", "1");
            var iban = new PayloadGenerator.SwissQrCode.Iban("CH2609000000857666015", PayloadGenerator.SwissQrCode.Iban.IbanType.QrIban);
            var reference = new PayloadGenerator.SwissQrCode.Reference(ReferenceType.QRR);
            var currency = PayloadGenerator.SwissQrCode.Currency.CHF;
            var amount = 100.25m;
            var reqDateOfPayment = new DateTime(2017, 03, 01);
            var alt1 = "lorem ipsum";
            var alt2 = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean ma";
            var exception = Record.Exception(() => new PayloadGenerator.SwissQrCode(iban, currency, contactGeneral, reference, contactGeneral, amount, reqDateOfPayment, contactGeneral, alt1, alt2));

            Assert.NotNull(exception);
            Assert.IsType<PayloadGenerator.SwissQrCode.SwissQrCodeException>(exception);
            exception.Message.ShouldBe("Alternative procedure information block 2 must be shorter than 101 chars.");
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
        [Category("PayloadGenerator/ContactData")]
        public void contactdata_generator_can_generate_payload_simple_mecard()
        {
            var firstname = "John";
            var lastname = "Doe";
            var outputType = PayloadGenerator.ContactData.ContactOutputType.MeCard;

            var generator = new PayloadGenerator.ContactData(outputType, firstname, lastname);

            generator
                .ToString()
                .ShouldBe("MECARD+\r\nN:Doe, John\r\nADR:,,,,,,");
        }

        [Fact]
        [Category("PayloadGenerator/ContactData")]
        public void contactdata_generator_can_generate_payload_full_mecard()
        {
            var firstname = "John";
            var lastname = "Doe";
            var nickname = "Johnny";
            var phone = "+4253212222";
            var mobilePhone = "+421701234567";
            var workPhone = "+4253211337";
            var email = "me@john.doe";
            var birthday = new DateTime(1970,02,01);
            var website = "http://john.doe";
            var street = "Long street";
            var houseNumber = "42";
            var city = "Super-Town";
            var zipCode = "12345";
            var country = "Starlight Country";
            var note = "Badass programmer.";
            var outputType = PayloadGenerator.ContactData.ContactOutputType.MeCard;

            var generator = new PayloadGenerator.ContactData(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email, birthday, website, street, houseNumber, city, zipCode, country, note);

            generator
                .ToString()
                .ShouldBe("MECARD+\r\nN:Doe, John\r\nTEL:+4253212222\r\nTEL:+421701234567\r\nTEL:+4253211337\r\nEMAIL:me@john.doe\r\nNOTE:Badass programmer.\r\nBDAY:19700201\r\nADR:,,Long street 42,Super-Town,,12345,Starlight Country\r\nURL:http://john.doe\r\nNICKNAME:Johnny");
        }

        [Fact]
        [Category("PayloadGenerator/ContactData")]
        public void contactdata_generator_can_generate_payload_full_vcard21()
        {
            var firstname = "John";
            var lastname = "Doe";
            var nickname = "Johnny";
            var phone = "+4253212222";
            var mobilePhone = "+421701234567";
            var workPhone = "+4253211337";
            var email = "me@john.doe";
            var birthday = new DateTime(1970, 02, 01);
            var website = "http://john.doe";
            var street = "Long street";
            var houseNumber = "42";
            var city = "Super-Town";
            var zipCode = "12345";
            var country = "Starlight Country";
            var note = "Badass programmer.";
            var outputType = PayloadGenerator.ContactData.ContactOutputType.VCard21;

            var generator = new PayloadGenerator.ContactData(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email, birthday, website, street, houseNumber, city, zipCode, country, note);

            generator
                .ToString()
                .ShouldBe("BEGIN:VCARD\r\nVERSION:2.1\r\nN:Doe;John;;;\r\nFN:John Doe\r\nTEL;HOME;VOICE:+4253212222\r\nTEL;HOME;CELL:+421701234567\r\nTEL;WORK;VOICE:+4253211337\r\nADR;HOME;PREF:;;Long street 42;Super-Town;;12345;Starlight Country\r\nBDAY:19700201\r\nURL:http://john.doe\r\nEMAIL:me@john.doe\r\nNOTE:Badass programmer.\r\nEND:VCARD");
        }

        [Fact]
        [Category("PayloadGenerator/ContactData")]
        public void contactdata_generator_can_generate_payload_full_vcard3()
        {
            var firstname = "John";
            var lastname = "Doe";
            var nickname = "Johnny";
            var phone = "+4253212222";
            var mobilePhone = "+421701234567";
            var workPhone = "+4253211337";
            var email = "me@john.doe";
            var birthday = new DateTime(1970, 02, 01);
            var website = "http://john.doe";
            var street = "Long street";
            var houseNumber = "42";
            var city = "Super-Town";
            var zipCode = "12345";
            var country = "Starlight Country";
            var note = "Badass programmer.";
            var outputType = PayloadGenerator.ContactData.ContactOutputType.VCard3;

            var generator = new PayloadGenerator.ContactData(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email, birthday, website, street, houseNumber, city, zipCode, country, note);

            generator
                .ToString()
                .ShouldBe("BEGIN:VCARD\r\nVERSION:3.0\r\nN:Doe;John;;;\r\nFN:John Doe\r\nTEL;TYPE=HOME,VOICE:+4253212222\r\nTEL;TYPE=HOME,CELL:+421701234567\r\nTEL;TYPE=WORK,VOICE:+4253211337\r\nADR;TYPE=HOME,PREF:;;Long street 42;Super-Town;;12345;Starlight Country\r\nBDAY:19700201\r\nURL:http://john.doe\r\nEMAIL:me@john.doe\r\nNOTE:Badass programmer.\r\nNICKNAME:Johnny\r\nEND:VCARD");
        }

        [Fact]
        [Category("PayloadGenerator/ContactData")]
        public void contactdata_generator_can_generate_payload_full_vcard4()
        {
            var firstname = "John";
            var lastname = "Doe";
            var nickname = "Johnny";
            var phone = "+4253212222";
            var mobilePhone = "+421701234567";
            var workPhone = "+4253211337";
            var email = "me@john.doe";
            var birthday = new DateTime(1970, 02, 01);
            var website = "http://john.doe";
            var street = "Long street";
            var houseNumber = "42";
            var city = "Super-Town";
            var zipCode = "12345";
            var country = "Starlight Country";
            var note = "Badass programmer.";
            var outputType = PayloadGenerator.ContactData.ContactOutputType.VCard4;

            var generator = new PayloadGenerator.ContactData(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email, birthday, website, street, houseNumber, city, zipCode, country, note);

            generator
                .ToString()
                .ShouldBe("BEGIN:VCARD\r\nVERSION:4.0\r\nN:Doe;John;;;\r\nFN:John Doe\r\nTEL;TYPE=home,voice;VALUE=uri:tel:+4253212222\r\nTEL;TYPE=home,cell;VALUE=uri:tel:+421701234567\r\nTEL;TYPE=work,voice;VALUE=uri:tel:+4253211337\r\nADR;TYPE=home,pref:;;Long street 42;Super-Town;;12345;Starlight Country\r\nBDAY:19700201\r\nURL:http://john.doe\r\nEMAIL:me@john.doe\r\nNOTE:Badass programmer.\r\nNICKNAME:Johnny\r\nEND:VCARD");
        }

        [Fact]
        [Category("PayloadGenerator/WhatsAppMessage")]
        public void whatsapp_generator_can_generate_payload_simple()
        {
            var msg = "This is a sample message with Umlauts: Ä,ö, ü and ß.";
            var generator = new PayloadGenerator.WhatsAppMessage(msg);

            generator
                .ToString()
                .ShouldBe("whatsapp://send?text=This%20is%20a%20sample%20message%20with%20Umlauts%3A%20%C3%84%2C%C3%B6%2C%20%C3%BC%20and%20%C3%9F.");
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



