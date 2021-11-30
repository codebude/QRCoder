namespace QRCoder2.Payloads
{
    public class WiFiPayload : PayloadBase
    {
        private readonly string ssid, password, authenticationMode;
        private readonly bool isHiddenSsid;

        /// <summary>
        /// Generates a WiFi payload. Scanned by a QR Code scanner app, the device will connect to the WiFi.
        /// </summary>
        /// <param name="ssid">SSID of the WiFi network</param>
        /// <param name="password">Password of the WiFi network</param>
        /// <param name="authenticationMode">Authentication mode (WEP, WPA, WPA2)</param>
        /// <param name="isHiddenSSID">Set flag, if the WiFi network hides its SSID</param>
        public WiFiPayload(string ssid, string password, Authentication authenticationMode, bool isHiddenSSID = false)
        {
            this.ssid = EscapeInput(ssid);
            this.ssid = IsHexStyle(this.ssid) ? "\"" + this.ssid + "\"" : this.ssid;
            this.password = EscapeInput(password);
            this.password = IsHexStyle(this.password) ? "\"" + this.password + "\"" : this.password;
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
}