namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private struct CodewordBlock
        {
            public CodewordBlock(byte[] codeWords, byte[] eccWords)
            {
                this.CodeWords = codeWords;
                this.ECCWords = eccWords;
            }

            public byte[] CodeWords { get; }
            public byte[] ECCWords { get; }
        }
    }
}
