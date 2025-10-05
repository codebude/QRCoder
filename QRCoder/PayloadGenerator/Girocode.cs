namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates the payload for a Girocode (QR-Code with credit transfer information).
    /// </summary>
    public class Girocode : Payload
    {
        //Keep in mind, that the ECC level has to be set to "M" when generating a Girocode!
        //Girocode specification: http://www.europeanpaymentscouncil.eu/index.cfm/knowledge-bank/epc-documents/quick-response-code-guidelines-to-enable-data-capture-for-the-initiation-of-a-sepa-credit-transfer/epc069-12-quick-response-code-guidelines-to-enable-data-capture-for-the-initiation-of-a-sepa-credit-transfer1/

        private readonly string _br = "\n";
        private readonly string _iban, _bic, _name, _purposeOfCreditTransfer, _remittanceInformation, _messageToGirocodeUser;
        private readonly decimal _amount;
        private readonly GirocodeVersion _version;
        private readonly GirocodeEncoding _encoding;
        private readonly TypeOfRemittance _typeOfRemittance;

        /// <summary>
        /// Gets the ECC level required for Girocode, which is always set to M.
        /// </summary>
        public override QRCodeGenerator.ECCLevel EccLevel => QRCodeGenerator.ECCLevel.M;

        /// <summary>
        /// Initializes a new instance of the <see cref="Girocode"/> class, which contains a payload for a Girocode (QR-Code with credit transfer information).
        /// </summary>
        /// <param name="iban">Account number of the Beneficiary. Only IBAN is allowed.</param>
        /// <param name="bic">BIC of the Beneficiary Bank.</param>
        /// <param name="name">Name of the Beneficiary.</param>
        /// <param name="amount">Amount of the Credit Transfer in Euro. (Amount must be more than 0.01 and less than 999999999.99)</param>
        /// <param name="remittanceInformation">Remittance Information (Purpose-/reference text). (optional)</param>
        /// <param name="typeOfRemittance">Type of remittance information. Either structured (e.g. ISO 11649 RF Creditor Reference) and max. 35 chars or unstructured and max. 140 chars.</param>
        /// <param name="purposeOfCreditTransfer">Purpose of the Credit Transfer (optional)</param>
        /// <param name="messageToGirocodeUser">Beneficiary to originator information. (optional)</param>
        /// <param name="version">Girocode version. Either 001 or 002. Default: 001.</param>
        /// <param name="encoding">Encoding of the Girocode payload. Default: ISO-8859-1</param>
        /// <exception cref="GirocodeException">Thrown when the input values are not valid according to the Girocode specification.</exception>
        public Girocode(string iban, string bic, string name, decimal amount, string remittanceInformation = "", TypeOfRemittance typeOfRemittance = TypeOfRemittance.Unstructured, string purposeOfCreditTransfer = "", string messageToGirocodeUser = "", GirocodeVersion version = GirocodeVersion.Version1, GirocodeEncoding encoding = GirocodeEncoding.ISO_8859_1)
        {
            _version = version;
            _encoding = encoding;
            if (!IsValidIban(iban))
                throw new GirocodeException("The IBAN entered isn't valid.");
            _iban = iban.Replace(" ", "").ToUpper();
            if (!IsValidBic(bic, _version == GirocodeVersion.Version1))
                throw new GirocodeException("The BIC entered isn't valid.");
            _bic = bic?.Replace(" ", "").ToUpper() ?? string.Empty;
            if (name.Length > 70)
                throw new GirocodeException("(Payee-)Name must be shorter than 71 chars.");
            _name = name;
            if (amount.ToString().Replace(",", ".").Contains(".") && amount.ToString().Replace(",", ".").Split('.')[1].TrimEnd('0').Length > 2)
                throw new GirocodeException("Amount must have less than 3 digits after decimal point.");
            if (amount < 0.01m || amount > 999999999.99m)
                throw new GirocodeException("Amount has to be at least 0.01 and must be smaller or equal to 999999999.99.");
            _amount = amount;
            if (purposeOfCreditTransfer.Length > 4)
                throw new GirocodeException("Purpose of credit transfer can only have 4 chars at maximum.");
            _purposeOfCreditTransfer = purposeOfCreditTransfer;
            if (typeOfRemittance == TypeOfRemittance.Unstructured && remittanceInformation.Length > 140)
                throw new GirocodeException("Unstructured reference texts have to be shorter than 141 chars.");
            if (typeOfRemittance == TypeOfRemittance.Structured && remittanceInformation.Length > 35)
                throw new GirocodeException("Structured reference texts have to be shorter than 36 chars.");
            _typeOfRemittance = typeOfRemittance;
            _remittanceInformation = remittanceInformation;
            if (messageToGirocodeUser.Length > 70)
                throw new GirocodeException("Message to the Girocode-User reader texts have to be shorter than 71 chars.");
            _messageToGirocodeUser = messageToGirocodeUser;
        }

        /// <summary>
        /// Returns the Girocode payload as a string.
        /// </summary>
        /// <returns>The Girocode payload as a string.</returns>
        public override string ToString()
        {
            var girocodePayload = "BCD" + _br;
            girocodePayload += ((_version == GirocodeVersion.Version1) ? "001" : "002") + _br;
            girocodePayload += (int)_encoding + 1 + _br;
            girocodePayload += "SCT" + _br;
            girocodePayload += _bic + _br;
            girocodePayload += _name + _br;
            girocodePayload += _iban + _br;
            girocodePayload += $"EUR{_amount:0.00}".Replace(",", ".") + _br;
            girocodePayload += _purposeOfCreditTransfer + _br;
            girocodePayload += ((_typeOfRemittance == TypeOfRemittance.Structured)
                ? _remittanceInformation
                : string.Empty) + _br;
            girocodePayload += ((_typeOfRemittance == TypeOfRemittance.Unstructured)
                ? _remittanceInformation
                : string.Empty) + _br;
            girocodePayload += _messageToGirocodeUser;

            return ConvertStringToEncoding(girocodePayload, _encoding.ToString().Replace("_", "-"));
        }

        /// <summary>
        /// Defines the Girocode version.
        /// </summary>
        public enum GirocodeVersion
        {
            /// <summary>
            /// Girocode version 1.
            /// </summary>
            Version1,

            /// <summary>
            /// Girocode version 2.
            /// </summary>
            Version2
        }

        /// <summary>
        /// Defines the type of remittance information.
        /// </summary>
        public enum TypeOfRemittance
        {
            /// <summary>
            /// Structured remittance information.
            /// </summary>
            Structured,

            /// <summary>
            /// Unstructured remittance information.
            /// </summary>
            Unstructured
        }

        /// <summary>
        /// Defines the encoding types for Girocode payloads.
        /// </summary>
        public enum GirocodeEncoding
        {
            /// <summary>
            /// UTF-8 encoding.
            /// </summary>
            UTF_8,

            /// <summary>
            /// ISO-8859-1 encoding.
            /// </summary>
            ISO_8859_1,

            /// <summary>
            /// ISO-8859-2 encoding.
            /// </summary>
            ISO_8859_2,

            /// <summary>
            /// ISO-8859-4 encoding.
            /// </summary>
            ISO_8859_4,

            /// <summary>
            /// ISO-8859-5 encoding.
            /// </summary>
            ISO_8859_5,

            /// <summary>
            /// ISO-8859-7 encoding.
            /// </summary>
            ISO_8859_7,

            /// <summary>
            /// ISO-8859-10 encoding.
            /// </summary>
            ISO_8859_10,

            /// <summary>
            /// ISO-8859-15 encoding.
            /// </summary>
            ISO_8859_15
        }

        /// <summary>
        /// Represents errors that occur during Girocode generation.
        /// </summary>
        public class GirocodeException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GirocodeException"/> class.
            /// </summary>
            public GirocodeException()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="GirocodeException"/> class with a specified error message.
            /// </summary>
            /// <param name="message">The message that describes the error.</param>
            public GirocodeException(string message)
                : base(message)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="GirocodeException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
            /// </summary>
            /// <param name="message">The error message that explains the reason for the exception.</param>
            /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
            public GirocodeException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }
    }
}
