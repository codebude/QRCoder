namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a geo location payload. Supports raw location (GEO encoding) or Google Maps link (GoogleMaps encoding).
    /// </summary>
    public class Geolocation : Payload
    {
        private readonly string _latitude, _longitude;
        private readonly GeolocationEncoding _encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="Geolocation"/> class.
        /// Supports raw location (GEO encoding) or Google Maps link (GoogleMaps encoding).
        /// </summary>
        /// <param name="latitude">Latitude with . as splitter.</param>
        /// <param name="longitude">Longitude with . as splitter.</param>
        /// <param name="encoding">Encoding type - GEO or GoogleMaps.</param>
        public Geolocation(string latitude, string longitude, GeolocationEncoding encoding = GeolocationEncoding.GEO)
        {
            _latitude = latitude.Replace(",", ".");
            _longitude = longitude.Replace(",", ".");
            _encoding = encoding;
        }

        /// <summary>
        /// Returns a string representation of the geolocation payload.
        /// </summary>
        /// <returns>A string representation of the geolocation payload in the specified encoding format.</returns>
        public override string ToString() => _encoding switch
        {
            GeolocationEncoding.GEO => $"geo:{_latitude},{_longitude}",
            GeolocationEncoding.GoogleMaps => $"http://maps.google.com/maps?q={_latitude},{_longitude}",
            _ => "geo:",
        };

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
