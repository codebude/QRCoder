using System;

namespace QRCoder
{
    public static partial class PayloadGenerator
    {
        /// <summary>
        /// Generates a vCard or meCard contact dataset.
        /// </summary>
        public class ContactData : Payload
        {
            private readonly string firstname;
            private readonly string lastname;
            private readonly string nickname;
            private readonly string org;
            private readonly string orgTitle;
            private readonly string phone;
            private readonly string mobilePhone;
            private readonly string workPhone;
            private readonly string email;
            private readonly DateTime? birthday;
            private readonly string website;
            private readonly string street;
            private readonly string houseNumber;
            private readonly string city;
            private readonly string zipCode;
            private readonly string stateRegion;
            private readonly string country;
            private readonly string note;
            private readonly ContactOutputType outputType;
            private readonly AddressOrder addressOrder;


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
            public ContactData(ContactOutputType outputType, string firstname, string lastname, string nickname = null, string phone = null, string mobilePhone = null, string workPhone = null, string email = null, DateTime? birthday = null, string website = null, string street = null, string houseNumber = null, string city = null, string zipCode = null, string country = null, string note = null, string stateRegion = null, AddressOrder addressOrder = AddressOrder.Default, string org = null, string orgTitle = null)
            {
                this.firstname = firstname;
                this.lastname = lastname;
                this.nickname = nickname;
                this.org = org;
                this.orgTitle = orgTitle;
                this.phone = phone;
                this.mobilePhone = mobilePhone;
                this.workPhone = workPhone;
                this.email = email;
                this.birthday = birthday;
                this.website = website;
                this.street = street;
                this.houseNumber = houseNumber;
                this.city = city;
                this.stateRegion = stateRegion;
                this.zipCode = zipCode;
                this.country = country;
                this.addressOrder = addressOrder;
                this.note = note;
                this.outputType = outputType;
            }

