namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a Slovenian UPN QR payment payload.
    /// </summary>
    public class SlovenianUpnQr : Payload
    {
        //Keep in mind, that the ECC level has to be set to "M", version to 15 and ECI to EciMode.Iso8859_2 when generating a SlovenianUpnQr!
        //SlovenianUpnQr specification: https://www.upn-qr.si/uploads/files/NavodilaZaProgramerjeUPNQR.pdf

        private readonly string _payerName = "";
        private readonly string _payerAddress = "";
        private readonly string _payerPlace = "";
        private readonly string _amount = "";
        private readonly string _code = "";
        private readonly string _purpose = "";
        private readonly string _deadLine = "";
        private readonly string _recipientIban = "";
        private readonly string _recipientName = "";
        private readonly string _recipientAddress = "";
        private readonly string _recipientPlace = "";
        private readonly string _recipientSiModel = "";
        private readonly string _recipientSiReference = "";

        /// <summary>
        /// Gets the version of the QR code, which is 15 for Slovenian UPN QR.
        /// </summary>
        public override int Version => 15;

        /// <summary>
        /// Gets the error correction level of the QR code, which is M for Slovenian UPN QR.
        /// </summary>
        public override QRCodeGenerator.ECCLevel EccLevel => QRCodeGenerator.ECCLevel.M;

        /// <summary>
        /// Gets the ECI mode of the QR code, which is Iso8859_2 for Slovenian UPN QR.
        /// </summary>
        public override QRCodeGenerator.EciMode EciMode => QRCodeGenerator.EciMode.Iso8859_2;

        /// <summary>
        /// Limits the length of a string to a specified maximum length.
        /// </summary>
        /// <param name="value">The string to limit.</param>
        /// <param name="maxLength">The maximum length of the string.</param>
        /// <returns>The limited string.</returns>
        private static string LimitLength(string value, int maxLength)
            => (value.Length <= maxLength) ? value : value.Substring(0, maxLength);

        /// <summary>
        /// Initializes a new instance of the <see cref="SlovenianUpnQr"/> class.
        /// </summary>
        /// <param name="payerName">The name of the payer.</param>
        /// <param name="payerAddress">The address of the payer.</param>
        /// <param name="payerPlace">The place of the payer.</param>
        /// <param name="recipientName">The name of the recipient.</param>
        /// <param name="recipientAddress">The address of the recipient.</param>
        /// <param name="recipientPlace">The place of the recipient.</param>
        /// <param name="recipientIban">The IBAN of the recipient.</param>
        /// <param name="description">The description of the payment.</param>
        /// <param name="amount">The amount of the payment.</param>
        /// <param name="recipientSiModel">The SI model of the recipient.</param>
        /// <param name="recipientSiReference">The SI reference of the recipient.</param>
        /// <param name="code">The code of the payment.</param>
        public SlovenianUpnQr(string payerName, string payerAddress, string payerPlace, string recipientName, string recipientAddress, string recipientPlace, string recipientIban, string description, double amount, string recipientSiModel = "SI00", string recipientSiReference = "", string code = "OTHR") :
            this(payerName, payerAddress, payerPlace, recipientName, recipientAddress, recipientPlace, recipientIban, description, amount, null, recipientSiModel, recipientSiReference, code)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlovenianUpnQr"/> class with a deadline.
        /// </summary>
        /// <param name="payerName">The name of the payer.</param>
        /// <param name="payerAddress">The address of the payer.</param>
        /// <param name="payerPlace">The place of the payer.</param>
        /// <param name="recipientName">The name of the recipient.</param>
        /// <param name="recipientAddress">The address of the recipient.</param>
        /// <param name="recipientPlace">The place of the recipient.</param>
        /// <param name="recipientIban">The IBAN of the recipient.</param>
        /// <param name="description">The description of the payment.</param>
        /// <param name="amount">The amount of the payment.</param>
        /// <param name="deadline">The deadline for the payment.</param>
        /// <param name="recipientSiModel">The SI model of the recipient.</param>
        /// <param name="recipientSiReference">The SI reference of the recipient.</param>
        /// <param name="code">The code of the payment.</param>
        public SlovenianUpnQr(string payerName, string payerAddress, string payerPlace, string recipientName, string recipientAddress, string recipientPlace, string recipientIban, string description, double amount, DateTime? deadline, string recipientSiModel = "SI99", string recipientSiReference = "", string code = "OTHR")
        {
            _payerName = SlovenianUpnQr.LimitLength(payerName.Trim(), 33);
            _payerAddress = SlovenianUpnQr.LimitLength(payerAddress.Trim(), 33);
            _payerPlace = SlovenianUpnQr.LimitLength(payerPlace.Trim(), 33);
            _amount = FormatAmount(amount);
            _code = SlovenianUpnQr.LimitLength(code.Trim().ToUpperInvariant(), 4);
            _purpose = SlovenianUpnQr.LimitLength(description.Trim(), 42);
            _deadLine = (deadline == null) ? "" : deadline.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            _recipientIban = SlovenianUpnQr.LimitLength(recipientIban.Trim(), 34);
            _recipientName = SlovenianUpnQr.LimitLength(recipientName.Trim(), 33);
            _recipientAddress = SlovenianUpnQr.LimitLength(recipientAddress.Trim(), 33);
            _recipientPlace = SlovenianUpnQr.LimitLength(recipientPlace.Trim(), 33);
            _recipientSiModel = SlovenianUpnQr.LimitLength(recipientSiModel.Trim().ToUpperInvariant(), 4);
            _recipientSiReference = SlovenianUpnQr.LimitLength(recipientSiReference.Trim(), 22);
        }


        /// <summary>
        /// Formats the amount as a string with leading zeros.
        /// </summary>
        /// <param name="amount">The amount to format.</param>
        /// <returns>The formatted amount string.</returns>
        private static string FormatAmount(double amount)
        {
            int _amt = (int)Math.Round(amount * 100.0);
            return string.Format(CultureInfo.InvariantCulture, "{0:00000000000}", _amt);
        }

        /// <summary>
        /// Calculates the checksum of the payment data.
        /// </summary>
        /// <returns>The checksum of the payment data.</returns>
        private int CalculateChecksum()
        {
            int _cs = 5 + _payerName.Length; //5 = UPNQR constant Length
            _cs += _payerAddress.Length;
            _cs += _payerPlace.Length;
            _cs += _amount.Length;
            _cs += _code.Length;
            _cs += _purpose.Length;
            _cs += _deadLine.Length;
            _cs += _recipientIban.Length;
            _cs += _recipientName.Length;
            _cs += _recipientAddress.Length;
            _cs += _recipientPlace.Length;
            _cs += _recipientSiModel.Length;
            _cs += _recipientSiReference.Length;
            _cs += 19;
            return _cs;
        }

        /// <summary>
        /// Returns the Slovenian UPN QR payment data as a string.
        /// </summary>
        /// <returns>The Slovenian UPN QR payment data as a string.</returns>
        public override string ToString()
        {
            var _sb = new StringBuilder();
            _sb.Append("UPNQR");
            _sb.Append('\n').Append('\n').Append('\n').Append('\n').Append('\n');
            _sb.Append(_payerName).Append('\n');
            _sb.Append(_payerAddress).Append('\n');
            _sb.Append(_payerPlace).Append('\n');
            _sb.Append(_amount).Append('\n').Append('\n').Append('\n');
            _sb.Append(_code.ToUpperInvariant()).Append('\n');
            _sb.Append(_purpose).Append('\n');
            _sb.Append(_deadLine).Append('\n');
            _sb.Append(_recipientIban.ToUpperInvariant()).Append('\n');
            _sb.Append(_recipientSiModel).Append(_recipientSiReference).Append('\n');
            _sb.Append(_recipientName).Append('\n');
            _sb.Append(_recipientAddress).Append('\n');
            _sb.Append(_recipientPlace).Append('\n');
            _sb.AppendFormat(CultureInfo.InvariantCulture, "{0:000}", CalculateChecksum()).Append('\n');
            return _sb.ToString();
        }
    }
}
