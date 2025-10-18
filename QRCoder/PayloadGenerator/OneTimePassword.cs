namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a payload for One Time Password (OTP) used in applications like Google Authenticator.
    /// </summary>
    public class OneTimePassword : Payload
    {
        //https://github.com/google/google-authenticator/wiki/Key-Uri-Format

        /// <summary>
        /// The type of OTP (TOTP or HOTP).
        /// </summary>
        public OneTimePasswordAuthType Type { get; set; } = OneTimePasswordAuthType.TOTP;

        /// <summary>
        /// The secret key used for OTP generation.
        /// </summary>
        public string Secret { get; set; } = null!;

        /// <summary>
        /// The hashing algorithm used for OTP (SHA1, SHA256, SHA512).
        /// </summary>
        public OneTimePasswordAuthAlgorithm AuthAlgorithm { get; set; } = OneTimePasswordAuthAlgorithm.SHA1;

        /// <summary>
        /// Obsolete. Use <see cref="AuthAlgorithm"/> instead.
        /// </summary>
        [Obsolete("This property is obsolete, use " + nameof(AuthAlgorithm) + " instead", false)]
        public OoneTimePasswordAuthAlgorithm Algorithm
        {
            get => (OoneTimePasswordAuthAlgorithm)Enum.Parse(typeof(OoneTimePasswordAuthAlgorithm), AuthAlgorithm.ToString());
            set => AuthAlgorithm = (OneTimePasswordAuthAlgorithm)Enum.Parse(typeof(OneTimePasswordAuthAlgorithm), value.ToString());
        }

        /// <summary>
        /// The issuer of the OTP (usually the service or company name).
        /// </summary>
        public string? Issuer { get; set; }

        /// <summary>
        /// The label for the OTP (usually the user's email or username).
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// The number of digits in the OTP (default is 6).
        /// </summary>
        public int Digits { get; set; } = 6;

        /// <summary>
        /// The counter value for HOTP (only used if Type is HOTP).
        /// </summary>
        public int? Counter { get; set; }

        /// <summary>
        /// The period in seconds for TOTP (default is 30).
        /// </summary>
        public int? Period { get; set; } = 30;

        /// <summary>
        /// Enum for OTP types.
        /// </summary>
        public enum OneTimePasswordAuthType
        {
            /// <summary>
            /// Time-based One-Time Password (TOTP)
            /// </summary>
            TOTP,

            /// <summary>
            /// HMAC-based One-Time Password (HOTP)
            /// </summary>
            HOTP,
        }

        /// <summary>
        /// Enum for hashing algorithms used in OTP.
        /// </summary>
        public enum OneTimePasswordAuthAlgorithm
        {
            /// <summary>
            /// SHA-1 hashing algorithm
            /// </summary>
            SHA1,

            /// <summary>
            /// SHA-256 hashing algorithm
            /// </summary>
            SHA256,

            /// <summary>
            /// SHA-512 hashing algorithm
            /// </summary>
            SHA512,
        }

        /// <summary>
        /// Obsolete. Use <see cref="OneTimePasswordAuthAlgorithm"/> instead.
        /// </summary>
        [Obsolete("This enum is obsolete, use " + nameof(OneTimePasswordAuthAlgorithm) + " instead", false)]
        public enum OoneTimePasswordAuthAlgorithm
        {
            /// <summary>
            /// SHA-1 hashing algorithm (Obsolete)
            /// </summary>
            SHA1,

            /// <summary>
            /// SHA-256 hashing algorithm (Obsolete)
            /// </summary>
            SHA256,

            /// <summary>
            /// SHA-512 hashing algorithm (Obsolete)
            /// </summary>
            SHA512,
        }

        /// <summary>
        /// Returns the OTP payload as a string.
        /// </summary>
        /// <returns>The OTP payload as a string.</returns>
        public override string ToString() => Type switch
        {
            OneTimePasswordAuthType.TOTP => TimeToString(),
            OneTimePasswordAuthType.HOTP => HMACToString(),
            _ => throw new ArgumentOutOfRangeException(),
        };

        // Note: Issuer:Label must only contain 1 : if either of the Issuer or the Label has a : then it is invalid.
        // Defaults are 6 digits and 30 for Period

        /// <summary>
        /// Returns the HOTP payload as a string.
        /// </summary>
        /// <returns>The HOTP payload as a string.</returns>
        private string HMACToString()
        {
            var sb = new StringBuilder("otpauth://hotp/");
            ProcessCommonFields(sb);
            var actualCounter = Counter ?? 1;
            sb.Append("&counter=" + actualCounter);
            return sb.ToString();
        }

        /// <summary>
        /// Returns the TOTP payload as a string.
        /// </summary>
        /// <returns>The TOTP payload as a string.</returns>
        private string TimeToString()
        {
            if (Period == null)
            {
                throw new InvalidOperationException("Period must be set when using OneTimePasswordAuthType.TOTP");
            }

            var sb = new StringBuilder("otpauth://totp/");

            ProcessCommonFields(sb);

            if (Period != 30)
            {
                sb.Append("&period=" + Period);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Processes the common fields for both TOTP and HOTP.
        /// </summary>
        /// <param name="sb">StringBuilder to append the common fields.</param>
        private void ProcessCommonFields(StringBuilder sb)
        {
            if (string.IsNullOrWhiteSpace(Secret))
            {
                throw new InvalidOperationException("Secret must be a filled out base32 encoded string");
            }
            string strippedSecret = Secret.Replace(" ", "");
            string? escapedIssuer = null;
            string? escapedLabel = null;
            string? label = null;

            if (!string.IsNullOrWhiteSpace(Issuer))
            {
                if (Issuer!.Contains(':'))
                {
                    throw new InvalidOperationException("Issuer must not have a ':'");
                }
                escapedIssuer = Uri.EscapeDataString(Issuer);
            }

            if (!string.IsNullOrWhiteSpace(Label))
            {
                if (Label!.Contains(':'))
                {
                    throw new InvalidOperationException("Label must not have a ':'");
                }
                escapedLabel = Uri.EscapeDataString(Label);
            }

            if (escapedLabel != null && escapedIssuer != null)
            {
                label = escapedIssuer + ":" + escapedLabel;
            }
            else if (escapedIssuer != null)
            {
                label = escapedIssuer;
            }

            if (label != null)
            {
                sb.Append(label);
            }

            sb.Append("?secret=" + strippedSecret);

            if (escapedIssuer != null)
            {
                sb.Append("&issuer=" + escapedIssuer);
            }

            if (AuthAlgorithm != OneTimePasswordAuthAlgorithm.SHA1)
            {
                sb.Append("&algorithm=" + AuthAlgorithm.ToString());
            }

            if (Digits != 6)
            {
                sb.Append("&digits=" + Digits);
            }
        }
    }
}
