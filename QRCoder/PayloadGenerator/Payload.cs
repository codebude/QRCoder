namespace QRCoder
{
    public static partial class PayloadGenerator
    {
        public abstract class Payload
        {
            public virtual int Version { get { return -1; } }
            public virtual QRCodeGenerator.ECCLevel EccLevel { get { return QRCodeGenerator.ECCLevel.Default; } }
            public virtual QRCodeGenerator.EciMode EciMode { get { return QRCodeGenerator.EciMode.Default; } }
            public abstract override string ToString();
        }
    }
}
