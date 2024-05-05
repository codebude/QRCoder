using System.Collections.Generic;

namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private struct VersionInfoDetails
        {
            public VersionInfoDetails(ECCLevel errorCorrectionLevel, Dictionary<EncodingMode, int> capacityDict)
            {
                this.ErrorCorrectionLevel = errorCorrectionLevel;
                this.CapacityDict = capacityDict;
            }

            public ECCLevel ErrorCorrectionLevel { get; }
            public Dictionary<EncodingMode, int> CapacityDict { get; }
        }
    }
}
