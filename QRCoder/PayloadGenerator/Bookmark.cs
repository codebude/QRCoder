namespace QRCoder
{
    public static partial class PayloadGenerator
    {
        /// <summary>
        /// Generates a bookmark payload. When scanned by a QR code reader, this creates a browser bookmark.
        /// </summary>
        public class Bookmark : Payload
        {
            private readonly string url, title;

            /// <summary>
            /// Initializes a new instance of the <see cref="Bookmark"/> class.
            /// </summary>
            /// <param name="url">The URL of the bookmark.</param>
            /// <param name="title">The title of the bookmark.</param>
            public Bookmark(string url, string title)
            {
                this.url = EscapeInput(url);
                this.title = EscapeInput(title);
            }

            /// <summary>
            /// Returns a string representation of the bookmark payload.
            /// </summary>
            /// <returns>A string representation of the bookmark payload in the MEBKM format.</returns>
            public override string ToString()
            {
                return $"MEBKM:TITLE:{this.title};URL:{this.url};;";
            }
        }
    }
}
