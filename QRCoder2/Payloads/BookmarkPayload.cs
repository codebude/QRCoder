namespace QRCoder2.Payloads
{
    public class BookmarkPayload : PayloadBase
    {
        private readonly string url, title;

        /// <summary>
        /// Generates a bookmark payload. Scanned by an QR Code reader, this one creates a browser bookmark.
        /// </summary>
        /// <param name="url">Url of the bookmark</param>
        /// <param name="title">Title of the bookmark</param>
        public BookmarkPayload(string url, string title)
        {
            this.url = EscapeInput(url);
            this.title = EscapeInput(title);
        }

        public override string ToString()
        {
            return $"MEBKM:TITLE:{this.title};URL:{this.url};;";
        }
    }
}