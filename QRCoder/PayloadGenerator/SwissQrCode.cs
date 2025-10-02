using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates the payload for a SwissQrCode.
    /// </summary>
    public class SwissQrCode : Payload
    {
        //Keep in mind, that the ECC level has to be set to "M" when generating a SwissQrCode!
        //SwissQrCode specification: 
        //    - (de) https://www.paymentstandards.ch/dam/downloads/ig-qr-bill-de.pdf
        //    - (en) https://www.paymentstandards.ch/dam/downloads/ig-qr-bill-en.pdf
        //Changes between version 1.0 and 2.0: https://www.paymentstandards.ch/dam/downloads/change-documentation-qrr-de.pdf

        private readonly string _br = "\r\n";
        private readonly string? _alternativeProcedure1, _alternativeProcedure2;
        private readonly Iban _iban;
        private readonly decimal? _amount;
        private readonly Contact _creditor;
        private readonly Contact? _ultimateCreditor, _debitor;
        private readonly Currency _currency;
        private readonly DateTime? _requestedDateOfPayment;
        private readonly Reference _reference;
        private readonly AdditionalInformation _additionalInformation;

        /// <inheritdoc/>
        public override QRCodeGenerator.ECCLevel EccLevel => QRCodeGenerator.ECCLevel.M;
        /// <inheritdoc/>
        public override QRCodeGenerator.EciMode EciMode => QRCodeGenerator.EciMode.Utf8;

        /// <summary>
        /// Generates the payload for a SwissQrCode v2.0. (Don't forget to use ECC-Level=M, EncodingMode=UTF-8 and to set the Swiss flag icon to the final QR code.)
        /// </summary>
        /// <param name="iban">IBAN object</param>
        /// <param name="currency">Currency (either EUR or CHF)</param>
        /// <param name="creditor">Creditor (payee) information</param>
        /// <param name="reference">Reference information</param>
        /// <param name="debitor">Debitor (payer) information</param>
        /// <param name="amount">Amount</param>
        /// <param name="requestedDateOfPayment">Requested date of debitor's payment</param>
        /// <param name="ultimateCreditor">Ultimate creditor information (use only in consultation with your bank - for future use only!)</param>
        /// <param name="alternativeProcedure1">Optional command for alternative processing mode - line 1</param>
        /// <param name="alternativeProcedure2">Optional command for alternative processing mode - line 2</param>
        public SwissQrCode(Iban iban, Currency currency, Contact creditor, Reference reference, AdditionalInformation? additionalInformation = null, Contact? debitor = null, decimal? amount = null, DateTime? requestedDateOfPayment = null, Contact? ultimateCreditor = null, string? alternativeProcedure1 = null, string? alternativeProcedure2 = null)
        {
            _iban = iban;

            _creditor = creditor;
            _ultimateCreditor = ultimateCreditor;

            _additionalInformation = additionalInformation ?? new AdditionalInformation();

            if (amount != null && amount.ToString()!.Length > 12)
                throw new SwissQrCodeException("Amount (including decimals) must be shorter than 13 places.");
            _amount = amount;

            _currency = currency;
            _requestedDateOfPayment = requestedDateOfPayment;
            _debitor = debitor;

            if (iban.IsQrIban && reference.RefType != Reference.ReferenceType.QRR)
                throw new SwissQrCodeException("If QR-IBAN is used, you have to choose \"QRR\" as reference type!");
            if (!iban.IsQrIban && reference.RefType == Reference.ReferenceType.QRR)
                throw new SwissQrCodeException("If non QR-IBAN is used, you have to choose either \"SCOR\" or \"NON\" as reference type!");
            _reference = reference;

            if (alternativeProcedure1 != null && alternativeProcedure1.Length > 100)
                throw new SwissQrCodeException("Alternative procedure information block 1 must be shorter than 101 chars.");
            _alternativeProcedure1 = alternativeProcedure1;
            if (alternativeProcedure2 != null && alternativeProcedure2.Length > 100)
                throw new SwissQrCodeException("Alternative procedure information block 2 must be shorter than 101 chars.");
            _alternativeProcedure2 = alternativeProcedure2;
        }

        /// <summary>
        /// Represents additional information for the SwissQrCode.
        /// </summary>
        public class AdditionalInformation
        {
            private readonly string? _unstructuredMessage, _billInformation;

            /// <summary>
            /// Creates an additional information object. Both parameters are optional and must be shorter than 141 chars in combination.
            /// </summary>
            /// <param name="unstructuredMessage">Unstructured text message</param>
            /// <param name="billInformation">Bill information</param>
            public AdditionalInformation(string? unstructuredMessage = null, string? billInformation = null)
            {
                if (((unstructuredMessage != null ? unstructuredMessage.Length : 0) + (billInformation != null ? billInformation.Length : 0)) > 140)
                    throw new SwissQrCodeAdditionalInformationException("Unstructured message and bill information must be shorter than 141 chars in total/combined.");
                _unstructuredMessage = unstructuredMessage;
                _billInformation = billInformation;
                Trailer = "EPD";
            }

            /// <summary>
            /// Gets the unstructured message.
            /// </summary>
            public string? UnstructureMessage => !string.IsNullOrEmpty(_unstructuredMessage) ? _unstructuredMessage!.Replace("\n", "") : null;

            /// <summary>
            /// Gets the bill information.
            /// </summary>
            public string? BillInformation => !string.IsNullOrEmpty(_billInformation) ? _billInformation!.Replace("\n", "") : null;

            /// <summary>
            /// Gets the trailer.
            /// </summary>
            public string Trailer { get; }


            /// <summary>
            /// Represents exceptions specific to SwissQrCode additional information.
            /// </summary>
            public class SwissQrCodeAdditionalInformationException : Exception
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="SwissQrCodeAdditionalInformationException"/> class.
                /// </summary>
                public SwissQrCodeAdditionalInformationException()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="SwissQrCodeAdditionalInformationException"/> class with a specified error message.
                /// </summary>
                /// <param name="message">The message that describes the error.</param>
                public SwissQrCodeAdditionalInformationException(string message)
                    : base(message)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="SwissQrCodeAdditionalInformationException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name="message">The error message that explains the reason for the exception.</param>
                /// <param name="inner">The exception that is the cause of the current exception.</param>
                public SwissQrCodeAdditionalInformationException(string message, Exception inner)
                    : base(message, inner)
                {
                }
            }
        }

        /// <summary>
        /// Represents reference information for the SwissQrCode.
        /// </summary>
        public class Reference
        {
            private readonly string? _reference;
            private readonly ReferenceTextType? _referenceTextType;

            /// <summary>
            /// Creates a reference object which must be passed to the SwissQrCode instance
            /// </summary>
            /// <param name="referenceType">Type of the reference (QRR, SCOR or NON)</param>
            /// <param name="reference">Reference text</param>
            /// <param name="referenceTextType">Type of the reference text (QR-reference or Creditor Reference)</param>                
            public Reference(ReferenceType referenceType, string? reference = null, ReferenceTextType? referenceTextType = null)
            {
                RefType = referenceType;
                _referenceTextType = referenceTextType;

                if (referenceType == ReferenceType.NON && reference != null)
                    throw new SwissQrCodeReferenceException("Reference is only allowed when referenceType not equals \"NON\"");
                if (referenceType != ReferenceType.NON && reference != null && referenceTextType == null)
                    throw new SwissQrCodeReferenceException("You have to set an ReferenceTextType when using the reference text.");
                if (referenceTextType == ReferenceTextType.QrReference && reference != null && (reference.Length > 27))
                    throw new SwissQrCodeReferenceException("QR-references have to be shorter than 28 chars.");
                if (referenceTextType == ReferenceTextType.QrReference && reference != null && !Regex.IsMatch(reference, @"^[0-9]+$"))
                    throw new SwissQrCodeReferenceException("QR-reference must exist out of digits only.");
                if (referenceTextType == ReferenceTextType.QrReference && reference != null && !ChecksumMod10(reference))
                    throw new SwissQrCodeReferenceException("QR-references is invalid. Checksum error.");
                if (referenceTextType == ReferenceTextType.CreditorReferenceIso11649 && reference != null && (reference.Length > 25))
                    throw new SwissQrCodeReferenceException("Creditor references (ISO 11649) have to be shorter than 26 chars.");

                _reference = reference;
            }

            /// <summary>
            /// Gets the reference type.
            /// </summary>
            public ReferenceType RefType { get; }

            /// <summary>
            /// Gets the reference text.
            /// </summary>
            public string? ReferenceText => !string.IsNullOrEmpty(_reference) ? _reference!.Replace("\n", "") : null;

            /// <summary>
            /// Reference type. When using a QR-IBAN you have to use either "QRR" or "SCOR".
            /// </summary>
            public enum ReferenceType
            {
                /// <summary>
                /// QR Reference
                /// </summary>
                QRR,
                /// <summary>
                /// Creditor Reference
                /// </summary>
                SCOR,
                /// <summary>
                /// No Reference
                /// </summary>
                NON
            }

            /// <summary>
            /// Represents the text type for the reference.
            /// </summary>
            public enum ReferenceTextType
            {
                /// <summary>
                /// QR Reference Text
                /// </summary>
                QrReference,
                /// <summary>
                /// Creditor Reference ISO 11649
                /// </summary>
                CreditorReferenceIso11649
            }

            /// <summary>
            /// Represents exceptions specific to SwissQrCode references.
            /// </summary>
            public class SwissQrCodeReferenceException : Exception
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="SwissQrCodeReferenceException"/> class.
                /// </summary>
                public SwissQrCodeReferenceException()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="SwissQrCodeReferenceException"/> class with a specified error message.
                /// </summary>
                /// <param name="message">The message that describes the error.</param>
                public SwissQrCodeReferenceException(string message)
                    : base(message)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="SwissQrCodeReferenceException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name="message">The error message that explains the reason for the exception.</param>
                /// <param name="inner">The exception that is the cause of the current exception.</param>
                public SwissQrCodeReferenceException(string message, Exception inner)
                    : base(message, inner)
                {
                }
            }
        }

        /// <summary>
        /// Represents an IBAN with type information.
        /// </summary>
        public class Iban
        {
            private readonly string _iban;
            private readonly IbanType _ibanType;

            /// <summary>
            /// IBAN object with type information
            /// </summary>
            /// <param name="iban">IBAN</param>
            /// <param name="ibanType">Type of IBAN (normal or QR-IBAN)</param>
            public Iban(string iban, IbanType ibanType)
            {
                if (ibanType == IbanType.Iban && !IsValidIban(iban))
                    throw new SwissQrCodeIbanException("The IBAN entered isn't valid.");
                if (ibanType == IbanType.QrIban && !IsValidQRIban(iban))
                    throw new SwissQrCodeIbanException("The QR-IBAN entered isn't valid.");
                if (!iban.StartsWith("CH") && !iban.StartsWith("LI"))
                    throw new SwissQrCodeIbanException("The IBAN must start with \"CH\" or \"LI\".");
                _iban = iban;
                _ibanType = ibanType;
            }

            /// <summary>
            /// Gets a value indicating whether this is a QR-IBAN.
            /// </summary>
            public bool IsQrIban => _ibanType == IbanType.QrIban;

            /// <summary>
            /// Converts the IBAN object to its string representation.
            /// </summary>
            /// <returns>A string representation of the IBAN.</returns>
            public override string ToString()
                => _iban.Replace("-", "").Replace("\n", "").Replace(" ", "");

            /// <summary>
            /// Represents the type of IBAN.
            /// </summary>
            public enum IbanType
            {
                /// <summary>
                /// Regular IBAN
                /// </summary>
                Iban,
                /// <summary>
                /// QR-IBAN
                /// </summary>
                QrIban
            }

            /// <summary>
            /// Represents exceptions specific to SwissQrCode IBANs.
            /// </summary>
            public class SwissQrCodeIbanException : Exception
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="SwissQrCodeIbanException"/> class.
                /// </summary>
                public SwissQrCodeIbanException()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="SwissQrCodeIbanException"/> class with a specified error message.
                /// </summary>
                /// <param name="message">The message that describes the error.</param>
                public SwissQrCodeIbanException(string message)
                    : base(message)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="SwissQrCodeIbanException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name="message">The error message that explains the reason for the exception.</param>
                /// <param name="inner">The exception that is the cause of the current exception.</param>
                public SwissQrCodeIbanException(string message, Exception inner)
                    : base(message, inner)
                {
                }
            }
        }

        /// <summary>
        /// Represents contact information.
        /// </summary>
        public class Contact
        {
            private static readonly HashSet<string> _twoLetterCodes = ValidTwoLetterCodes();
            private readonly string _br = "\r\n";
            private readonly string _name, _zipCode, _city, _country;
            private readonly string? _streetOrAddressline1, _houseNumberOrAddressline2;
            private readonly AddressType _adrType;

            /// <summary>
            /// Contact type. Can be used for payee, ultimate payee, etc. with address in structured mode (S).
            /// </summary>
            /// <param name="name">Last name or company (optional first name)</param>
            /// <param name="zipCode">Zip-/Postcode</param>
            /// <param name="city">City name</param>
            /// <param name="country">Two-letter country code as defined in ISO 3166-1</param>
            /// <param name="street">Streetname without house number</param>
            /// <param name="houseNumber">House number</param>
            [Obsolete("This constructor is deprecated. Use WithStructuredAddress instead.")]
            public Contact(string name, string zipCode, string city, string country, string? street = null, string? houseNumber = null) : this(name, zipCode, city, country, street, houseNumber, AddressType.StructuredAddress)
            {
            }


            /// <summary>
            /// Contact type. Can be used for payee, ultimate payee, etc. with address in combined mode (K).
            /// </summary>
            /// <param name="name">Last name or company (optional first name)</param>
            /// <param name="country">Two-letter country code as defined in ISO 3166-1</param>
            /// <param name="addressLine1">Adress line 1</param>
            /// <param name="addressLine2">Adress line 2</param>
            [Obsolete("This constructor is deprecated. Use WithCombinedAddress instead.")]
            public Contact(string name, string country, string addressLine1, string addressLine2) : this(name, null, null, country, addressLine1, addressLine2, AddressType.CombinedAddress)
            {
            }

            /// <summary>
            /// Creates a contact with structured address.
            /// </summary>
            public static Contact WithStructuredAddress(string name, string zipCode, string city, string country, string? street = null, string? houseNumber = null)
                => new Contact(name, zipCode, city, country, street, houseNumber, AddressType.StructuredAddress);

            /// <summary>
            /// Creates a contact with combined address.
            /// </summary>
            public static Contact WithCombinedAddress(string name, string country, string addressLine1, string addressLine2)
                => new Contact(name, null, null, country, addressLine1, addressLine2, AddressType.CombinedAddress);


            private Contact(string name, string? zipCode, string? city, string country, string? streetOrAddressline1, string? houseNumberOrAddressline2, AddressType addressType)
            {
                //Pattern extracted from https://qr-validation.iso-payments.ch as explained in https://github.com/Shane32/QRCoder/issues/97
                var charsetPattern = @"^([a-zA-Z0-9\.,;:'\ \+\-/\(\)?\*\[\]\{\}\\`´~ ^|]|[!""#%&<>÷=@_$£¡¢¤¥¦§¨©ª«¬®¯°±²³µ¶·¸¹º»¼½¾¿×Ø€]|[àáâäãåāăąçćĉċčďđèéêëēĕėęěĝğġģĥħìíîïĩīĭįıĳķĸĺļľŀłñńņňŉŋòóôöōŏőõŕŗřśŝşšșţťŧțùúûüũūŭůűųŵýÿŷźżžßÀÁÂÄÃÅĀĂĄÇĆĈĊČĎĐÈÉÊËĒĔĖĘĚĜĞĠĢĤĦÌÍÎÏĨĪĬĮİĲĴĵĶĹĻĽĿŁÑŃŅŇŊÒÓÔÖÕŌŎŐŔŖŘŚŜŞŠȘŢŤŦȚÙÚÛÜŨŪŬŮŰŲŴÝŶŸŹŻŽÆÐÞæðøþŒœſ])*$";

                _adrType = addressType;

                if (string.IsNullOrEmpty(name))
                    throw new SwissQrCodeContactException("Name must not be empty.");
                if (name.Length > 70)
                    throw new SwissQrCodeContactException("Name must be shorter than 71 chars.");
                if (!Regex.IsMatch(name, charsetPattern))
                    throw new SwissQrCodeContactException($"Name must match the following pattern as defined in pain.001: {charsetPattern}");
                _name = name;

                if (AddressType.StructuredAddress == _adrType)
                {
                    if (!string.IsNullOrEmpty(streetOrAddressline1) && (streetOrAddressline1!.Length > 70))
                        throw new SwissQrCodeContactException("Street must be shorter than 71 chars.");
                    if (!string.IsNullOrEmpty(streetOrAddressline1) && !Regex.IsMatch(streetOrAddressline1, charsetPattern))
                        throw new SwissQrCodeContactException($"Street must match the following pattern as defined in pain.001: {charsetPattern}");
                    _streetOrAddressline1 = streetOrAddressline1;

                    if (!string.IsNullOrEmpty(houseNumberOrAddressline2) && houseNumberOrAddressline2!.Length > 16)
                        throw new SwissQrCodeContactException("House number must be shorter than 17 chars.");
                    _houseNumberOrAddressline2 = houseNumberOrAddressline2;
                }
                else
                {
                    if (!string.IsNullOrEmpty(streetOrAddressline1) && (streetOrAddressline1!.Length > 70))
                        throw new SwissQrCodeContactException("Address line 1 must be shorter than 71 chars.");
                    if (!string.IsNullOrEmpty(streetOrAddressline1) && !Regex.IsMatch(streetOrAddressline1, charsetPattern))
                        throw new SwissQrCodeContactException($"Address line 1 must match the following pattern as defined in pain.001: {charsetPattern}");
                    _streetOrAddressline1 = streetOrAddressline1;

                    if (string.IsNullOrEmpty(houseNumberOrAddressline2))
                        throw new SwissQrCodeContactException("Address line 2 must be provided for combined addresses (address line-based addresses).");
                    if (!string.IsNullOrEmpty(houseNumberOrAddressline2) && (houseNumberOrAddressline2!.Length > 70))
                        throw new SwissQrCodeContactException("Address line 2 must be shorter than 71 chars.");
                    if (!string.IsNullOrEmpty(houseNumberOrAddressline2) && !Regex.IsMatch(houseNumberOrAddressline2, charsetPattern))
                        throw new SwissQrCodeContactException($"Address line 2 must match the following pattern as defined in pain.001: {charsetPattern}");
                    _houseNumberOrAddressline2 = houseNumberOrAddressline2;
                }

                if (AddressType.StructuredAddress == _adrType)
                {
                    if (string.IsNullOrEmpty(zipCode))
                        throw new SwissQrCodeContactException("Zip code must not be empty.");
                    if (zipCode!.Length > 16)
                        throw new SwissQrCodeContactException("Zip code must be shorter than 17 chars.");
                    if (!Regex.IsMatch(zipCode, charsetPattern))
                        throw new SwissQrCodeContactException($"Zip code must match the following pattern as defined in pain.001: {charsetPattern}");
                    _zipCode = zipCode;

                    if (string.IsNullOrEmpty(city))
                        throw new SwissQrCodeContactException("City must not be empty.");
                    if (city!.Length > 35)
                        throw new SwissQrCodeContactException("City name must be shorter than 36 chars.");
                    if (!Regex.IsMatch(city, charsetPattern))
                        throw new SwissQrCodeContactException($"City name must match the following pattern as defined in pain.001: {charsetPattern}");
                    _city = city;
                }
                else
                {
                    _zipCode = _city = string.Empty;
                }

                if (!IsValidTwoLetterCode(country))
                    throw new SwissQrCodeContactException("Country must be a valid \"two letter\" country code as defined by ISO 3166-1, but it isn't.");

                _country = country;
            }

            private static bool IsValidTwoLetterCode(string code) => _twoLetterCodes.Contains(code);

            private static HashSet<string> ValidTwoLetterCodes()
            {
                string[] codes = new string[] { "AF", "AL", "DZ", "AS", "AD", "AO", "AI", "AQ", "AG", "AR", "AM", "AW", "AU", "AT", "AZ", "BS", "BH", "BD", "BB", "BY", "BE", "BZ", "BJ", "BM", "BT", "BO", "BQ", "BA", "BW", "BV", "BR", "IO", "BN", "BG", "BF", "BI", "CV", "KH", "CM", "CA", "KY", "CF", "TD", "CL", "CN", "CX", "CC", "CO", "KM", "CG", "CD", "CK", "CR", "CI", "HR", "CU", "CW", "CY", "CZ", "DK", "DJ", "DM", "DO", "EC", "EG", "SV", "GQ", "ER", "EE", "SZ", "ET", "FK", "FO", "FJ", "FI", "FR", "GF", "PF", "TF", "GA", "GM", "GE", "DE", "GH", "GI", "GR", "GL", "GD", "GP", "GU", "GT", "GG", "GN", "GW", "GY", "HT", "HM", "VA", "HN", "HK", "HU", "IS", "IN", "ID", "IR", "IQ", "IE", "IM", "IL", "IT", "JM", "JP", "JE", "JO", "KZ", "KE", "KI", "KP", "KR", "KW", "KG", "LA", "LV", "LB", "LS", "LR", "LY", "LI", "LT", "LU", "MO", "MG", "MW", "MY", "MV", "ML", "MT", "MH", "MQ", "MR", "MU", "YT", "MX", "FM", "MD", "MC", "MN", "ME", "MS", "MA", "MZ", "MM", "NA", "NR", "NP", "NL", "NC", "NZ", "NI", "NE", "NG", "NU", "NF", "MP", "MK", "NO", "OM", "PK", "PW", "PS", "PA", "PG", "PY", "PE", "PH", "PN", "PL", "PT", "PR", "QA", "RE", "RO", "RU", "RW", "BL", "SH", "KN", "LC", "MF", "PM", "VC", "WS", "SM", "ST", "SA", "SN", "RS", "SC", "SL", "SG", "SX", "SK", "SI", "SB", "SO", "ZA", "GS", "SS", "ES", "LK", "SD", "SR", "SJ", "SE", "CH", "SY", "TW", "TJ", "TZ", "TH", "TL", "TG", "TK", "TO", "TT", "TN", "TR", "TM", "TC", "TV", "UG", "UA", "AE", "GB", "US", "UM", "UY", "UZ", "VU", "VE", "VN", "VG", "VI", "WF", "EH", "YE", "ZM", "ZW", "AX", "XK" };
                return new HashSet<string>(codes, StringComparer.OrdinalIgnoreCase);
            }

            /// <summary>
            /// Returns a string that represents the contact information in the format required for Swiss QR codes.
            /// </summary>
            /// <returns>A string representing the contact information.</returns>
            public override string ToString()
            {
                string contactData = $"{(AddressType.StructuredAddress == _adrType ? "S" : "K")}{_br}"; //AdrTp
                contactData += _name.Replace("\n", "") + _br; //Name
                contactData += (!string.IsNullOrEmpty(_streetOrAddressline1) ? _streetOrAddressline1!.Replace("\n", "") : string.Empty) + _br; //StrtNmOrAdrLine1
                contactData += (!string.IsNullOrEmpty(_houseNumberOrAddressline2) ? _houseNumberOrAddressline2!.Replace("\n", "") : string.Empty) + _br; //BldgNbOrAdrLine2
                contactData += _zipCode.Replace("\n", "") + _br; //PstCd
                contactData += _city.Replace("\n", "") + _br; //TwnNm
                contactData += _country + _br; //Ctry
                return contactData;
            }

            /// <summary>
            /// Defines the type of address.
            /// </summary>
            public enum AddressType
            {
                /// <summary>
                /// Structured Address
                /// </summary>
                StructuredAddress,
                /// <summary>
                /// Combined Address
                /// </summary>
                CombinedAddress
            }

            /// <summary>
            /// Represents errors that occur during the creation of a Swiss QR code contact.
            /// </summary>
            public class SwissQrCodeContactException : Exception
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="SwissQrCodeContactException"/> class.
                /// </summary>
                public SwissQrCodeContactException()
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="SwissQrCodeContactException"/> class with a specified error message.
                /// </summary>
                /// <param name="message">The message that describes the error.</param>
                public SwissQrCodeContactException(string message)
                    : base(message)
                {
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="SwissQrCodeContactException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
                /// </summary>
                /// <param name="message">The error message that explains the reason for the exception.</param>
                /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
                public SwissQrCodeContactException(string message, Exception inner)
                    : base(message, inner)
                {
                }
            }
        }

        /// <summary>
        /// Returns a string that represents the entire Swiss QR code payload.
        /// </summary>
        /// <returns>A string representing the Swiss QR code payload.</returns>
        public override string ToString()
        {
            //Header "logical" element
            var SwissQrCodePayload = "SPC" + _br; //QRType
            SwissQrCodePayload += "0200" + _br; //Version
            SwissQrCodePayload += "1" + _br; //Coding

            //CdtrInf "logical" element
            SwissQrCodePayload += _iban.ToString() + _br; //IBAN


            //Cdtr "logical" element
            SwissQrCodePayload += _creditor.ToString();

            //UltmtCdtr "logical" element
            //Since version 2.0 ultimate creditor was marked as "for future use" and has to be delivered empty in any case!
            SwissQrCodePayload += string.Concat(Enumerable.Repeat(_br, 7).ToArray());

            //CcyAmtDate "logical" element
            //Amoutn has to use . as decimal seperator in any case. See https://www.paymentstandards.ch/dam/downloads/ig-qr-bill-en.pdf page 27.
            SwissQrCodePayload += (_amount != null ? $"{_amount:0.00}".Replace(",", ".") : string.Empty) + _br; //Amt
            SwissQrCodePayload += _currency + _br; //Ccy                
            //Removed in S-QR version 2.0
            //SwissQrCodePayload += (requestedDateOfPayment != null ?  ((DateTime)requestedDateOfPayment).ToString("yyyy-MM-dd") : string.Empty) + br; //ReqdExctnDt

            //UltmtDbtr "logical" element
            if (_debitor != null)
                SwissQrCodePayload += _debitor.ToString();
            else
                SwissQrCodePayload += string.Concat(Enumerable.Repeat(_br, 7).ToArray());


            //RmtInf "logical" element
            SwissQrCodePayload += _reference.RefType.ToString() + _br; //Tp
            SwissQrCodePayload += (!string.IsNullOrEmpty(_reference.ReferenceText) ? _reference.ReferenceText : string.Empty) + _br; //Ref


            //AddInf "logical" element
            SwissQrCodePayload += (!string.IsNullOrEmpty(_additionalInformation.UnstructureMessage) ? _additionalInformation.UnstructureMessage : string.Empty) + _br; //Ustrd
            SwissQrCodePayload += _additionalInformation.Trailer + _br; //Trailer
            // Bugfix PR #399 If BillInformation is empty, insert no linebreak
            SwissQrCodePayload += (!string.IsNullOrEmpty(_additionalInformation.BillInformation) ? _additionalInformation.BillInformation + _br : string.Empty); //StrdBkgInf

            //AltPmtInf "logical" element
            if (!string.IsNullOrEmpty(_alternativeProcedure1))
                SwissQrCodePayload += _alternativeProcedure1!.Replace("\n", "") + _br; //AltPmt
            if (!string.IsNullOrEmpty(_alternativeProcedure2))
                SwissQrCodePayload += _alternativeProcedure2!.Replace("\n", "") + _br; //AltPmt

            //S-QR specification 2.0, chapter 4.2.3
            if (SwissQrCodePayload.EndsWith(_br))
                SwissQrCodePayload = SwissQrCodePayload.Remove(SwissQrCodePayload.Length - _br.Length);

            return SwissQrCodePayload;
        }




        /// <summary>
        /// ISO 4217 currency codes.
        /// </summary>
        public enum Currency
        {
            /// <summary>
            /// Swiss Franc
            /// </summary>
            CHF = 756,
            /// <summary>
            /// Euro
            /// </summary>
            EUR = 978
        }

        /// <summary>
        /// Represents errors that occur during Swiss QR Code generation.
        /// </summary>
        public class SwissQrCodeException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SwissQrCodeException"/> class.
            /// </summary>
            public SwissQrCodeException()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SwissQrCodeException"/> class with a specified error message.
            /// </summary>
            /// <param name="message">The message that describes the error.</param>
            public SwissQrCodeException(string message)
                : base(message)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SwissQrCodeException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
            /// </summary>
            /// <param name="message">The error message that explains the reason for the exception.</param>
            /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
            public SwissQrCodeException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }
    }
}
