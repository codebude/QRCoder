namespace QRCoder2.Payloads
{
    public class SkypeCallPayload : PayloadBase
    {
        private readonly string skypeUsername;

        /// <summary>
        /// Generates a Skype call payload
        /// </summary>
        /// <param name="skypeUsername">Skype username which will be called</param>
        public SkypeCallPayload(string skypeUsername)
        {
            this.skypeUsername = skypeUsername;
        }

        public override string ToString()
        {
            return $"skype:{this.skypeUsername}?call";
        }
    }
}