using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using QRCoder;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace QRCoderDemoUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            comboBoxECC.SelectedIndex = 0;
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxECC.SelectedItem != null)
            {
                //Create generator
                string level = comboBoxECC.SelectedItem.ToString();
                QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);

                //Create raw qr code data
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", eccLevel);

                //Create byte/raw bitmap qr code
                BitmapByteQRCode qrCodeBmp = new BitmapByteQRCode(qrCodeData);
                byte[] qrCodeImageBmp = qrCodeBmp.GetGraphic(20, new byte[] { 118, 126, 152 },new byte[] { 144, 201, 111 });
                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
                    {
                        writer.WriteBytes(qrCodeImageBmp);
                        await writer.StoreAsync();
                    }
                    var image = new BitmapImage();
                    await image.SetSourceAsync(stream);

                    imageViewerBmp.Source = image;                    
                }

                //Create byte/raw png qr code
                PngByteQRCode qrCodePng = new PngByteQRCode(qrCodeData);
                byte[] qrCodeImagePng = qrCodePng.GetGraphic(20, new byte[] { 144, 201, 111 }, new byte[] { 118, 126, 152 });
                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
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

        public List<string> EccModes
        {
            get
            {
                return Enum.GetValues(typeof(QRCodeGenerator.ECCLevel)).Cast<Enum>().Select(x => x.ToString()).ToList();
            }
        }
    }
}

