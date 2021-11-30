namespace QRCoder2.Payloads
{
    public class PlainTextPayload : PayloadBase
    {
        private readonly string text;

        /// <summary>
        /// Generates a plain text payload
        /// </summary>
        /// <param name="text"></param>
        public PlainTextPayload(string text)
        {
            this.text = text;
        }

        public override string ToString()
        {
            return text;
        }
    }
}