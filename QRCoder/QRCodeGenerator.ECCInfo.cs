namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private struct ECCInfo
        {
            public ECCInfo(int version, ECCLevel errorCorrectionLevel, int totalDataCodewords, int eccPerBlock, int blocksInGroup1,
                int codewordsInGroup1, int blocksInGroup2, int codewordsInGroup2)
            {
                this.Version = version;
                this.ErrorCorrectionLevel = errorCorrectionLevel;
                this.TotalDataCodewords = totalDataCodewords;
                this.ECCPerBlock = eccPerBlock;
                this.BlocksInGroup1 = blocksInGroup1;
                this.CodewordsInGroup1 = codewordsInGroup1;
                this.BlocksInGroup2 = blocksInGroup2;
                this.CodewordsInGroup2 = codewordsInGroup2;
            }
            public int Version { get; }
            public ECCLevel ErrorCorrectionLevel { get; }
            public int TotalDataCodewords { get; }
            public int ECCPerBlock { get; }
            public int BlocksInGroup1 { get; }
            public int CodewordsInGroup1 { get; }
            public int BlocksInGroup2 { get; }
            public int CodewordsInGroup2 { get; }
        }
    }
}
