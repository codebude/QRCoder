using System;
using System.Text;

namespace QRCoder2.Payloads
{
    public class OneTimePasswordPayload : PayloadBase
    {
        //https://github.com/google/google-authenticator/wiki/Key-Uri-Format
        public OneTimePasswordAuthType Type { get; set; } = OneTimePasswordAuthType.TOTP;
        public string Secret { get; set; }

        public OneTimePasswordAuthAlgorithm AuthAlgorithm { get; set; } = OneTimePasswordAuthAlgorithm.SHA1;

        [Obsolete("This property is obsolete, use " + nameof(AuthAlgorithm) + " instead", false)]
        public OoneTimePasswordAuthAlgorithm Algorithm
        {
            get { return (OoneTimePasswordAuthAlgorithm)Enum.Parse(typeof(OoneTimePasswordAuthAlgorithm), AuthAlgorithm.ToString()); }
            set { AuthAlgorithm = (OneTimePasswordAuthAlgorithm)Enum.Parse(typeof(OneTimePasswordAuthAlgorithm), value.ToString()); }
        }

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

        public enum OneTimePasswordAuthAlgorithm
        {
            SHA1,
            SHA256,
            SHA512,
        }

        [Obsolete("This enum is obsolete, use " + nameof(OneTimePasswordAuthAlgorithm) + " instead", false)]
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
            if (string.IsNullOrWhiteSpace(Secret))
            {
                throw new Exception("Secret must be a filled out base32 encoded string");
            }
            string strippedSecret = Secret.Replace(" ", "");
            string escapedIssuer = null;
            string escapedLabel = null;

            if (!string.IsNullOrWhiteSpace(Issuer))
            {
                if (Issuer.Contains(":"))
                {
                    throw new Exception("Issuer must not have a ':'");
                }
                escapedIssuer = Uri.EscapeUriString(Issuer);
            }

            if (!string.IsNullOrWhiteSpace(Label))
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
}