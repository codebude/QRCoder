using System.Collections.Generic;

namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private struct VersionInfo
        {
            public VersionInfo(int version, List<VersionInfoDetails> versionInfoDetails)
            {
                this.Version = version;
                this.Details = versionInfoDetails;
            }
            public int Version { get; }
            public List<VersionInfoDetails> Details { get; }
        }
    }
}
