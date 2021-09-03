namespace QRCoder2.Renderers
{
    public abstract class QRCodeRendererBase
    {
        protected QRCodeData QrCodeData { get; set; }

        protected QRCodeRendererBase(QRCodeData data) 
        {
            this.QrCodeData = data;
        }
    }
}