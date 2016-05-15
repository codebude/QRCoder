using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NDesk.Options;
using System.Reflection;
using QRCoder;
using System.Text;

namespace QRCoderConsole
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            var friendlyName = AppDomain.CurrentDomain.FriendlyName;
            var newLine = Environment.NewLine;
            var setter = new OptionSetter ();

            String fileName = null, outputFileName = null, payload = null;

            QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.L;
            ImageFormat imageFormat = ImageFormat.Png;


            var showHelp = false;


            var optionSet = new OptionSet {
                {    "e|ecc-level=",
                    "error correction level",
                    value => eccLevel = setter.GetECCLevel(value)
                },
                {   "f|output-format=",
                    "Image format for outputfile. Possible values: png, jpg, gif, bmp, tiff (default: png)",
                    value => imageFormat = setter.GetImageFormat(value)
                },
                {
                    "i|in=",
                    "input file | alternative to parameter -p",
                    value => fileName = value
                },
                {
                    "p|payload=",
                    "payload string | alternative to parameter -i",
                    value => payload = value
                },
                {
                    "o|out=",
                    "output file",
                    value => outputFileName = value
                },
                {     "h|help",
                    "show this message and exit.",
                    value => showHelp = value != null
                }
            };

            try
            {

                var settings = optionSet.Parse(args);

                if (showHelp)
                {
                    optionSet.WriteOptionDescriptions(Console.Out);
                    Environment.Exit(0);
                }

                if (fileName != null)
                {
                    var fileInfo = new FileInfo(fileName);
                    if (fileInfo.Exists)
                    {
                        var buffer = new byte[fileInfo.Length];

                        using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
                        {
                            fileStream.Read(buffer, 0, buffer.Length);
                        }

                        var text = Encoding.UTF8.GetString(buffer);

                        GenerateQRCode(text, eccLevel, outputFileName, imageFormat);
                    }
                    else
                    {
                        Console.WriteLine($"{friendlyName}: {fileName}: No such file or directory");
                    }
                }
                else if (payload != null)
                {
                    GenerateQRCode(payload, eccLevel, outputFileName, imageFormat);
                }
            }
            catch (Exception oe)
            {
                Console.Error.WriteLine(
                    $"{friendlyName}:{newLine}{oe.Message}{newLine}Try '{friendlyName} --help' for more information");
                Environment.Exit(-1);
            }
        }

        private static void GenerateQRCode(string payloadString, QRCodeGenerator.ECCLevel eccLevel, string outputFileName, ImageFormat imgFormat)
        {
            using (var generator = new QRCodeGenerator())
            {
                using (var data = generator.CreateQrCode(payloadString, eccLevel))
                {
                    using (var code = new QRCode(data))
                    {
                        using (var bitmap = code.GetGraphic(20))
                        {
                            bitmap.Save(outputFileName, imgFormat);
                        }
                    }
                }
            }
        }
    }

    public class OptionSetter
    {
        public QRCodeGenerator.ECCLevel GetECCLevel(string value)
        {
            QRCodeGenerator.ECCLevel level;

            Enum.TryParse (value, out level);

            return level;
        }

        public ImageFormat GetImageFormat(string value)
        {
            switch (value.ToLower())
            {
                case "png":
                    return ImageFormat.Png;
                case "jpg":
                    return ImageFormat.Jpeg;
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "gif":
                    return ImageFormat.Gif;
                case "bmp":
                    return ImageFormat.Bmp;
                case "tiff":
                    return ImageFormat.Tiff;
                default:
                    return ImageFormat.Png;
            }
        }
    }
}
