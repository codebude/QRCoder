using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
#if !NETCOREAPP1_1
using System.Drawing;
#endif

namespace QRCoderTests.Helpers
{
    public static class HelperFunctions
    {

#if !NETCOREAPP1_1
        public static string GetAssemblyPath()
        {
            return
#if NET5_0
                AppDomain.CurrentDomain.BaseDirectory;
#else
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "");
#endif
        }
#endif


#if !NETCOREAPP1_1
        public static string BitmapToHash(Bitmap bmp)
        {
            byte[] imgBytes = null;
            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imgBytes = ms.ToArray();
                ms.Dispose();
            }
            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(imgBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
#endif

    }
}
