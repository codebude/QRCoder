using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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

        public class Girocode
        {
            //Keep in mind, that the ECC level has to be set to "M" when generating a Girocode!
            //Girocode specification: http://www.europeanpaymentscouncil.eu/index.cfm/knowledge-bank/epc-documents/quick-response-code-guidelines-to-enable-data-capture-for-the-initiation-of-a-sepa-credit-transfer/epc069-12-quick-response-code-guidelines-to-enable-data-capture-for-the-initiation-of-a-sepa-credit-transfer1/

            private string br = "\n";
            private readonly string iban, bic, name, purposeOfCreditTransfer, remittanceInformation, messageToGirocodeUser;
            private readonly decimal amount;
            private readonly GirocodeVersion version;
            private readonly GirocodeEncoding encoding;
            private readonly TypeOfRemittance typeOfRemittance;

            /// <summary>
            /// Generates the payload for a Girocode (QR-Code with credit transfer information).
            /// Attention: When using Girocode payload, QR code must be generated with ECC level M!
            /// </summary>
            /// <param name="iban">Account number of the Beneficiary. Only IBAN is allowed.</param>
            /// <param name="bic">BIC of the Beneficiary Bank.</param>
            /// <param name="name">Name of the Beneficiary.</param>
            /// <param name="amount">Amount of the Credit Transfer in Euro.
            /// (Amount must be more than 0.01 and less than 999999999.99)</param>
            /// <param name="remittanceInformation">Remittance Information (Purpose-/reference text). (optional)</param>
            /// <param name="typeOfRemittance">Type of remittance information. Either structured (e.g. ISO 11649 RF Creditor Reference) and max. 35 chars or unstructured and max. 140 chars.</param>
            /// <param name="purposeOfCreditTransfer">Purpose of the Credit Transfer (optional)</param>
            /// <param name="messageToGirocodeUser">Beneficiary to originator information. (optional)</param>
            /// <param name="version">Girocode version. Either 001 or 002. Default: 001.</param>
            /// <param name="encoding">Encoding of the Girocode payload. Default: ISO-8859-1</param>
            public Girocode(string iban, string bic, string name, decimal amount, string remittanceInformation = "", TypeOfRemittance typeOfRemittance = TypeOfRemittance.Unstructured, string purposeOfCreditTransfer = "", string messageToGirocodeUser = "", GirocodeVersion version = GirocodeVersion.Version1, GirocodeEncoding encoding = GirocodeEncoding.ISO_8859_1)
            {
                this.version = version;
                this.encoding = encoding;
                if (!Regex.IsMatch(iban.Replace(" ",""), @"[a-zA-Z]{2}[0-9]{2}[a-zA-Z0-9]{4}[0-9]{7}([a-zA-Z0-9]?){0,16}"))
                    throw new GirocodeException("The IBAN entered isn't valid.");
                this.iban = iban.Replace(" ","").ToUpper();
                if (!Regex.IsMatch(bic.Replace(" ", ""), @"([a-zA-Z]{4}[a-zA-Z]{2}[a-zA-Z0-9]{2}([a-zA-Z0-9]{3})?)"))
                    throw new GirocodeException("The BIC entered isn't valid.");
                this.bic = bic.Replace(" ", "").ToUpper();
                if (name.Length > 70)
                    throw new GirocodeException("(Payee-)Name must be shorter than 71 chars.");
                this.name = name;
                if (amount.ToString().Replace(",", ".").Contains(".") && amount.ToString().Replace(",",".").Split('.')[1].TrimEnd('0').Length > 2)
                    throw new GirocodeException("Amount must have less than 3 digits after decimal point.");
                if (amount < 0.01m || amount > 999999999.99m)
                    throw new GirocodeException("Amount has to at least 0.01 and must be smaller or equal to 999999999.99.");
                this.amount = amount;
                if (purposeOfCreditTransfer.Length > 4)
                    throw new GirocodeException("Purpose of credit transfer can only have 4 chars at maximum.");
                this.purposeOfCreditTransfer = purposeOfCreditTransfer;
                if (typeOfRemittance.Equals(TypeOfRemittance.Unstructured) && remittanceInformation.Length > 140)
                    throw new GirocodeException("Unstructured reference texts have to shorter than 141 chars.");
                if (typeOfRemittance.Equals(TypeOfRemittance.Structured) && remittanceInformation.Length > 35)
                    throw new GirocodeException("Structured reference texts have to shorter than 36 chars.");
                this.typeOfRemittance = typeOfRemittance;
                this.remittanceInformation = remittanceInformation;
                if (messageToGirocodeUser.Length > 70)
                    throw new GirocodeException("Message to the Girocode-User reader texts have to shorter than 71 chars.");
                this.messageToGirocodeUser = messageToGirocodeUser;
            }

            public override string ToString()
            {
                var girocodePayload = "BCD" + br;
                girocodePayload += (version.Equals(GirocodeVersion.Version1) ? "001" : "002") + br;
                girocodePayload += (int)encoding + 1 + br;
                girocodePayload += "SCT" + br;
                girocodePayload += bic + br;
                girocodePayload += name + br;
                girocodePayload += iban + br;
                girocodePayload += $"EUR{amount:0.00}".Replace(",",".") + br;
                girocodePayload += purposeOfCreditTransfer + br;
                girocodePayload += (typeOfRemittance.Equals(TypeOfRemittance.Structured)
                    ? remittanceInformation
                    : string.Empty) + br;
                girocodePayload += (typeOfRemittance.Equals(TypeOfRemittance.Unstructured)
                    ? remittanceInformation
                    : string.Empty) + br;
                girocodePayload += messageToGirocodeUser;

                return ConvertStringToEncoding(girocodePayload, encoding.ToString().Replace("_","-"));
            }

            public enum GirocodeVersion
            {
                Version1,
                Version2
            }

            public enum TypeOfRemittance
            {
                Structured,
                Unstructured
            }

            public enum GirocodeEncoding
            {
                UTF_8,
                ISO_8859_1,
                ISO_8859_2,
                ISO_8859_4,
                ISO_8859_5,
                ISO_8859_7,
                ISO_8859_10,
                ISO_8859_15
            }

            public class GirocodeException : Exception
            {
                public GirocodeException()
                {
                }

                public GirocodeException(string message)
                    : base(message)
                {
                }

                public GirocodeException(string message, Exception inner)
                    : base(message, inner)
                {
                }
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

        public class OneTimePassword
        {
            //https://github.com/google/google-authenticator/wiki/Key-Uri-Format
            public OneTimePasswordAuthType Type { get; set; } = OneTimePasswordAuthType.TOTP;
            public string Secret { get; set; }
            public OoneTimePasswordAuthAlgorithm Algorithm { get; set; } = OoneTimePasswordAuthAlgorithm.SHA1;
            public string Issuer { get; set; }
            public string Label { get; set; }
            public int Digits { get; set; } = 6;
            public int? Counter { get; set; } = null;
            public int? Period { get; set; } = 30;

            public enum OneTimePasswordAuthType
            {
                TOTP,
                HOTP,
            }

            public enum OoneTimePasswordAuthAlgorithm
            {
                SHA1,
                SHA256,
                SHA512,
            }

            public override string ToString()
            {
                switch (Type)
                {
                    case OneTimePasswordAuthType.TOTP:
                        return TimeToString();
                    case OneTimePasswordAuthType.HOTP:
                        return HMACToString();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Note: Issuer:Label must only contain 1 : if either of the Issuer or the Label has a : then it is invalid.
            // Defaults are 6 digits and 30 for Period

            private string HMACToString()
            {
                var sb = new StringBuilder("otpauth://hotp/");
                ProcessCommonFields(sb);
                var actualCounter = Counter ?? 1;
                sb.Append("&counter=" + actualCounter);
                return sb.ToString();
            }

            private string TimeToString()
            {
                if (Period == null)
                {
                    throw new Exception("Period must be set when using OneTimePasswordAuthType.TOTP");
                }

                var sb = new StringBuilder("otpauth://totp/");

                ProcessCommonFields(sb);

                if (Period != 30)
                {
                    sb.Append("&period=" + Period);
                }

                return sb.ToString();
            }

            private void ProcessCommonFields(StringBuilder sb)
            {
                if (String40Methods.IsNullOrWhiteSpace(Secret))
                {
                    throw new Exception("Secret must be a filled out base32 encoded string");
                }
                string strippedSecret = Secret.Replace(" ", "");
                string escapedIssuer = null;
                string escapedLabel = null;

                if (!String40Methods.IsNullOrWhiteSpace(Issuer))
                {
                    if (Issuer.Contains(":"))
                    {
                        throw new Exception("Issuer must not have a ':'");
                    }
                    escapedIssuer = Uri.EscapeUriString(Issuer);
                }

                if (!String40Methods.IsNullOrWhiteSpace(Label))
                {
                    if (Label.Contains(":"))
                    {
                        throw new Exception("Label must not have a ':'");
                    }
                    escapedLabel = Uri.EscapeUriString(Label);
                }

                if (escapedLabel != null)
                {
                    if (escapedIssuer != null)
                    {
                        escapedLabel = escapedIssuer + ":" + escapedLabel;
                    }
                }
                else if (escapedIssuer != null)
                {
                    escapedLabel = escapedIssuer;
                }

                if (escapedLabel != null)
                {
                    sb.Append(escapedLabel);
                }

                sb.Append("?secret=" + strippedSecret);

                if (escapedIssuer != null)
                {
                    sb.Append("&issuer=" + escapedIssuer);
                }

                if (Digits != 6)
                {
                    sb.Append("&digits=" + Digits);
                }
            }
        }


        public class ShadowSocksConfig
        {
            private readonly string hostname, password, tag, methodStr;
            private readonly Method method;
            private readonly int port;
            private Dictionary<string, string> encryptionTexts = new Dictionary<string, string>() {
                { "Aes128Cfb", "aes-128-cfb" },
                { "Aes128Cfb1", "aes-128-cfb1" },
                { "Aes128Cfb8", "aes-128-cfb8" },
                { "Aes128Ctr", "aes-128-ctr" },
                { "Aes128Ofb", "aes-128-ofb" },
                { "Aes192Cfb", "aes-192-cfb" },
                { "Aes192Cfb1", "aes-192-cfb1" },
                { "Aes192Cfb8", "aes-192-cfb8" },
                { "Aes192Ctr", "aes-192-ctr" },
                { "Aes192Ofb", "aes-192-ofb" },
                { "Aes256Cb", "aes-256-cfb" },
                { "Aes256Cfb1", "aes-256-cfb1" },
                { "Aes256Cfb8", "aes-256-cfb8" },
                { "Aes256Ctr", "aes-256-ctr" },
                { "Aes256Ofb", "aes-256-ofb" },
                { "BfCfb", "bf-cfb" },
                { "Camellia128Cfb", "camellia-128-cfb" },
                { "Camellia192Cfb", "camellia-192-cfb" },
                { "Camellia256Cfb", "camellia-256-cfb" },
                { "Cast5Cfb", "cast5-cfb" },
                { "Chacha20", "chacha20" },
                { "DesCfb", "des-cfb" },
                { "IdeaCfb", "idea-cfb" },
                { "Rc2Cfb", "rc2-cfb" },
                { "Rc4", "rc4" },
                { "Rc4Md5", "rc4-md5" },
                { "Salsa20", "salsa20" },
                { "Salsa20Ctr", "salsa20-ctr" },
                { "SeedCfb", "seed-cfb" },
                { "Table", "table" }
            };


            public ShadowSocksConfig(string hostname, int port, string password, Method method, string tag = null)
            {
                this.hostname = hostname;
                if (port < 1 || port > 65535)
                    throw new ShadowSocksConfigException("Value of 'port' must be within 0 and 65535.");
                this.port = port;
                this.password = password;
                this.method = method;
                this.methodStr = encryptionTexts[method.ToString()];
                this.tag = tag;    
            }

            public override string ToString()
            {
                var connectionString = $"{methodStr}:{password}@{hostname}:{port}";
                var connectionStringEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(connectionString));
                return $"ss://{connectionStringEncoded}{(!string.IsNullOrEmpty(tag) ? $"#{tag}" : string.Empty)}";
            }

            public enum Method
            {
                Aes128Cfb,
                Aes128Cfb1,
                Aes128Cfb8,
                Aes128Ctr,
                Aes128Ofb,
                Aes192Cfb,
                Aes192Cfb1,
                Aes192Cfb8,
                Aes192Ctr,
                Aes192Ofb,
                Aes256Cb,
                Aes256Cfb1,
                Aes256Cfb8,
                Aes256Ctr,
                Aes256Ofb,
                BfCfb,
                Camellia128Cfb,
                Camellia192Cfb,
                Camellia256Cfb,
                Cast5Cfb,
                Chacha20,
                DesCfb,
                IdeaCfb,
                Rc2Cfb,
                Rc4,
                Rc4Md5,
                Salsa20,
                Salsa20Ctr,
                SeedCfb,
                Table
            }

            public class ShadowSocksConfigException : Exception
            {
                public ShadowSocksConfigException()
                {
                }

                public ShadowSocksConfigException(string message)
                    : base(message)
                {
                }

                public ShadowSocksConfigException(string message, Exception inner)
                    : base(message, inner)
                {
                }
            }
        }


        public class MoneroTransaction
        {
            private readonly string address, txPaymentId, recipientName, txDescription;
            private readonly float? txAmount;
                       

            public MoneroTransaction(string address, float? txAmount = null, string txPaymentId = null, string recipientName = null, string txDescription = null)
            {
                if (string.IsNullOrEmpty(address))
                    throw new MoneroTransactionException("The address is mandatory and has to be set.");
                this.address = address;
                if (txAmount != null && txAmount <= 0)
                    throw new MoneroTransactionException("Value of 'txAmount' must be greater than 0.");
                this.txAmount = txAmount;
                this.txPaymentId = txPaymentId;
                this.recipientName = recipientName;
                this.txDescription = txDescription;
            }

            public override string ToString()
            {
                var moneroUri = $"monero://{address}{(!string.IsNullOrEmpty(txPaymentId) || !string.IsNullOrEmpty(recipientName) || !string.IsNullOrEmpty(txDescription) || txAmount != null ? "?" : string.Empty)}";
                moneroUri += (!string.IsNullOrEmpty(txPaymentId) ? $"tx_payment_id={Uri.EscapeDataString(txPaymentId)}&" : string.Empty);
                moneroUri += (!string.IsNullOrEmpty(recipientName) ? $"recipient_name={Uri.EscapeDataString(recipientName)}&" : string.Empty);
                moneroUri += (txAmount != null ? $"tx_amount={txAmount.ToString().Replace(",",".")}&" : string.Empty);
                moneroUri += (!string.IsNullOrEmpty(txDescription) ? $"tx_description={Uri.EscapeDataString(txDescription)}" : string.Empty);
                return moneroUri.TrimEnd('&');
            }
                        

            public class MoneroTransactionException : Exception
            {
                public MoneroTransactionException()
                {
                }

                public MoneroTransactionException(string message)
                    : base(message)
                {
                }

                public MoneroTransactionException(string message, Exception inner)
                    : base(message, inner)
                {
                }
            }
        }



        private static string ConvertStringToEncoding(string message, string encoding)
        {
            Encoding iso = Encoding.GetEncoding(encoding);
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(message);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
#if NET40
            return iso.GetString(isoBytes);
#else
                return iso.GetString(isoBytes,0, isoBytes.Length);
            #endif
        }

        private static string EscapeInput(string inp, bool simple = false)
        {
            char[] forbiddenChars = {'\\', ';', ',', ':'};
            if (simple)
            {
                forbiddenChars = new char[1] {':'};
            }
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
