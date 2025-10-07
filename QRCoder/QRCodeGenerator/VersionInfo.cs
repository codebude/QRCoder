namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Represents version-specific information of a QR code.
    /// </summary>
    private struct VersionInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionInfo"/> struct with a specific version number and its details.
        /// </summary>
        /// <param name="version">The version number of the QR code. Each version has a different module configuration.</param>
        /// <param name="versionInfoDetails">A list of detailed information related to error correction levels and capacity for each encoding mode.</param>
        public VersionInfo(int version, List<VersionInfoDetails> versionInfoDetails)
        {
            Version = version;
            Details = versionInfoDetails;
        }

        /// <summary>
        /// Gets the version number of the QR code. Each version number specifies a different size of the QR matrix.
        /// </summary>
        public int Version { get; }

        /// <summary>
        /// Gets a list of details about the QR code version, including the error correction levels and encoding capacities.
        /// </summary>
        public List<VersionInfoDetails> Details { get; }
    }
}
