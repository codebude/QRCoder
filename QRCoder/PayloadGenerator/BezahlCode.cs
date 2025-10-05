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
            /// <summary>
            /// United Arab Emirates Dirham
            /// </summary>
            AED = 784,
            /// <summary>
            /// Afghan Afghani
            /// </summary>
            AFN = 971,
            /// <summary>
            /// Albanian Lek
            /// </summary>
            ALL = 008,
            /// <summary>
            /// Armenian Dram
            /// </summary>
            AMD = 051,
            /// <summary>
            /// Netherlands Antillean Guilder
            /// </summary>
            ANG = 532,
            /// <summary>
            /// Angolan Kwanza
            /// </summary>
            AOA = 973,
            /// <summary>
            /// Argentine Peso
            /// </summary>
            ARS = 032,
            /// <summary>
            /// Australian Dollar
            /// </summary>
            AUD = 036,
            /// <summary>
            /// Aruban Florin
            /// </summary>
            AWG = 533,
            /// <summary>
            /// Azerbaijani Manat
            /// </summary>
            AZN = 944,
            /// <summary>
            /// Bosnia and Herzegovina Convertible Mark
            /// </summary>
            BAM = 977,
            /// <summary>
            /// Barbados Dollar
            /// </summary>
            BBD = 052,
            /// <summary>
            /// Bangladeshi Taka
            /// </summary>
            BDT = 050,
            /// <summary>
            /// Bulgarian Lev
            /// </summary>
            BGN = 975,
            /// <summary>
            /// Bahraini Dinar
            /// </summary>
            BHD = 048,
            /// <summary>
            /// Burundian Franc
            /// </summary>
            BIF = 108,
            /// <summary>
            /// Bermudian Dollar
            /// </summary>
            BMD = 060,
            /// <summary>
            /// Brunei Dollar
            /// </summary>
            BND = 096,
            /// <summary>
            /// Bolivian Boliviano
            /// </summary>
            BOB = 068,
            /// <summary>
            /// Bolivian Mvdol (funds code)
            /// </summary>
            BOV = 984,
            /// <summary>
            /// Brazilian Real
            /// </summary>
            BRL = 986,
            /// <summary>
            /// Bahamian Dollar
            /// </summary>
            BSD = 044,
            /// <summary>
            /// Bhutanese Ngultrum
            /// </summary>
            BTN = 064,
            /// <summary>
            /// Botswana Pula
            /// </summary>
            BWP = 072,
            /// <summary>
            /// Belarusian Ruble
            /// </summary>
            BYR = 974,
            /// <summary>
            /// Belize Dollar
            /// </summary>
            BZD = 084,
            /// <summary>
            /// Canadian Dollar
            /// </summary>
            CAD = 124,
            /// <summary>
            /// Congolese Franc
            /// </summary>
            CDF = 976,
            /// <summary>
            /// WIR Euro (complementary currency)
            /// </summary>
            CHE = 947,
            /// <summary>
            /// Swiss Franc
            /// </summary>
            CHF = 756,
            /// <summary>
            /// WIR Franc (complementary currency)
            /// </summary>
            CHW = 948,
            /// <summary>
            /// Unidad de Fomento (funds code)
            /// </summary>
            CLF = 990,
            /// <summary>
            /// Chilean Peso
            /// </summary>
            CLP = 152,
            /// <summary>
            /// Chinese Yuan
            /// </summary>
            CNY = 156,
            /// <summary>
            /// Colombian Peso
            /// </summary>
            COP = 170,
            /// <summary>
            /// Unidad de Valor Real (UVR) (funds code)
            /// </summary>
            COU = 970,
            /// <summary>
            /// Costa Rican Colón
            /// </summary>
            CRC = 188,
            /// <summary>
            /// Cuban Convertible Peso
            /// </summary>
            CUC = 931,
            /// <summary>
            /// Cuban Peso
            /// </summary>
            CUP = 192,
            /// <summary>
            /// Cape Verde Escudo
            /// </summary>
            CVE = 132,
            /// <summary>
            /// Czech Koruna
            /// </summary>
            CZK = 203,
            /// <summary>
            /// Djiboutian Franc
            /// </summary>
            DJF = 262,
            /// <summary>
            /// Danish Krone
            /// </summary>
            DKK = 208,
            /// <summary>
            /// Dominican Peso
            /// </summary>
            DOP = 214,
            /// <summary>
            /// Algerian Dinar
            /// </summary>
            DZD = 012,
            /// <summary>
            /// Egyptian Pound
            /// </summary>
            EGP = 818,
            /// <summary>
            /// Eritrean Nakfa
            /// </summary>
            ERN = 232,
            /// <summary>
            /// Ethiopian Birr
            /// </summary>
            ETB = 230,
            /// <summary>
            /// Euro
            /// </summary>
            EUR = 978,
            /// <summary>
            /// Fiji Dollar
            /// </summary>
            FJD = 242,
            /// <summary>
            /// Falkland Islands Pound
            /// </summary>
            FKP = 238,
            /// <summary>
            /// Pound Sterling
            /// </summary>
            GBP = 826,
            /// <summary>
            /// Georgian Lari
            /// </summary>
            GEL = 981,
            /// <summary>
            /// Ghanaian Cedi
            /// </summary>
            GHS = 936,
            /// <summary>
            /// Gibraltar Pound
            /// </summary>
            GIP = 292,
            /// <summary>
            /// Gambian Dalasi
            /// </summary>
            GMD = 270,
            /// <summary>
            /// Guinean Franc
            /// </summary>
            GNF = 324,
            /// <summary>
            /// Guatemalan Quetzal
            /// </summary>
            GTQ = 320,
            /// <summary>
            /// Guyanese Dollar
            /// </summary>
            GYD = 328,
            /// <summary>
            /// Hong Kong Dollar
            /// </summary>
            HKD = 344,
            /// <summary>
            /// Honduran Lempira
            /// </summary>
            HNL = 340,
            /// <summary>
            /// Croatian Kuna
            /// </summary>
            HRK = 191,
            /// <summary>
            /// Haitian Gourde
            /// </summary>
            HTG = 332,
            /// <summary>
            /// Hungarian Forint
            /// </summary>
            HUF = 348,
            /// <summary>
            /// Indonesian Rupiah
            /// </summary>
            IDR = 360,
            /// <summary>
            /// Israeli New Shekel
            /// </summary>
            ILS = 376,
            /// <summary>
            /// Indian Rupee
            /// </summary>
            INR = 356,
            /// <summary>
            /// Iraqi Dinar
            /// </summary>
            IQD = 368,
            /// <summary>
            /// Iranian Rial
            /// </summary>
            IRR = 364,
            /// <summary>
            /// Icelandic Króna
            /// </summary>
            ISK = 352,
            /// <summary>
            /// Jamaican Dollar
            /// </summary>
            JMD = 388,
            /// <summary>
            /// Jordanian Dinar
            /// </summary>
            JOD = 400,
            /// <summary>
            /// Japanese Yen
            /// </summary>
            JPY = 392,
            /// <summary>
            /// Kenyan Shilling
            /// </summary>
            KES = 404,
            /// <summary>
            /// Kyrgyzstani Som
            /// </summary>
            KGS = 417,
            /// <summary>
            /// Cambodian Riel
            /// </summary>
            KHR = 116,
            /// <summary>
            /// Comoro Franc
            /// </summary>
            KMF = 174,
            /// <summary>
            /// North Korean Won
            /// </summary>
            KPW = 408,
            /// <summary>
            /// South Korean Won
            /// </summary>
            KRW = 410,
            /// <summary>
            /// Kuwaiti Dinar
            /// </summary>
            KWD = 414,
            /// <summary>
            /// Cayman Islands Dollar
            /// </summary>
            KYD = 136,
            /// <summary>
            /// Kazakhstani Tenge
            /// </summary>
            KZT = 398,
            /// <summary>
            /// Lao Kip
            /// </summary>
            LAK = 418,
            /// <summary>
            /// Lebanese Pound
            /// </summary>
            LBP = 422,
            /// <summary>
            /// Sri Lankan Rupee
            /// </summary>
            LKR = 144,
            /// <summary>
            /// Liberian Dollar
            /// </summary>
            LRD = 430,
            /// <summary>
            /// Lesotho Loti
            /// </summary>
            LSL = 426,
            /// <summary>
            /// Libyan Dinar
            /// </summary>
            LYD = 434,
            /// <summary>
            /// Moroccan Dirham
            /// </summary>
            MAD = 504,
            /// <summary>
            /// Moldovan Leu
            /// </summary>
            MDL = 498,
            /// <summary>
            /// Malagasy Ariary
            /// </summary>
            MGA = 969,
            /// <summary>
            /// Macedonian Denar
            /// </summary>
            MKD = 807,
            /// <summary>
            /// Myanmar Kyat
            /// </summary>
            MMK = 104,
            /// <summary>
            /// Mongolian Tögrög
            /// </summary>
            MNT = 496,
            /// <summary>
            /// Macanese Pataca
            /// </summary>
            MOP = 446,
            /// <summary>
            /// Mauritanian Ouguiya
            /// </summary>
            MRO = 478,
            /// <summary>
            /// Mauritian Rupee
            /// </summary>
            MUR = 480,
            /// <summary>
            /// Maldivian Rufiyaa
            /// </summary>
            MVR = 462,
            /// <summary>
            /// Malawian Kwacha
            /// </summary>
            MWK = 454,
            /// <summary>
            /// Mexican Peso
            /// </summary>
            MXN = 484,
            /// <summary>
            /// Mexican Unidad de Inversión (UDI) (funds code)
            /// </summary>
            MXV = 979,
            /// <summary>
            /// Malaysian Ringgit
            /// </summary>
            MYR = 458,
            /// <summary>
            /// Mozambican Metical
            /// </summary>
            MZN = 943,
            /// <summary>
            /// Namibian Dollar
            /// </summary>
            NAD = 516,
            /// <summary>
            /// Nigerian Naira
            /// </summary>
            NGN = 566,
            /// <summary>
            /// Nicaraguan Córdoba
            /// </summary>
            NIO = 558,
            /// <summary>
            /// Norwegian Krone
            /// </summary>
            NOK = 578,
            /// <summary>
            /// Nepalese Rupee
            /// </summary>
            NPR = 524,
            /// <summary>
            /// New Zealand Dollar
            /// </summary>
            NZD = 554,
            /// <summary>
            /// Omani Rial
            /// </summary>
            OMR = 512,
            /// <summary>
            /// Panamanian Balboa
            /// </summary>
            PAB = 590,
            /// <summary>
            /// Peruvian Sol
            /// </summary>
            PEN = 604,
            /// <summary>
            /// Papua New Guinean Kina
            /// </summary>
            PGK = 598,
            /// <summary>
            /// Philippine Peso
            /// </summary>
            PHP = 608,
            /// <summary>
            /// Pakistani Rupee
            /// </summary>
            PKR = 586,
            /// <summary>
            /// Polish Złoty
            /// </summary>
            PLN = 985,
            /// <summary>
            /// Paraguayan Guaraní
            /// </summary>
            PYG = 600,
            /// <summary>
            /// Qatari Riyal
            /// </summary>
            QAR = 634,
            /// <summary>
            /// Romanian Leu
            /// </summary>
            RON = 946,
            /// <summary>
            /// Serbian Dinar
            /// </summary>
            RSD = 941,
            /// <summary>
            /// Russian Ruble
            /// </summary>
            RUB = 643,
            /// <summary>
            /// Rwandan Franc
            /// </summary>
            RWF = 646,
            /// <summary>
            /// Saudi Riyal
            /// </summary>
            SAR = 682,
            /// <summary>
            /// Solomon Islands Dollar
            /// </summary>
            SBD = 090,
            /// <summary>
            /// Seychelles Rupee
            /// </summary>
            SCR = 690,
            /// <summary>
            /// Sudanese Pound
            /// </summary>
            SDG = 938,
            /// <summary>
            /// Swedish Krona
            /// </summary>
            SEK = 752,
            /// <summary>
            /// Singapore Dollar
            /// </summary>
            SGD = 702,
            /// <summary>
            /// Saint Helena Pound
            /// </summary>
            SHP = 654,
            /// <summary>
            /// Sierra Leonean Leone
            /// </summary>
            SLL = 694,
            /// <summary>
            /// Somali Shilling
            /// </summary>
            SOS = 706,
            /// <summary>
            /// Surinamese Dollar
            /// </summary>
            SRD = 968,
            /// <summary>
            /// South Sudanese Pound
            /// </summary>
            SSP = 728,
            /// <summary>
            /// São Tomé and Príncipe Dobra
            /// </summary>
            STD = 678,
            /// <summary>
            /// Salvadoran Colón
            /// </summary>
            SVC = 222,
            /// <summary>
            /// Syrian Pound
            /// </summary>
            SYP = 760,
            /// <summary>
            /// Swazi Lilangeni
            /// </summary>
            SZL = 748,
            /// <summary>
            /// Thai Baht
            /// </summary>
            THB = 764,
            /// <summary>
            /// Tajikistani Somoni
            /// </summary>
            TJS = 972,
            /// <summary>
            /// Turkmenistan Manat
            /// </summary>
            TMT = 934,
            /// <summary>
            /// Tunisian Dinar
            /// </summary>
            TND = 788,
            /// <summary>
            /// Tongan Paʻanga
            /// </summary>
            TOP = 776,
            /// <summary>
            /// Turkish Lira
            /// </summary>
            TRY = 949,
            /// <summary>
            /// Trinidad and Tobago Dollar
            /// </summary>
            TTD = 780,
            /// <summary>
            /// New Taiwan Dollar
            /// </summary>
            TWD = 901,
            /// <summary>
            /// Tanzanian Shilling
            /// </summary>
            TZS = 834,
            /// <summary>
            /// Ukrainian Hryvnia
            /// </summary>
            UAH = 980,
            /// <summary>
            /// Ugandan Shilling
            /// </summary>
            UGX = 800,
            /// <summary>
            /// United States Dollar
            /// </summary>
            USD = 840,
            /// <summary>
            /// United States Dollar (Next day) (funds code)
            /// </summary>
            USN = 997,
            /// <summary>
            /// Uruguay Peso en Unidades Indexadas (URUIURUI) (funds code)
            /// </summary>
            UYI = 940,
            /// <summary>
            /// Uruguayan Peso
            /// </summary>
            UYU = 858,
            /// <summary>
            /// Uzbekistan Som
            /// </summary>
            UZS = 860,
            /// <summary>
            /// Venezuelan Bolívar
            /// </summary>
            VEF = 937,
            /// <summary>
            /// Vietnamese Đồng
            /// </summary>
            VND = 704,
            /// <summary>
            /// Vanuatu Vatu
            /// </summary>
            VUV = 548,
            /// <summary>
            /// Samoan Tālā
            /// </summary>
            WST = 882,
            /// <summary>
            /// CFA Franc BEAC
            /// </summary>
            XAF = 950,
            /// <summary>
            /// Silver (one troy ounce)
            /// </summary>
            XAG = 961,
            /// <summary>
            /// Gold (one troy ounce)
            /// </summary>
            XAU = 959,
            /// <summary>
            /// Bond Markets Unit European Composite Unit (EURCO)
            /// </summary>
            XBA = 955,
            /// <summary>
            /// Bond Markets Unit European Monetary Unit (E.M.U.-6)
            /// </summary>
            XBB = 956,
            /// <summary>
            /// Bond Markets Unit European Unit of Account 9 (E.U.A.-9)
            /// </summary>
            XBC = 957,
            /// <summary>
            /// Bond Markets Unit European Unit of Account 17 (E.U.A.-17)
            /// </summary>
            XBD = 958,
            /// <summary>
            /// East Caribbean Dollar
            /// </summary>
            XCD = 951,
            /// <summary>
            /// Special Drawing Rights
            /// </summary>
            XDR = 960,
            /// <summary>
            /// CFA Franc BCEAO
            /// </summary>
            XOF = 952,
            /// <summary>
            /// Palladium (one troy ounce)
            /// </summary>
            XPD = 964,
            /// <summary>
            /// CFP Franc
            /// </summary>
            XPF = 953,
            /// <summary>
            /// Platinum (one troy ounce)
            /// </summary>
            XPT = 962,
            /// <summary>
            /// SUCRE
            /// </summary>
            XSU = 994,
            /// <summary>
            /// Codes specifically reserved for testing purposes
            /// </summary>
            XTS = 963,
            /// <summary>
            /// ADB Unit of Account
            /// </summary>
            XUA = 965,
            /// <summary>
            /// The code assigned for transactions where no currency is involved
            /// </summary>
            XXX = 999,
            /// <summary>
            /// Yemeni Rial
            /// </summary>
            YER = 886,
            /// <summary>
            /// South African Rand
            /// </summary>
            ZAR = 710,
            /// <summary>
            /// Zambian Kwacha
            /// </summary>
            ZMW = 967,
            /// <summary>
            /// Zimbabwean Dollar
            /// </summary>
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
