namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a WiFi payload. Scanned by a QR Code scanner app, the device will connect to the WiFi.
    /// </summary>
    public class WiFi : Payload
    {
        private readonly string _ssid, _password, _authenticationMode;
        private readonly bool _isHiddenSsid;

        /// <summary>
        /// Initializes a new instance of the <see cref="WiFi"/> class.
        /// </summary>
        /// <param name="ssid">SSID of the WiFi network</param>
        /// <param name="password">Password of the WiFi network</param>
        /// <param name="authenticationMode">Authentication mode (WEP, WPA, WPA2)</param>
        /// <param name="isHiddenSSID">Set flag if the WiFi network hides its SSID</param>
        /// <param name="escapeHexStrings">Set flag if ssid/password is delivered as HEX string. Note: May not be supported on iOS devices.</param>
        public WiFi(string ssid, string password, Authentication authenticationMode, bool isHiddenSSID = false, bool escapeHexStrings = true)
        {
            this._ssid = EscapeInput(ssid);
            this._ssid = escapeHexStrings && isHexStyle(this._ssid) ? "\"" + this._ssid + "\"" : this._ssid;
            this._password = EscapeInput(password);
            this._password = escapeHexStrings && isHexStyle(this._password) ? "\"" + this._password + "\"" : this._password;
            this._authenticationMode = authenticationMode.ToString();
            _isHiddenSsid = isHiddenSSID;
        }

        /// <summary>
        /// Returns the WiFi payload as a string.
        /// </summary>
        public override string ToString()
        {
            return
                $"WIFI:T:{_authenticationMode};S:{_ssid};P:{_password};{(_isHiddenSsid ? "H:true" : string.Empty)};";
        }

        /// <summary>
        /// Specifies the authentication mode for the WiFi network.
        /// </summary>
        public enum Authentication
        {
            /// <summary>
            /// WEP authentication mode
            /// </summary>
            WEP,
            /// <summary>
            /// WPA authentication mode
            /// </summary>
            WPA,
            /// <summary>
            /// No password authentication mode
            /// </summary>
            nopass,
            /// <summary>
            /// WPA2 authentication mode
            /// </summary>
            WPA2
        }
    }
}
