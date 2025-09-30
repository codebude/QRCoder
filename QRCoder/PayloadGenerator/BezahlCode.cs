using System;
using System.Text.RegularExpressions;

namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Represents a BezahlCode payload for QR codes.
    /// </summary>
    public class BezahlCode : Payload
    {
        //BezahlCode specification: http://www.bezahlcode.de/wp-content/uploads/BezahlCode_TechDok.pdf
        //SEPA Credit Transfer Guideline: https://www.europeanpaymentscouncil.eu/sites/default/files/kb/file/2024-03/EPC069-12%20v3.1%20Quick%20Response%20Code%20-%20Guidelines%20to%20Enable%20the%20Data%20Capture%20for%20the%20Initiation%20of%20an%20SCT.pdf

        private readonly string? _iban, _bic, _account, _bnc, _sepaReference, _creditorId, _mandateId, _periodicTimeunit;
        private readonly string _name, _reason;
        private readonly decimal _amount;
        private readonly int _postingKey, _periodicTimeunitRotation;
        private readonly Currency _currency;
        private readonly AuthorityType _authority;
        private readonly DateTime _executionDate, _dateOfSignature, _periodicFirstExecutionDate, _periodicLastExecutionDate;


        /// <summary>
        /// Initializes a new instance of the <see cref="BezahlCode"/> class for contact data.
        /// </summary>
        /// <param name="authority">Type of the bank transfer</param>
        /// <param name="name">Name of the receiver (Empfänger)</param>
        /// <param name="account">Bank account (Kontonummer)</param>
        /// <param name="bnc">Bank institute (Bankleitzahl)</param>
        /// <param name="iban">IBAN</param>
        /// <param name="bic">BIC</param>
        /// <param name="reason">Reason (Verwendungszweck)</param>
        public BezahlCode(AuthorityType authority, string name, string account = "", string bnc = "", string iban = "", string bic = "", string reason = "") : this(authority, name, account, bnc, iban, bic, 0, string.Empty, 0, null, null, string.Empty, string.Empty, null, reason, 0, string.Empty, Currency.EUR, null, 1)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="BezahlCode"/> class for non-SEPA payments.
        /// </summary>
        /// <param name="authority">Type of the bank transfer</param>
        /// <param name="name">Name of the receiver (Empfänger)</param>
        /// <param name="account">Bank account (Kontonummer)</param>
        /// <param name="bnc">Bank institute (Bankleitzahl)</param>
        /// <param name="amount">Amount (Betrag)</param>
        /// <param name="periodicTimeunit">Unit of intervall for payment ('M' = monthly, 'W' = weekly)</param>
        /// <param name="periodicTimeunitRotation">Intervall for payment. This value is combined with 'periodicTimeunit'</param>
        /// <param name="periodicFirstExecutionDate">Date of first periodic execution</param>
        /// <param name="periodicLastExecutionDate">Date of last periodic execution</param>
        /// <param name="reason">Reason (Verwendungszweck)</param>
        /// <param name="postingKey">Transfer Key (Textschlüssel, z.B. Spendenzahlung = 69)</param>
        /// <param name="currency">Currency (Währung)</param>
        /// <param name="executionDate">Execution date (Ausführungsdatum)</param>
        public BezahlCode(AuthorityType authority, string name, string account, string bnc, decimal amount, string periodicTimeunit = "", int periodicTimeunitRotation = 0, DateTime? periodicFirstExecutionDate = null, DateTime? periodicLastExecutionDate = null, string reason = "", int postingKey = 0, Currency currency = Currency.EUR, DateTime? executionDate = null) : this(authority, name, account, bnc, string.Empty, string.Empty, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, string.Empty, string.Empty, null, reason, postingKey, string.Empty, currency, executionDate, 2)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BezahlCode"/> class for SEPA payments.
        /// </summary>
        /// <param name="authority">Type of the bank transfer</param>
        /// <param name="name">Name of the receiver (Empfänger)</param>
        /// <param name="iban">IBAN</param>
        /// <param name="bic">BIC</param>
        /// <param name="amount">Amount (Betrag)</param>
        /// <param name="periodicTimeunit">Unit of intervall for payment ('M' = monthly, 'W' = weekly)</param>
        /// <param name="periodicTimeunitRotation">Intervall for payment. This value is combined with 'periodicTimeunit'</param>
        /// <param name="periodicFirstExecutionDate">Date of first periodic execution</param>
        /// <param name="periodicLastExecutionDate">Date of last periodic execution</param>
        /// <param name="creditorId">Creditor id (Gläubiger ID)</param>
        /// <param name="mandateId">Manadate id (Mandatsreferenz)</param>
        /// <param name="dateOfSignature">Signature date (Erteilungsdatum des Mandats)</param>
        /// <param name="reason">Reason (Verwendungszweck)</param>
        /// <param name="postingKey">Transfer Key (Textschlüssel, z.B. Spendenzahlung = 69)</param>
        /// <param name="sepaReference">SEPA reference (SEPA-Referenz)</param>
        /// <param name="currency">Currency (Währung)</param>
        /// <param name="executionDate">Execution date (Ausführungsdatum)</param>
        public BezahlCode(AuthorityType authority, string name, string iban, string bic, decimal amount, string periodicTimeunit = "", int periodicTimeunitRotation = 0, DateTime? periodicFirstExecutionDate = null, DateTime? periodicLastExecutionDate = null, string creditorId = "", string mandateId = "", DateTime? dateOfSignature = null, string reason = "", string sepaReference = "", Currency currency = Currency.EUR, DateTime? executionDate = null) : this(authority, name, string.Empty, string.Empty, iban, bic, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, creditorId, mandateId, dateOfSignature, reason, 0, sepaReference, currency, executionDate, 3)
        {
        }




        /// <summary>
        /// Generic constructor. Please use specific (non-SEPA or SEPA) constructor.
        /// </summary>
        /// <param name="authority">Type of the bank transfer</param>
        /// <param name="name">Name of the receiver (Empfänger)</param>
        /// <param name="account">Bank account (Kontonummer)</param>
        /// <param name="bnc">Bank institute (Bankleitzahl)</param>
        /// <param name="iban">IBAN</param>
        /// <param name="bic">BIC</param>
        /// <param name="amount">Amount (Betrag)</param>
        /// <param name="periodicTimeunit">Unit of intervall for payment ('M' = monthly, 'W' = weekly)</param>
        /// <param name="periodicTimeunitRotation">Intervall for payment. This value is combined with 'periodicTimeunit'</param>
        /// <param name="periodicFirstExecutionDate">Date of first periodic execution</param>
        /// <param name="periodicLastExecutionDate">Date of last periodic execution</param>
        /// <param name="creditorId">Creditor id (Gläubiger ID)</param>
        /// <param name="mandateId">Manadate id (Mandatsreferenz)</param>
        /// <param name="dateOfSignature">Signature date (Erteilungsdatum des Mandats)</param>
        /// <param name="reason">Reason (Verwendungszweck)</param>
        /// <param name="postingKey">Transfer Key (Textschlüssel, z.B. Spendenzahlung = 69)</param>
        /// <param name="sepaReference">SEPA reference (SEPA-Referenz)</param>
        /// <param name="currency">Currency (Währung)</param>
        /// <param name="executionDate">Execution date (Ausführungsdatum)</param>
        /// <param name="internalMode">Only used for internal state handdling</param>
        public BezahlCode(AuthorityType authority, string name, string account, string bnc, string iban, string bic, decimal amount, string periodicTimeunit = "", int periodicTimeunitRotation = 0, DateTime? periodicFirstExecutionDate = null, DateTime? periodicLastExecutionDate = null, string creditorId = "", string mandateId = "", DateTime? dateOfSignature = null, string reason = "", int postingKey = 0, string sepaReference = "", Currency currency = Currency.EUR, DateTime? executionDate = null, int internalMode = 0)
        {
            //Loaded via "contact-constructor"
            if (internalMode == 1)
            {
                if (authority != AuthorityType.contact && authority != AuthorityType.contact_v2)
                    throw new BezahlCodeException("The constructor without an amount may only ne used with authority types 'contact' and 'contact_v2'.");
                if (authority == AuthorityType.contact && (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(bnc)))
                    throw new BezahlCodeException("When using authority type 'contact' the parameters 'account' and 'bnc' must be set.");

                if (authority != AuthorityType.contact_v2)
                {
                    var oldFilled = (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(bnc));
                    var newFilled = (!string.IsNullOrEmpty(iban) && !string.IsNullOrEmpty(bic));
                    if ((!oldFilled && !newFilled) || (oldFilled && newFilled))
                        throw new BezahlCodeException("When using authority type 'contact_v2' either the parameters 'account' and 'bnc' or the parameters 'iban' and 'bic' must be set. Leave the other parameter pair empty.");
                }
            }
            else if (internalMode == 2)
            {
#pragma warning disable CS0612
                if (authority != AuthorityType.periodicsinglepayment && authority != AuthorityType.singledirectdebit && authority != AuthorityType.singlepayment)
                    throw new BezahlCodeException("The constructor with 'account' and 'bnc' may only be used with 'non SEPA' authority types. Either choose another authority type or switch constructor.");
                if (authority == AuthorityType.periodicsinglepayment && (string.IsNullOrEmpty(periodicTimeunit) || periodicTimeunitRotation == 0))
                    throw new BezahlCodeException("When using 'periodicsinglepayment' as authority type, the parameters 'periodicTimeunit' and 'periodicTimeunitRotation' must be set.");
#pragma warning restore CS0612
            }
            else if (internalMode == 3)
            {
                if (authority != AuthorityType.periodicsinglepaymentsepa && authority != AuthorityType.singledirectdebitsepa && authority != AuthorityType.singlepaymentsepa)
                    throw new BezahlCodeException("The constructor with 'iban' and 'bic' may only be used with 'SEPA' authority types. Either choose another authority type or switch constructor.");
                if (authority == AuthorityType.periodicsinglepaymentsepa && (string.IsNullOrEmpty(periodicTimeunit) || periodicTimeunitRotation == 0))
                    throw new BezahlCodeException("When using 'periodicsinglepaymentsepa' as authority type, the parameters 'periodicTimeunit' and 'periodicTimeunitRotation' must be set.");
            }

            _authority = authority;

            var oldWayFilled = (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(bnc));
            var newWayFilled = (!string.IsNullOrEmpty(iban) && !string.IsNullOrEmpty(bic));

            if (name.Length > 70)
                throw new BezahlCodeException("(Payee-)Name must be shorter than 71 chars.");
            _name = name;

            //Limit reason length depending on payment type
            //140 chars for SEPA payments and 27 chars for others 
            var reasonLength = authority == AuthorityType.periodicsinglepaymentsepa || authority == AuthorityType.singledirectdebitsepa || authority == AuthorityType.singlepaymentsepa || (authority == AuthorityType.contact_v2 && newWayFilled) ? 140 : 27;
            if (reason.Length > reasonLength)
                throw new BezahlCodeException($"Reasons texts have to be shorter than {reasonLength + 1} chars.");
            _reason = reason;

            //Non-SEPA payment types
#pragma warning disable CS0612
            if (authority == AuthorityType.periodicsinglepayment || authority == AuthorityType.singledirectdebit || authority == AuthorityType.singlepayment || authority == AuthorityType.contact || (authority == AuthorityType.contact_v2 && oldWayFilled))
            {
#pragma warning restore CS0612

                if (!Regex.IsMatch(account.Replace(" ", ""), @"^[0-9]{1,9}$"))
                    throw new BezahlCodeException("The account entered isn't valid.");
                _account = account.Replace(" ", "").ToUpper();
                if (!Regex.IsMatch(bnc.Replace(" ", ""), @"^[0-9]{1,9}$"))
                    throw new BezahlCodeException("The bnc entered isn't valid.");
                _bnc = bnc.Replace(" ", "").ToUpper();

                if (authority != AuthorityType.contact && authority != AuthorityType.contact_v2)
                {
                    if (postingKey < 0 || postingKey >= 100)
                        throw new BezahlCodeException("PostingKey must be within 0 and 99.");
                    _postingKey = postingKey;
                }
            }

            //SEPA payment types
            if (authority == AuthorityType.periodicsinglepaymentsepa || authority == AuthorityType.singledirectdebitsepa || authority == AuthorityType.singlepaymentsepa || (authority == AuthorityType.contact_v2 && newWayFilled))
            {
                if (!IsValidIban(iban))
                    throw new BezahlCodeException("The IBAN entered isn't valid.");
                _iban = iban.Replace(" ", "").ToUpper();
                if (!IsValidBic(bic))
                    throw new BezahlCodeException("The BIC entered isn't valid.");
                _bic = bic.Replace(" ", "").ToUpper();

                if (authority != AuthorityType.contact_v2)
                {
                    if (sepaReference.Length > 35)
                        throw new BezahlCodeException("SEPA reference texts have to be shorter than 36 chars.");
                    _sepaReference = sepaReference;

                    if (!string.IsNullOrEmpty(creditorId) && !Regex.IsMatch(creditorId.Replace(" ", ""), @"^[a-zA-Z]{2,2}[0-9]{2,2}([A-Za-z0-9]|[\+|\?|/|\-|:|\(|\)|\.|,|']){3,3}([A-Za-z0-9]|[\+|\?|/|\-|:|\(|\)|\.|,|']){1,28}$"))
                        throw new BezahlCodeException("The creditorId entered isn't valid.");
                    _creditorId = creditorId;
                    if (!string.IsNullOrEmpty(mandateId) && !Regex.IsMatch(mandateId.Replace(" ", ""), @"^([A-Za-z0-9]|[\+|\?|/|\-|:|\(|\)|\.|,|']){1,35}$"))
                        throw new BezahlCodeException("The mandateId entered isn't valid.");
                    _mandateId = mandateId;
                    if (dateOfSignature != null)
                        _dateOfSignature = (DateTime)dateOfSignature;
                }
            }

            //Checks for all payment types
            if (authority != AuthorityType.contact && authority != AuthorityType.contact_v2)
            {
                if (amount.ToString().Replace(",", ".").Contains(".") && amount.ToString().Replace(",", ".").Split('.')[1].TrimEnd('0').Length > 2)
                    throw new BezahlCodeException("Amount must have less than 3 digits after decimal point.");
                if (amount < 0.01m || amount > 999999999.99m)
                    throw new BezahlCodeException("Amount has to at least 0.01 and must be smaller or equal to 999999999.99.");
                _amount = amount;

                _currency = currency;

                if (executionDate == null)
                    _executionDate = DateTime.Now;
                else
                {
                    if (DateTime.Today.Ticks > executionDate.Value.Ticks)
                        throw new BezahlCodeException("Execution date must be today or in future.");
                    _executionDate = (DateTime)executionDate;
                }
#pragma warning disable CS0612
                if (authority == AuthorityType.periodicsinglepayment || authority == AuthorityType.periodicsinglepaymentsepa)
#pragma warning restore CS0612
                {
                    if (periodicTimeunit.ToUpper() != "M" && periodicTimeunit.ToUpper() != "W")
                        throw new BezahlCodeException("The periodicTimeunit must be either 'M' (monthly) or 'W' (weekly).");
                    _periodicTimeunit = periodicTimeunit;
                    if (periodicTimeunitRotation < 1 || periodicTimeunitRotation > 52)
                        throw new BezahlCodeException("The periodicTimeunitRotation must be 1 or greater. (It means repeat the payment every 'periodicTimeunitRotation' weeks/months.");
                    _periodicTimeunitRotation = periodicTimeunitRotation;
                    if (periodicFirstExecutionDate != null)
                        _periodicFirstExecutionDate = (DateTime)periodicFirstExecutionDate;
                    if (periodicLastExecutionDate != null)
                        _periodicLastExecutionDate = (DateTime)periodicLastExecutionDate;
                }

            }



        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var bezahlCodePayload = $"bank://{_authority}?";

            bezahlCodePayload += $"name={Uri.EscapeDataString(_name)}&";

            if (_authority != AuthorityType.contact && _authority != AuthorityType.contact_v2)
            {
                //Handle what is same for all payments
#pragma warning disable CS0612
                if (_authority == AuthorityType.periodicsinglepayment || _authority == AuthorityType.singledirectdebit || _authority == AuthorityType.singlepayment)
#pragma warning restore CS0612
                {
                    bezahlCodePayload += $"account={_account}&";
                    bezahlCodePayload += $"bnc={_bnc}&";
                    if (_postingKey > 0)
                        bezahlCodePayload += $"postingkey={_postingKey}&";
                }
                else
                {
                    bezahlCodePayload += $"iban={_iban}&";
                    bezahlCodePayload += $"bic={_bic}&";

                    if (!string.IsNullOrEmpty(_sepaReference))
                        bezahlCodePayload += $"separeference={Uri.EscapeDataString(_sepaReference)}&";

                    if (_authority == AuthorityType.singledirectdebitsepa)
                    {
                        if (!string.IsNullOrEmpty(_creditorId))
                            bezahlCodePayload += $"creditorid={Uri.EscapeDataString(_creditorId)}&";
                        if (!string.IsNullOrEmpty(_mandateId))
                            bezahlCodePayload += $"mandateid={Uri.EscapeDataString(_mandateId)}&";
                        if (_dateOfSignature != DateTime.MinValue)
                            bezahlCodePayload += $"dateofsignature={_dateOfSignature.ToString("ddMMyyyy")}&";
                    }
                }
                bezahlCodePayload += $"amount={_amount:0.00}&".Replace(".", ",");

                if (!string.IsNullOrEmpty(_reason))
                    bezahlCodePayload += $"reason={Uri.EscapeDataString(_reason)}&";
                bezahlCodePayload += $"currency={_currency}&";
                bezahlCodePayload += $"executiondate={_executionDate.ToString("ddMMyyyy")}&";
#pragma warning disable CS0612
                if (_authority == AuthorityType.periodicsinglepayment || _authority == AuthorityType.periodicsinglepaymentsepa)
                {
                    bezahlCodePayload += $"periodictimeunit={_periodicTimeunit}&";
                    bezahlCodePayload += $"periodictimeunitrotation={_periodicTimeunitRotation}&";
                    if (_periodicFirstExecutionDate != DateTime.MinValue)
                        bezahlCodePayload += $"periodicfirstexecutiondate={_periodicFirstExecutionDate.ToString("ddMMyyyy")}&";
                    if (_periodicLastExecutionDate != DateTime.MinValue)
                        bezahlCodePayload += $"periodiclastexecutiondate={_periodicLastExecutionDate.ToString("ddMMyyyy")}&";
                }
#pragma warning restore CS0612
            }
            else
            {
                //Handle what is same for all contacts
                if (_authority == AuthorityType.contact)
                {
                    bezahlCodePayload += $"account={_account}&";
                    bezahlCodePayload += $"bnc={_bnc}&";
                }
                else if (_authority == AuthorityType.contact_v2)
                {
                    if (!string.IsNullOrEmpty(_account) && !string.IsNullOrEmpty(_bnc))
                    {
                        bezahlCodePayload += $"account={_account}&";
                        bezahlCodePayload += $"bnc={_bnc}&";
                    }
                    else
                    {
                        bezahlCodePayload += $"iban={_iban}&";
                        bezahlCodePayload += $"bic={_bic}&";
                    }
                }

                if (!string.IsNullOrEmpty(_reason))
                    bezahlCodePayload += $"reason={Uri.EscapeDataString(_reason)}&";
            }

            return bezahlCodePayload.Trim('&');
        }

        /// <summary>
        /// ISO 4217 currency codes
        /// </summary>
        public enum Currency
        {
            AED = 784,
            AFN = 971,
            ALL = 008,
            AMD = 051,
            ANG = 532,
            AOA = 973,
            ARS = 032,
            AUD = 036,
            AWG = 533,
            AZN = 944,
            BAM = 977,
            BBD = 052,
            BDT = 050,
            BGN = 975,
            BHD = 048,
            BIF = 108,
            BMD = 060,
            BND = 096,
            BOB = 068,
            BOV = 984,
            BRL = 986,
            BSD = 044,
            BTN = 064,
            BWP = 072,
            BYR = 974,
            BZD = 084,
            CAD = 124,
            CDF = 976,
            CHE = 947,
            CHF = 756,
            CHW = 948,
            CLF = 990,
            CLP = 152,
            CNY = 156,
            COP = 170,
            COU = 970,
            CRC = 188,
            CUC = 931,
            CUP = 192,
            CVE = 132,
            CZK = 203,
            DJF = 262,
            DKK = 208,
            DOP = 214,
            DZD = 012,
            EGP = 818,
            ERN = 232,
            ETB = 230,
            EUR = 978,
            FJD = 242,
            FKP = 238,
            GBP = 826,
            GEL = 981,
            GHS = 936,
            GIP = 292,
            GMD = 270,
            GNF = 324,
            GTQ = 320,
            GYD = 328,
            HKD = 344,
            HNL = 340,
            HRK = 191,
            HTG = 332,
            HUF = 348,
            IDR = 360,
            ILS = 376,
            INR = 356,
            IQD = 368,
            IRR = 364,
            ISK = 352,
            JMD = 388,
            JOD = 400,
            JPY = 392,
            KES = 404,
            KGS = 417,
            KHR = 116,
            KMF = 174,
            KPW = 408,
            KRW = 410,
            KWD = 414,
            KYD = 136,
            KZT = 398,
            LAK = 418,
            LBP = 422,
            LKR = 144,
            LRD = 430,
            LSL = 426,
            LYD = 434,
            MAD = 504,
            MDL = 498,
            MGA = 969,
            MKD = 807,
            MMK = 104,
            MNT = 496,
            MOP = 446,
            MRO = 478,
            MUR = 480,
            MVR = 462,
            MWK = 454,
            MXN = 484,
            MXV = 979,
            MYR = 458,
            MZN = 943,
            NAD = 516,
            NGN = 566,
            NIO = 558,
            NOK = 578,
            NPR = 524,
            NZD = 554,
            OMR = 512,
            PAB = 590,
            PEN = 604,
            PGK = 598,
            PHP = 608,
            PKR = 586,
            PLN = 985,
            PYG = 600,
            QAR = 634,
            RON = 946,
            RSD = 941,
            RUB = 643,
            RWF = 646,
            SAR = 682,
            SBD = 090,
            SCR = 690,
            SDG = 938,
            SEK = 752,
            SGD = 702,
            SHP = 654,
            SLL = 694,
            SOS = 706,
            SRD = 968,
            SSP = 728,
            STD = 678,
            SVC = 222,
            SYP = 760,
            SZL = 748,
            THB = 764,
            TJS = 972,
            TMT = 934,
            TND = 788,
            TOP = 776,
            TRY = 949,
            TTD = 780,
            TWD = 901,
            TZS = 834,
            UAH = 980,
            UGX = 800,
            USD = 840,
            USN = 997,
            UYI = 940,
            UYU = 858,
            UZS = 860,
            VEF = 937,
            VND = 704,
            VUV = 548,
            WST = 882,
            XAF = 950,
            XAG = 961,
            XAU = 959,
            XBA = 955,
            XBB = 956,
            XBC = 957,
            XBD = 958,
            XCD = 951,
            XDR = 960,
            XOF = 952,
            XPD = 964,
            XPF = 953,
            XPT = 962,
            XSU = 994,
            XTS = 963,
            XUA = 965,
            XXX = 999,
            YER = 886,
            ZAR = 710,
            ZMW = 967,
            ZWL = 932
        }


        /// <summary>
        /// Operation modes of the BezahlCode
        /// </summary>
        public enum AuthorityType
        {
            /// <summary>
            /// Single payment (Überweisung)
            /// </summary>
            [Obsolete]
            singlepayment,
            /// <summary>
            /// Single SEPA payment (SEPA-Überweisung)
            /// </summary>
            singlepaymentsepa,
            /// <summary>
            /// Single debit (Lastschrift)
            /// </summary>
            [Obsolete]
            singledirectdebit,
            /// <summary>
            /// Single SEPA debit (SEPA-Lastschrift)
            /// </summary>
            singledirectdebitsepa,
            /// <summary>
            /// Periodic payment (Dauerauftrag)
            /// </summary>
            [Obsolete]
            periodicsinglepayment,
            /// <summary>
            /// Periodic SEPA payment (SEPA-Dauerauftrag)
            /// </summary>
            periodicsinglepaymentsepa,
            /// <summary>
            /// Contact data
            /// </summary>
            contact,
            /// <summary>
            /// Contact data V2
            /// </summary>
            contact_v2
        }

        /// <summary>
        /// Exception class for BezahlCode errors.
        /// </summary>
        public class BezahlCodeException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="BezahlCodeException"/> class.
            /// </summary>
            public BezahlCodeException()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BezahlCodeException"/> class with a specified error message.
            /// </summary>
            /// <param name="message">The message that describes the error.</param>
            public BezahlCodeException(string message)
                : base(message)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BezahlCodeException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
            /// </summary>
            /// <param name="message">The message that describes the error.</param>
            /// <param name="inner">The exception that is the cause of the current exception.</param>
            public BezahlCodeException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }
    }
}