            /// <summary>
            /// Returns a string representation of the contact data payload.
            /// </summary>
            /// <returns>A string representation of the contact data in the specified format.</returns>
            public override string ToString()
            {
                string payload = string.Empty;
                if (outputType == ContactOutputType.MeCard)
                {
                    payload += "MECARD+\r\n";
                    if (!string.IsNullOrEmpty(firstname) && !string.IsNullOrEmpty(lastname))
                        payload += $"N:{lastname}, {firstname}\r\n";
                    else if (!string.IsNullOrEmpty(firstname) || !string.IsNullOrEmpty(lastname))
                        payload += $"N:{firstname}{lastname}\r\n";
                    if (!string.IsNullOrEmpty(org))
                        payload += $"ORG:{org}\r\n";
                    if (!string.IsNullOrEmpty(orgTitle))
                        payload += $"TITLE:{orgTitle}\r\n";
                    if (!string.IsNullOrEmpty(phone))
                        payload += $"TEL:{phone}\r\n";
                    if (!string.IsNullOrEmpty(mobilePhone))
                        payload += $"TEL:{mobilePhone}\r\n";
                    if (!string.IsNullOrEmpty(workPhone))
                        payload += $"TEL:{workPhone}\r\n";
                    if (!string.IsNullOrEmpty(email))
                        payload += $"EMAIL:{email}\r\n";
                    if (!string.IsNullOrEmpty(note))
                        payload += $"NOTE:{note}\r\n";
                    if (birthday != null)
                        payload += $"BDAY:{((DateTime)birthday).ToString("yyyyMMdd")}\r\n";
                    string addressString = string.Empty;
                    if(addressOrder == AddressOrder.Default)
                    {
                        addressString = $"ADR:,,{(!string.IsNullOrEmpty(street) ? street + " " : "")}{(!string.IsNullOrEmpty(houseNumber) ? houseNumber : "")},{(!string.IsNullOrEmpty(zipCode) ? zipCode : "")},{(!string.IsNullOrEmpty(city) ? city : "")},{(!string.IsNullOrEmpty(stateRegion) ? stateRegion : "")},{(!string.IsNullOrEmpty(country) ? country : "")}\r\n";
                    }
                    else
                    {
                        addressString = $"ADR:,,{(!string.IsNullOrEmpty(houseNumber) ? houseNumber + " " : "")}{(!string.IsNullOrEmpty(street) ? street : "")},{(!string.IsNullOrEmpty(city) ? city : "")},{(!string.IsNullOrEmpty(stateRegion) ? stateRegion : "")},{(!string.IsNullOrEmpty(zipCode) ? zipCode : "")},{(!string.IsNullOrEmpty(country) ? country : "")}\r\n";
                    }
                    payload += addressString;
                    if (!string.IsNullOrEmpty(website))
                        payload += $"URL:{website}\r\n";
                    if (!string.IsNullOrEmpty(nickname))
                        payload += $"NICKNAME:{nickname}\r\n";
                    payload = payload.Trim(new char[] { '\r', '\n' });
                }
                else
                {
                    var version = outputType.ToString().Substring(5);
                    if (version.Length > 1)
                        version = version.Insert(1, ".");
                    else
                        version += ".0";

                    payload += "BEGIN:VCARD\r\n";
                    payload += $"VERSION:{version}\r\n";

                    payload += $"N:{(!string.IsNullOrEmpty(lastname) ? lastname : "")};{(!string.IsNullOrEmpty(firstname) ? firstname : "")};;;\r\n";
                    payload += $"FN:{(!string.IsNullOrEmpty(firstname) ? firstname + " " : "")}{(!string.IsNullOrEmpty(lastname) ? lastname : "")}\r\n";
                    if (!string.IsNullOrEmpty(org))
                    {
                        payload += $"ORG:" + org + "\r\n";
                    }
                    if (!string.IsNullOrEmpty(orgTitle))
                    {
                        payload += $"TITLE:" + orgTitle + "\r\n";
                    }
                    if (!string.IsNullOrEmpty(phone))
                    {
                        payload += $"TEL;";
                        if (outputType == ContactOutputType.VCard21)
                            payload += $"HOME;VOICE:{phone}";
                        else if (outputType == ContactOutputType.VCard3)
                            payload += $"TYPE=HOME,VOICE:{phone}";
                        else
                            payload += $"TYPE=home,voice;VALUE=uri:tel:{phone}";
                        payload += "\r\n";
                    }

                    if (!string.IsNullOrEmpty(mobilePhone))
                    {
                        payload += $"TEL;";
                        if (outputType == ContactOutputType.VCard21)
                            payload += $"HOME;CELL:{mobilePhone}";
                        else if (outputType == ContactOutputType.VCard3)
                            payload += $"TYPE=HOME,CELL:{mobilePhone}";
                        else
                            payload += $"TYPE=home,cell;VALUE=uri:tel:{mobilePhone}";
                        payload += "\r\n";
                    }

                    if (!string.IsNullOrEmpty(workPhone))
                    {
                        payload += $"TEL;";
                        if (outputType == ContactOutputType.VCard21)
                            payload += $"WORK;VOICE:{workPhone}";
                        else if (outputType == ContactOutputType.VCard3)
                            payload += $"TYPE=WORK,VOICE:{workPhone}";
                        else
                            payload += $"TYPE=work,voice;VALUE=uri:tel:{workPhone}";
                        payload += "\r\n";
                    }


                    payload += "ADR;";
                    if (outputType == ContactOutputType.VCard21)
                        payload += "HOME;PREF:";
                    else if (outputType == ContactOutputType.VCard3)
                        payload += "TYPE=HOME,PREF:";
                    else
                        payload += "TYPE=home,pref:";
                    string addressString = string.Empty;
                    if(addressOrder == AddressOrder.Default)
                    {
                        addressString = $";;{(!string.IsNullOrEmpty(street) ? street + " " : "")}{(!string.IsNullOrEmpty(houseNumber) ? houseNumber : "")};{(!string.IsNullOrEmpty(zipCode) ? zipCode : "")};{(!string.IsNullOrEmpty(city) ? city : "")};{(!string.IsNullOrEmpty(stateRegion) ? stateRegion : "")};{(!string.IsNullOrEmpty(country) ? country : "")}\r\n";
                    }
                    else
                    {
                        addressString = $";;{(!string.IsNullOrEmpty(houseNumber) ? houseNumber + " " : "")}{(!string.IsNullOrEmpty(street) ? street : "")};{(!string.IsNullOrEmpty(city) ? city : "")};{(!string.IsNullOrEmpty(stateRegion) ? stateRegion : "")};{(!string.IsNullOrEmpty(zipCode) ? zipCode : "")};{(!string.IsNullOrEmpty(country) ? country : "")}\r\n";
                    }
                    payload += addressString;

                    if (birthday != null)
                        payload += $"BDAY:{((DateTime)birthday).ToString("yyyyMMdd")}\r\n";
                    if (!string.IsNullOrEmpty(website))
                        payload += $"URL:{website}\r\n";
                    if (!string.IsNullOrEmpty(email))
                        payload += $"EMAIL:{email}\r\n";
                    if (!string.IsNullOrEmpty(note))
                        payload += $"NOTE:{note}\r\n";
                    if (outputType != ContactOutputType.VCard21 && !string.IsNullOrEmpty(nickname))
                        payload += $"NICKNAME:{nickname}\r\n";

                    payload += "END:VCARD";
                }

                return payload;
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
}
