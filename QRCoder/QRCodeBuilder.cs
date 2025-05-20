using QRCoder.Builders.Payloads.Implementations;

namespace QRCoder
{
    public static class QRCodeBuilder
    {
        public static StringPayload CreateUrl(string url)
        {
            return new StringPayload(url);
        }

        public static StringPayload CreateSms(string number, PayloadGenerator.SMS.SMSEncoding encoding = PayloadGenerator.SMS.SMSEncoding.SMS)
        {
            return new StringPayload(new PayloadGenerator.SMS(number, encoding).ToString());
        }

        public static EmailPayload CreateEmail(string address)
        {
            return new EmailPayload(address);
        }

        public static WiFiPayload CreateWiFi(string ssid)
        {
            return new WiFiPayload(ssid);
        }

        public static WiFiPayload CreateWiFi(string ssid, string password, PayloadGenerator.WiFi.Authentication authentication)
        {
            return new WiFiPayload(ssid, password, authentication);
        }

        public static StringPayload CreateBookmark(string url, string title)
        {
            return new StringPayload(new PayloadGenerator.Bookmark(url, title).ToString());
        }

        public static StringPayload CreateMMS(string number, PayloadGenerator.MMS.MMSEncoding encoding = PayloadGenerator.MMS.MMSEncoding.MMS)
        {
            return new StringPayload(new PayloadGenerator.MMS(number, encoding).ToString());
        }

        public static StringPayload CreateMMS(string number, string subject, PayloadGenerator.MMS.MMSEncoding encoding = PayloadGenerator.MMS.MMSEncoding.MMS)
        {
            return new StringPayload(new PayloadGenerator.MMS(number, subject, encoding).ToString());
        }

        public static StringPayload CreatePhoneNumber(string number)
        {
            return new StringPayload("tel:" + number);
        }
    }
}
