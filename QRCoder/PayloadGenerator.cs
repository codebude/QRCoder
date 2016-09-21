using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace QRCoder
{
    public static class PayloadGenerator
    {

        public class WiFi
        {
            private readonly string ssid, password, authenticationMode;
            private readonly bool isHiddenSsid;
            public WiFi(string ssid, string password, Authentication authenticationMode, bool isHiddenSSID = false)
            {
                this.ssid = EscapeInput(ssid);
                this.ssid = isHexStyle(this.ssid) ? "\"" + this.ssid + "\"" : this.ssid;
                this.password = EscapeInput(password);
                this.password = isHexStyle(this.password) ? "\"" + this.password + "\"" : this.password;
                this.authenticationMode = authenticationMode.ToString();
                this.isHiddenSsid = isHiddenSSID;
            }

            public override string ToString()
            {
                return
                    $"WIFI:T:{this.authenticationMode};S:{this.ssid};P:{this.password};{(this.isHiddenSsid ? "H:true" : string.Empty)};";
            }

            public enum Authentication
            {
                WEP,
                WPA,
                nopass
            }
        }


        public class Mail
        {
            private readonly string mailReceiver, subject, message;
            private readonly MailEncoding encoding;

            public Mail(string mailReceiver, MailEncoding encoding = MailEncoding.MAILTO)
            {
                this.mailReceiver = mailReceiver;
                this.subject = this.message = string.Empty;
                this.encoding = encoding;
            }

            public Mail(string mailReceiver, string subject, MailEncoding encoding = MailEncoding.MAILTO)
            {
                this.mailReceiver = mailReceiver;
                this.subject = subject;
                this.message = string.Empty;
                this.encoding = encoding;
            }

            public Mail(string mailReceiver, string subject, string message, MailEncoding encoding = MailEncoding.MAILTO)
            {
                this.mailReceiver = mailReceiver;
                this.subject = subject;
                this.message = message;
                this.encoding = encoding;
            }


            public override string ToString()
            {
                switch (this.encoding)
                {
                    case MailEncoding.MAILTO:
                        return
                            $"mailto:{this.mailReceiver}?subject={System.Uri.EscapeDataString(this.subject)}&body={System.Uri.EscapeDataString(this.message)}";
                    case MailEncoding.MATMSG:
                        return
                            $"MATMSG:TO:{this.mailReceiver};SUB:{EscapeInput(this.subject)};BODY:{EscapeInput(this.message)};;";
                    case MailEncoding.SMTP:
                        return
                            $"SMTP:{this.mailReceiver}:{EscapeInput(this.subject, true)}:{EscapeInput(this.message, true)}";
                    default:
                        return this.mailReceiver;
                }
            }

            public enum MailEncoding
            {
                MAILTO,
                MATMSG,
                SMTP
            }
        }

        public class SMS
        {
            private readonly string number, subject;
            private readonly SMSEncoding encoding;

            public SMS(string number, SMSEncoding encoding = SMSEncoding.SMS)
            {
                this.number = number;
                this.subject = string.Empty;
                this.encoding = encoding;
            }

            public SMS(string number, string subject, SMSEncoding encoding = SMSEncoding.SMS)
            {
                this.number = number;
                this.subject = subject;
                this.encoding = encoding;
            }


            public override string ToString()
            {
                switch (this.encoding)
                { 
                    case SMSEncoding.SMS:
                        return $"sms:{this.number}?body={System.Uri.EscapeDataString(this.subject)}";
                    case SMSEncoding.SMS_iOS:
                        return $"sms:{this.number};body={System.Uri.EscapeDataString(this.subject)}";
                    case SMSEncoding.SMSTO:
                        return $"SMSTO:{this.number}:{this.subject}";
                    default:
                        return "sms:";
                }
            }

            public enum SMSEncoding
            {
                SMS,
                SMSTO,
                SMS_iOS
            }
        }

        public class MMS
        {
            private readonly string number, subject;
            private readonly MMSEncoding encoding;

            public MMS(string number, MMSEncoding encoding = MMSEncoding.MMS)
            {
                this.number = number;
                this.subject = string.Empty;
                this.encoding = encoding;
            }

            public MMS(string number, string subject, MMSEncoding encoding = MMSEncoding.MMS)
            {
                this.number = number;
                this.subject = subject;
                this.encoding = encoding;
            }
            
            public override string ToString()
            {
                switch (this.encoding)
                {
                    case MMSEncoding.MMSTO:
                        return $"mmsto:{this.number}?subject={System.Uri.EscapeDataString(this.subject)}";
                    case MMSEncoding.MMS:
                        return $"mms:{this.number}?body={System.Uri.EscapeDataString(this.subject)}";
                    default:
                        return "mms:";
                }
                
            }

            public enum MMSEncoding
            {
                MMS,
                MMSTO
            }

        }

        public class Geolocation
        {
            private readonly string latitude, longitude;
            private readonly GeolocationEncoding encoding;
            public Geolocation(string latitude, string longitude, GeolocationEncoding encoding = GeolocationEncoding.GEO)
            {
                this.latitude = latitude.Replace(",",".");
                this.longitude = longitude.Replace(",", ".");
                this.encoding = encoding;
            }

            public override string ToString()
            {
                switch (this.encoding)
                {
                    case GeolocationEncoding.GEO:
                        return $"geo:{this.latitude},{this.longitude}";
                    case GeolocationEncoding.GoogleMaps:
                        return $"http://maps.google.com/maps?q={this.latitude},{this.longitude}";
                    default:
                        return "geo:";
                }
            }

            public enum GeolocationEncoding
            {
                GEO,
                GoogleMaps
            }
        }

        public class PhoneNumber
        {
            private readonly string number;
            public PhoneNumber(string number)
            {
                this.number = number;
            }

            public override string ToString()
            {
                return $"tel:{this.number}";
            }
        }


        public class SkypeCall
        {
            private readonly string skypeUsername;
            public SkypeCall(string skypeUsername)
            {
                this.skypeUsername = skypeUsername;
            }

            public override string ToString()
            {
                return $"skype:{this.skypeUsername}?call";
            }
        }

        public class Url
        {
            private readonly string url;
            public Url(string url)
            {
                this.url = url;
            }

            public override string ToString()
            {
                return (!this.url.StartsWith("http") ? "http://" + this.url : this.url);
            }
        }

        public class Bookmark
        {
            private readonly string url, title;
            public Bookmark(string url, string title)
            {
                this.url = EscapeInput(url);
                this.title = EscapeInput(title);
            }

            public override string ToString()
            {
                return $"MEBKM:TITLE:{this.title};URL:{this.url};;";
            }
        }

        public class BitcoinAddress
        {
            private readonly string address, label, message;
            private readonly double? amount;

            public BitcoinAddress(string address, double? amount, string label = null, string message = null)
            {
                this.address = address;

                if (!string.IsNullOrEmpty(label))
                {
                    this.label = Uri.EscapeUriString(label);
                }

                if (!string.IsNullOrEmpty(message))
                {
                    this.message = Uri.EscapeUriString(message);
                }

                this.amount = amount;
            }

            public override string ToString()
            {
                string query = null;

                var queryValues = new List<KeyValuePair<string,string>>{
                  new KeyValuePair<string, string>(nameof(label), label),
                  new KeyValuePair<string, string>(nameof(message), message),
                  new KeyValuePair<string, string>(nameof(amount), amount.HasValue ? amount.Value.ToString("#.########", CultureInfo.InvariantCulture) : null)
                };

                if (queryValues.Any(keyPair => !string.IsNullOrEmpty(keyPair.Value)))
                {
                    query = "?" + string.Join("&", queryValues
                        .Where(keyPair => !string.IsNullOrEmpty(keyPair.Value))
                        .Select(keyPair => $"{keyPair.Key}={keyPair.Value}")
                        .ToArray());
                }

                return $"bitcoin:{address}{query}";
            }
        }


        public class CalendarEvent
        {
            private readonly string subject, description, location, start, end;
            private readonly EventEncoding encoding;

            public CalendarEvent(string subject, string description, string location, DateTime start, DateTime end, bool allDayEvent, EventEncoding encoding = EventEncoding.Universal)
            {
                this.subject = subject;
                this.description = description;
                this.location = location;
                this.encoding = encoding;
                string dtFormat = allDayEvent ? "yyyyMMdd" : "yyyyMMddTHHmmss";
                this.start = start.ToString(dtFormat);
                this.end = end.ToString(dtFormat);
            }

            public override string ToString()
            {
                var vEvent = $"BEGIN:VEVENT{Environment.NewLine}";
                vEvent += $"SUMMARY:{this.subject}{Environment.NewLine}";
                vEvent += !string.IsNullOrEmpty(this.description) ? $"DESCRIPTION:{this.description}{Environment.NewLine}" : "";
                vEvent += !string.IsNullOrEmpty(this.location) ? $"LOCATION:{this.location}{Environment.NewLine}" : "";
                vEvent += $"DTSTART:{this.start}{Environment.NewLine}";
                vEvent += $"DTEND:{this.end}{Environment.NewLine}";
                vEvent += "END:VEVENT";

                if (this.encoding.Equals(EventEncoding.iCalComplete))
                    vEvent = $@"BEGIN:VCALENDAR{Environment.NewLine}VERSION:2.0{Environment.NewLine}{vEvent}{Environment.NewLine}END:VCALENDAR";
                
                return vEvent;
            }

            public enum EventEncoding
            {
                iCalComplete,
                Universal
            }
        }

        private static string EscapeInput(string inp, bool simple = false)
        {
            char[] forbiddenChars = { '\\', ';', ',', ':' };
            if (simple) { forbiddenChars = new char[1] { ':' }; }
            foreach (var c in forbiddenChars)
            {
                inp = inp.Replace(c.ToString(), "\\" + c);
            }
            return inp;
        }

        private static bool isHexStyle(string inp)
        {
            return (System.Text.RegularExpressions.Regex.IsMatch(inp, @"\A\b[0-9a-fA-F]+\b\Z") || System.Text.RegularExpressions.Regex.IsMatch(inp, @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"));
        }
    }
}
