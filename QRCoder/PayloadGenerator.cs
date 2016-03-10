using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QRCoder
{
    public static class PayloadGenerator
    {

        public class WiFi
        {
            private string SSID, password, authenticationMode;
            private bool isHiddenSSID;
            public WiFi(string SSID, string password, Authentication authenticationMode, bool isHiddenSSID = false)
            {
                this.SSID = escapeInput(SSID);
                this.SSID = isHexStyle(this.SSID) ? "\"" + this.SSID + "\"" : this.SSID;
                this.password = escapeInput(password);
                this.password = isHexStyle(this.password) ? "\"" + this.password + "\"" : this.password;
                this.authenticationMode = authenticationMode.ToString();
                this.isHiddenSSID = isHiddenSSID;
            }

            public override string ToString()
            {
                return String.Format("WIFI:T:{0};S:{1};P:{2};{3};", authenticationMode, SSID, password, (isHiddenSSID ? "H:true" : string.Empty));
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
            private string mailReceiver, subject, message;
            private MailEncoding encoding;

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
                switch (encoding)
                {
                    case MailEncoding.MAILTO:
                        return String.Format("mailto:{0}?subject={1}&body={2}", mailReceiver, System.Uri.EscapeDataString(subject), System.Uri.EscapeDataString(message));
                    case MailEncoding.MATMSG:
                        return String.Format("MATMSG:TO:{0};SUB:{1};BODY:{2};;", mailReceiver, escapeInput(subject), escapeInput(message));
                    case MailEncoding.SMTP:
                        return String.Format("SMTP:{0}:{1}:{2}", mailReceiver, escapeInput(subject, true), escapeInput(message, true));
                    default:
                        return mailReceiver;
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
            private string number, subject;
            private SMSEncoding encoding;

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
                switch (encoding)
                { 
                    case SMSEncoding.SMS:
                        return String.Format("sms:{0}?body={1}", number, System.Uri.EscapeDataString(subject));
                    case SMSEncoding.SMS_iOS:
                        return String.Format("sms:{0};body={1}", number, System.Uri.EscapeDataString(subject));
                    case SMSEncoding.SMSTO:
                        return String.Format("SMSTO:{0}:{1}", number, subject);
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
            private string number, subject;
            private MMSEncoding encoding;

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
                switch (encoding)
                {
                    case MMSEncoding.MMSTO:
                        return String.Format("mmsto:{0}?subject={1}", number, System.Uri.EscapeDataString(subject));
                    case MMSEncoding.MMS:
                        return String.Format("mms:{0}?body={1}", number, System.Uri.EscapeDataString(subject));
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
            private string latitude, longitude;
            private GeolocationEncoding encoding;
            public Geolocation(string latitude, string longitude, GeolocationEncoding encoding = GeolocationEncoding.GEO)
            {
                this.latitude = latitude.Replace(",",".");
                this.longitude = longitude.Replace(",", ".");
                this.encoding = encoding;
            }

            public override string ToString()
            {
                switch (encoding)
                {
                    case GeolocationEncoding.GEO:
                        return String.Format("geo:{0},{1}", latitude, longitude);
                    case GeolocationEncoding.GoogleMaps:
                        return String.Format("http://maps.google.com/maps?q={0},{1}", latitude, longitude);
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
            private string number;
            public PhoneNumber(string number)
            {
                this.number = number;
            }

            public override string ToString()
            {
                return "tel:"+number;
            }
        }

        public class Url
        {
            private string url;
            public Url(string url)
            {
                this.url = url;
            }

            public override string ToString()
            {
                return (!url.StartsWith("http") ? "http://" + url : url);
            }
        }

        public class Bookmark
        {
            private string url, title;
            public Bookmark(string url, string title)
            {
                this.url = escapeInput(url);
                this.title = escapeInput(title);
            }

            public override string ToString()
            {
                return String.Format("MEBKM:TITLE:{0};URL:{1};;", title, url);
            }
        }
      
        private static string escapeInput(string inp, bool simple = false)
        {
            char[] forbiddenChars = { '\\', ';', ',', ':' };
            if (simple) { forbiddenChars = new char[1] { ':' }; }
            foreach (char c in forbiddenChars)
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
