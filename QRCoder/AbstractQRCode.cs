namespace QRCoder
{
    public abstract class AbstractQRCode<T>
    {
        protected QRCodeData QrCodeData { get; set; }
        
        protected AbstractQRCode(QRCodeData data) {
            this.QrCodeData = data;
        }
        
        public abstract T GetGraphic(int pixelsPerModule);
    }
}