using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qr_P_Antilog_GenPolynom
{
    class Program
    {
        static void Main(string[] args)
        {            
            QRCoder qr = new QRCoder();

            var qrCode = qr.CreateQrCode("Dies ist ein QR Code Test!", QRCoder.ECCLevel.Q);
            //var qrCode = qr.CreateQrCode("HELLO WORLD", QRCoder.ECCLevel.Q);
            //var qrCode = qr.CreateQrCode("test!#'K", QRCoder.ECCLevel.H);
         
            Form previewForm = new Form();
            previewForm.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            previewForm.Size = new Size(665,525);
            previewForm.BackColor = Color.NavajoWhite;
            PictureBox pb = new PictureBox();
            pb.Dock = DockStyle.Fill;
            pb.Size = new Size(640, 480);
            pb.Location = new Point(5, 5);
            pb.BackgroundImageLayout = ImageLayout.Zoom;

            pb.BackgroundImage = qrCode.GetGraphic(20);

            previewForm.Controls.Add(pb);
            previewForm.ShowDialog();

            Console.WriteLine();
            Console.Read();
        }

        
        
    }

    
}
