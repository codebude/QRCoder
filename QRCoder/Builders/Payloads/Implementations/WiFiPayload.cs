namespace QRCoder.Builders.Payloads.Implementations
{
    public class WiFiPayload : PayloadBase
    {
        private string _ssid { get; set; }
        private string _password { get; set; }
        private PayloadGenerator.WiFi.Authentication _authentication { get; set; }
        private bool _isHiddenSSID { get; set; }
        private bool _isHexStrings { get; set; }

        public WiFiPayload(string ssid)
        {
            _ssid = ssid;
            _password = "";
            _authentication = PayloadGenerator.WiFi.Authentication.nopass;
        }

        public WiFiPayload(string ssid, string password, PayloadGenerator.WiFi.Authentication authentication)
        {
            _ssid = ssid;
            _password = password;
            _authentication = authentication;
        }

        public WiFiPayload WithHiddenSSID()
        {
            _isHiddenSSID = true;
            return this;
        }

        public WiFiPayload WithHexStrings()
        {
            _isHexStrings = true;
            return this;
        }

        protected override string Value
        {
            get
            {
                var wifi = new PayloadGenerator.WiFi(_ssid, _password, _authentication, _isHiddenSSID, _isHexStrings);
                return wifi.ToString();
            }
        }
    }
}
