namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a vCard or meCard contact dataset.
    /// </summary>
    public class ContactData : Payload
    {
        private static readonly char[] _trimChars = { '\r', '\n' };
        private readonly string _firstname;
        private readonly string _lastname;
        private readonly string? _nickname;
        private readonly string? _org;
        private readonly string? _orgTitle;
        private readonly string? _phone;
        private readonly string? _mobilePhone;
        private readonly string? _workPhone;
        private readonly string? _email;
        private readonly DateTime? _birthday;
        private readonly string? _website;
        private readonly string? _street;
        private readonly string? _houseNumber;
        private readonly string? _city;
        private readonly string? _zipCode;
        private readonly string? _stateRegion;
        private readonly string? _country;
        private readonly string? _note;
        private readonly ContactOutputType _outputType;
        private readonly AddressOrder _addressOrder;
        private readonly AddressType _addressType;


        /// <summary>
        /// Initializes a new instance of the <see cref="ContactData"/> class.
        /// </summary>
        /// <param name="outputType">Payload output type.</param>
        /// <param name="firstname">The first name.</param>
        /// <param name="lastname">The last name.</param>
        /// <param name="nickname">The display name.</param>
        /// <param name="phone">Normal phone number.</param>
        /// <param name="mobilePhone">Mobile phone.</param>
        /// <param name="workPhone">Office phone number.</param>
        /// <param name="email">E-Mail address.</param>
        /// <param name="birthday">Birthday.</param>
        /// <param name="website">Website / Homepage.</param>
        /// <param name="street">Street.</param>
        /// <param name="houseNumber">House number.</param>
        /// <param name="city">City.</param>
        /// <param name="stateRegion">State or Region.</param>
        /// <param name="zipCode">Zip code.</param>
        /// <param name="country">Country.</param>
        /// <param name="addressOrder">The address order format to use.</param>
        /// <param name="note">Memo text / notes.</param>
        /// <param name="org">Organization/Company.</param>
        /// <param name="orgTitle">Organization/Company Title.</param>
        public ContactData(ContactOutputType outputType, string firstname, string lastname, string? nickname = null, string? phone = null, string? mobilePhone = null, string? workPhone = null, string? email = null, DateTime? birthday = null, string? website = null, string? street = null, string? houseNumber = null, string? city = null, string? zipCode = null, string? country = null, string? note = null, string? stateRegion = null, AddressOrder addressOrder = AddressOrder.Default, string? org = null, string? orgTitle = null)
            : this(outputType, firstname, lastname, nickname, phone, mobilePhone, workPhone, email, birthday, website, street, houseNumber, city, zipCode, country, note, stateRegion, addressOrder, org, orgTitle, AddressType.HomePreferred)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactData"/> class with address type support.
        /// </summary>
        /// <param name="outputType">Payload output type.</param>
        /// <param name="firstname">The first name.</param>
        /// <param name="lastname">The last name.</param>
        /// <param name="nickname">The display name.</param>
        /// <param name="phone">Normal phone number.</param>
        /// <param name="mobilePhone">Mobile phone.</param>
        /// <param name="workPhone">Office phone number.</param>
        /// <param name="email">E-Mail address.</param>
        /// <param name="birthday">Birthday.</param>
        /// <param name="website">Website / Homepage.</param>
        /// <param name="street">Street.</param>
        /// <param name="houseNumber">House number.</param>
        /// <param name="city">City.</param>
        /// <param name="stateRegion">State or Region.</param>
        /// <param name="zipCode">Zip code.</param>
        /// <param name="country">Country.</param>
        /// <param name="addressOrder">The address order format to use.</param>
        /// <param name="note">Memo text / notes.</param>
        /// <param name="org">Organization/Company.</param>
        /// <param name="orgTitle">Organization/Company Title.</param>
        /// <param name="addressType">The address type (Home, Work, HomePreferred, or WorkPreferred).</param>
        public ContactData(ContactOutputType outputType, string firstname, string lastname, string? nickname, string? phone, string? mobilePhone, string? workPhone, string? email, DateTime? birthday, string? website, string? street, string? houseNumber, string? city, string? zipCode, string? country, string? note, string? stateRegion, AddressOrder addressOrder, string? org, string? orgTitle, AddressType addressType)
        {
            _firstname = firstname;
            _lastname = lastname;
            _nickname = nickname;
            _org = org;
            _orgTitle = orgTitle;
            _phone = phone;
            _mobilePhone = mobilePhone;
            _workPhone = workPhone;
            _email = email;
            _birthday = birthday;
            _website = website;
            _street = street;
            _houseNumber = houseNumber;
            _city = city;
            _stateRegion = stateRegion;
            _zipCode = zipCode;
            _country = country;
            _addressOrder = addressOrder;
            _note = note;
            _outputType = outputType;
            _addressType = addressType;
        }

        /// <summary>
        /// Returns a string representation of the contact data payload.
        /// </summary>
        /// <returns>A string representation of the contact data in the specified format.</returns>
        public override string ToString()
        {
            string payload = string.Empty;
            if (_outputType == ContactOutputType.MeCard)
            {
                payload += "MECARD+\r\n";
                if (!string.IsNullOrEmpty(_firstname) && !string.IsNullOrEmpty(_lastname))
                    payload += $"N:{_lastname}, {_firstname}\r\n";
                else if (!string.IsNullOrEmpty(_firstname) || !string.IsNullOrEmpty(_lastname))
                    payload += $"N:{_firstname}{_lastname}\r\n";
                if (!string.IsNullOrEmpty(_org))
                    payload += $"ORG:{_org}\r\n";
                if (!string.IsNullOrEmpty(_orgTitle))
                    payload += $"TITLE:{_orgTitle}\r\n";
                if (!string.IsNullOrEmpty(_phone))
                    payload += $"TEL:{_phone}\r\n";
                if (!string.IsNullOrEmpty(_mobilePhone))
                    payload += $"TEL:{_mobilePhone}\r\n";
                if (!string.IsNullOrEmpty(_workPhone))
                    payload += $"TEL:{_workPhone}\r\n";
                if (!string.IsNullOrEmpty(_email))
                    payload += $"EMAIL:{_email}\r\n";
                if (!string.IsNullOrEmpty(_note))
                    payload += $"NOTE:{_note}\r\n";
                if (_birthday != null)
                    payload += $"BDAY:{((DateTime)_birthday).ToString("yyyyMMdd", CultureInfo.InvariantCulture)}\r\n";
                // RFC 2426 Section 3.2.1: ADR format is PO Box; Extended Address; Street; Locality (City); Region; Postal Code; Country
                string addressString = string.Empty;
                if (_addressOrder == AddressOrder.Default)
                {
                    addressString = $"ADR:,,{(!string.IsNullOrEmpty(_street) ? _street + " " : "")}{(!string.IsNullOrEmpty(_houseNumber) ? _houseNumber : "")},{(!string.IsNullOrEmpty(_city) ? _city : "")},{(!string.IsNullOrEmpty(_stateRegion) ? _stateRegion : "")},{(!string.IsNullOrEmpty(_zipCode) ? _zipCode : "")},{(!string.IsNullOrEmpty(_country) ? _country : "")}\r\n";
                }
                else
                {
                    addressString = $"ADR:,,{(!string.IsNullOrEmpty(_houseNumber) ? _houseNumber + " " : "")}{(!string.IsNullOrEmpty(_street) ? _street : "")},{(!string.IsNullOrEmpty(_city) ? _city : "")},{(!string.IsNullOrEmpty(_stateRegion) ? _stateRegion : "")},{(!string.IsNullOrEmpty(_zipCode) ? _zipCode : "")},{(!string.IsNullOrEmpty(_country) ? _country : "")}\r\n";
                }
                payload += addressString;
                if (!string.IsNullOrEmpty(_website))
                    payload += $"URL:{_website}\r\n";
                if (!string.IsNullOrEmpty(_nickname))
                    payload += $"NICKNAME:{_nickname}\r\n";
                payload = payload.Trim(_trimChars);
            }
            else
            {
                var version = _outputType.ToString().Substring(5);
                if (version.Length > 1)
                    version = version.Insert(1, ".");
                else
                    version += ".0";

                payload += "BEGIN:VCARD\r\n";
                payload += $"VERSION:{version}\r\n";

                payload += $"N:{(!string.IsNullOrEmpty(_lastname) ? _lastname : "")};{(!string.IsNullOrEmpty(_firstname) ? _firstname : "")};;;\r\n";
                payload += $"FN:{(!string.IsNullOrEmpty(_firstname) ? _firstname + " " : "")}{(!string.IsNullOrEmpty(_lastname) ? _lastname : "")}\r\n";
                if (!string.IsNullOrEmpty(_org))
                {
                    payload += $"ORG:" + _org + "\r\n";
                }
                if (!string.IsNullOrEmpty(_orgTitle))
                {
                    payload += $"TITLE:" + _orgTitle + "\r\n";
                }
                if (!string.IsNullOrEmpty(_phone))
                {
                    payload += $"TEL;";
                    if (_outputType == ContactOutputType.VCard21)
                        payload += $"HOME;VOICE:{_phone}";
                    else if (_outputType == ContactOutputType.VCard3)
                        payload += $"TYPE=HOME,VOICE:{_phone}";
                    else
                        payload += $"TYPE=home,voice;VALUE=uri:tel:{_phone}";
                    payload += "\r\n";
                }

                if (!string.IsNullOrEmpty(_mobilePhone))
                {
                    payload += $"TEL;";
                    if (_outputType == ContactOutputType.VCard21)
                        payload += $"HOME;CELL:{_mobilePhone}";
                    else if (_outputType == ContactOutputType.VCard3)
                        payload += $"TYPE=HOME,CELL:{_mobilePhone}";
                    else
                        payload += $"TYPE=home,cell;VALUE=uri:tel:{_mobilePhone}";
                    payload += "\r\n";
                }

                if (!string.IsNullOrEmpty(_workPhone))
                {
                    payload += $"TEL;";
                    if (_outputType == ContactOutputType.VCard21)
                        payload += $"WORK;VOICE:{_workPhone}";
                    else if (_outputType == ContactOutputType.VCard3)
                        payload += $"TYPE=WORK,VOICE:{_workPhone}";
                    else
                        payload += $"TYPE=work,voice;VALUE=uri:tel:{_workPhone}";
                    payload += "\r\n";
                }


                // RFC 2426 Section 3.2.1: ADR format is PO Box; Extended Address; Street; Locality (City); Region; Postal Code; Country
                payload += "ADR;";
                if (_outputType == ContactOutputType.VCard21)
                    payload += GetAddressTypeString21() + ":";
                else if (_outputType == ContactOutputType.VCard3)
                    payload += "TYPE=" + GetAddressTypeString3() + ":";
                else
                    payload += "TYPE=" + GetAddressTypeString4() + ":";
                string addressString = string.Empty;
                if (_addressOrder == AddressOrder.Default)
                {
                    addressString = $";;{(!string.IsNullOrEmpty(_street) ? _street + " " : "")}{(!string.IsNullOrEmpty(_houseNumber) ? _houseNumber : "")};{(!string.IsNullOrEmpty(_city) ? _city : "")};{(!string.IsNullOrEmpty(_stateRegion) ? _stateRegion : "")};{(!string.IsNullOrEmpty(_zipCode) ? _zipCode : "")};{(!string.IsNullOrEmpty(_country) ? _country : "")}\r\n";
                }
                else
                {
                    addressString = $";;{(!string.IsNullOrEmpty(_houseNumber) ? _houseNumber + " " : "")}{(!string.IsNullOrEmpty(_street) ? _street : "")};{(!string.IsNullOrEmpty(_city) ? _city : "")};{(!string.IsNullOrEmpty(_stateRegion) ? _stateRegion : "")};{(!string.IsNullOrEmpty(_zipCode) ? _zipCode : "")};{(!string.IsNullOrEmpty(_country) ? _country : "")}\r\n";
                }
                payload += addressString;

                if (_birthday != null)
                    payload += $"BDAY:{((DateTime)_birthday).ToString("yyyyMMdd", CultureInfo.InvariantCulture)}\r\n";
                if (!string.IsNullOrEmpty(_website))
                    payload += $"URL:{_website}\r\n";
                if (!string.IsNullOrEmpty(_email))
                    payload += $"EMAIL:{_email}\r\n";
                if (!string.IsNullOrEmpty(_note))
                    payload += $"NOTE:{_note}\r\n";
                if (_outputType != ContactOutputType.VCard21 && !string.IsNullOrEmpty(_nickname))
                    payload += $"NICKNAME:{_nickname}\r\n";

                payload += "END:VCARD";
            }

            return payload;
        }

        /// <summary>
        /// Gets the address type string for vCard 2.1 format.
        /// </summary>
        private string GetAddressTypeString21()
        {
            return _addressType switch
            {
                AddressType.Home => "HOME",
                AddressType.Work => "WORK",
                AddressType.HomePreferred => "HOME;PREF",
                AddressType.WorkPreferred => "WORK;PREF",
                _ => "HOME;PREF"
            };
        }

        /// <summary>
        /// Gets the address type string for vCard 3.0 format.
        /// </summary>
        private string GetAddressTypeString3()
        {
            return _addressType switch
            {
                AddressType.Home => "HOME",
                AddressType.Work => "WORK",
                AddressType.HomePreferred => "HOME,PREF",
                AddressType.WorkPreferred => "WORK,PREF",
                _ => "HOME,PREF"
            };
        }

        /// <summary>
        /// Gets the address type string for vCard 4.0 format.
        /// </summary>
        private string GetAddressTypeString4()
        {
            return _addressType switch
            {
                AddressType.Home => "home",
                AddressType.Work => "work",
                AddressType.HomePreferred => "home,pref",
                AddressType.WorkPreferred => "work,pref",
                _ => "home,pref"
            };
        }

        /// <summary>
        /// Defines the type of address (home or work).
        /// </summary>
        public enum AddressType
        {
            /// <summary>
            /// Home address.
            /// </summary>
            Home,

            /// <summary>
            /// Work address.
            /// </summary>
            Work,

            /// <summary>
            /// Home address marked as preferred (default for backwards compatibility).
            /// </summary>
            HomePreferred,

            /// <summary>
            /// Work address marked as preferred.
            /// </summary>
            WorkPreferred
        }

        /// <summary>
        /// Possible output types. Either vCard 2.1, vCard 3.0, vCard 4.0 or MeCard.
        /// </summary>
        public enum ContactOutputType
        {
            /// <summary>
            /// MeCard output type.
            /// </summary>
            MeCard,

            /// <summary>
            /// vCard 2.1 output type.
            /// </summary>
            VCard21,

            /// <summary>
            /// vCard 3.0 output type.
            /// </summary>
            VCard3,

            /// <summary>
            /// vCard 4.0 output type.
            /// </summary>
            VCard4
        }


        /// <summary>
        /// Define the address format.
        /// Default: European format, ([Street] [House Number] and [Postal Code] [City])
        /// Reversed: North American and others format ([House Number] [Street] and [City] [Postal Code])
        /// </summary>
        public enum AddressOrder
        {
            /// <summary>
            /// Default address order format (European format).
            /// </summary>
            Default,

            /// <summary>
            /// Reversed address order format (North American and others format).
            /// </summary>
            Reversed
        }
    }
}
