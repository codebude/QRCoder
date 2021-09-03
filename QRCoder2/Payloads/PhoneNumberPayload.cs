namespace QRCoder2.Payloads
{
    public class PhoneNumberPayload : PayloadBase
    {
        private readonly string number;

        /// <summary>
        /// Generates a phone call payload
        /// </summary>
        /// <param name="number">Phonenumber of the receiver</param>
        public PhoneNumberPayload(string number)
        {
            this.number = number;
        }

        public override string ToString()
        {
            return $"tel:{this.number}";
        }
    }
}