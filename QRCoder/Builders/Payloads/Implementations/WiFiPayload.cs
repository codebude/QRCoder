namespace QRCoder.Builders.Payloads.Implementations;

public class WiFiPayload : PayloadBase
{
    private readonly string _ssid;
    private readonly string _password;
    private readonly PayloadGenerator.WiFi.Authentication _authentication;
    private bool _isHiddenSSID;
    private bool _quoteHexStrings;

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

    public WiFiPayload WithQuotedHexStrings()
    {
        _quoteHexStrings = true;
        return this;
    }

    protected override string Value
    {
        get
        {
            var wifi = new PayloadGenerator.WiFi(_ssid, _password, _authentication, _isHiddenSSID, _quoteHexStrings);
            return wifi.ToString();
        }
    }
}
