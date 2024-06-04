namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a geo location payload. Supports raw location (GEO encoding) or Google Maps link (GoogleMaps encoding).
    /// </summary>
    public class Geolocation : Payload
    {
        private readonly string latitude, longitude;
        private readonly GeolocationEncoding encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="Geolocation"/> class.
        /// Supports raw location (GEO encoding) or Google Maps link (GoogleMaps encoding).
        /// </summary>
        /// <param name="latitude">Latitude with . as splitter.</param>
        /// <param name="longitude">Longitude with . as splitter.</param>
        /// <param name="encoding">Encoding type - GEO or GoogleMaps.</param>
        public Geolocation(string latitude, string longitude, GeolocationEncoding encoding = GeolocationEncoding.GEO)
        {
            this.latitude = latitude.Replace(",", ".");
            this.longitude = longitude.Replace(",", ".");
            this.encoding = encoding;
        }

        /// <summary>
        /// Returns a string representation of the geolocation payload.
        /// </summary>
        /// <returns>A string representation of the geolocation payload in the specified encoding format.</returns>
        public override string ToString()
        {
            switch (this.encoding)
            {
                case GeolocationEncoding.GEO:
                    return $"geo:{this.latitude},{this.longitude}";
                case GeolocationEncoding.GoogleMaps:
                    return $"http://maps.google.com/maps?q={this.latitude},{this.longitude}";
                default:
                    return "geo:";
            }
        }

        /// <summary>
        /// Defines the encoding types for geolocation payloads.
        /// </summary>
        public enum GeolocationEncoding
        {
            /// <summary>
            /// GEO encoding type.
            /// </summary>
            GEO,

            /// <summary>
            /// Google Maps encoding type.
            /// </summary>
            GoogleMaps
        }
    }
}
