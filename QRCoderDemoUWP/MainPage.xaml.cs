using System;
using System.Collections.Generic;
using System.Linq;
using QRCoder;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace QRCoderDemoUWP;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
        DataContext = this;
        comboBoxECC.SelectedIndex = 0;
    }

#pragma warning disable IDE1006 // Naming Styles
    private async void button_Click(object sender, RoutedEventArgs e)
#pragma warning restore IDE1006 // Naming Styles
    {
        if (comboBoxECC.SelectedItem != null)
        {
            //Create generator
            string level = comboBoxECC.SelectedItem.ToString();
            var eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);

            //Create raw qr code data
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", eccLevel);

            //Create byte/raw bitmap qr code
            var qrCodeBmp = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeImageBmp = qrCodeBmp.GetGraphic(20, new byte[] { 118, 126, 152 }, new byte[] { 144, 201, 111 });
            using (var stream = new InMemoryRandomAccessStream())
            {
                using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(qrCodeImageBmp);
                    await writer.StoreAsync();
                }
                var image = new BitmapImage();
                await image.SetSourceAsync(stream);

                imageViewerBmp.Source = image;
            }

            //Create byte/raw png qr code
            var qrCodePng = new PngByteQRCode(qrCodeData);
            byte[] qrCodeImagePng = qrCodePng.GetGraphic(20, new byte[] { 144, 201, 111 }, new byte[] { 118, 126, 152 });
            using (var stream = new InMemoryRandomAccessStream())
            {
                using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(qrCodeImagePng);
                    await writer.StoreAsync();
                }
                var image = new BitmapImage();
                await image.SetSourceAsync(stream);

                imageViewerPng.Source = image;
            }
        }
    }

    public List<string> EccModes => Enum.GetValues(typeof(QRCodeGenerator.ECCLevel)).Cast<Enum>().Select(x => x.ToString()).ToList();
}

