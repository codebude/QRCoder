namespace QRCoder2.Payloads
{
    public class UrlPayload : PayloadBase
    {
        private readonly string url;

        /// <summary>
        /// Generates a link. If not given, http/https protocol will be added.
        /// </summary>
        /// <param name="url">Link url target</param>
        public UrlPayload(string url)
        {
            this.url = url;
        }

        public override string ToString()
        {
            return (!this.url.StartsWith("http") ? "http://" + this.url : this.url);
        }
    }
}