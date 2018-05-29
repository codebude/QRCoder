using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NDesk.Options;
using System.Reflection;
using QRCoder;
using System.Text;
using System.Windows.Markup;

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
            SupportedImageFormat imageFormat = SupportedImageFormat.Png;
            int pixelsPerModule = 20;
            string foregroundColor = "#000000";
            string backgroundColor = "#FFFFFF";


            var showHelp = false;

            var optionSet = new OptionSet {
                {    "e|ecc-level=",
                    "error correction level",
                    value => eccLevel = setter.GetECCLevel(value)
                },
                {   "f|output-format=",
                    "Image format for outputfile. Possible values: png, jpg, gif, bmp, tiff, svg, xaml, ps, eps (default: png)",
                    value => { Enum.TryParse(value, true, out imageFormat); }
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
                {     "s|pixel=",
                    "pixels per module",
                    value => {
                                 if (int.TryParse(value, out pixelsPerModule))
                                 {
                                     if (pixelsPerModule < 1)
                                     {
                                         pixelsPerModule = 20;
                                     }
                                 }
                                 else
                                 {
                                     pixelsPerModule = 20;
                                 }
                    }
                },
                {     "l|background=",
                    "background color",
                    value => backgroundColor = value
                },
                {     "d|foreground=",
                    "foreground color",
                    value => foregroundColor = value
                },
                {     "h|help",
                    "show this message and exit.",
                    value => showHelp = value != null
                }

            };

            try
            {

                optionSet.Parse(args);

                if (showHelp)
                {
                    ShowHelp(optionSet);
                }

                string text = null;
                if (fileName != null)
                {
                    var fileInfo = new FileInfo(fileName);
                    if (fileInfo.Exists)
                    {
                        text = GetTextFromFile(fileInfo);
                    }
                    else
                    {
                        Console.WriteLine($"{friendlyName}: {fileName}: No such file or directory");
                    }
                }
                else if (payload != null)
                {
                    text = payload;
                }
                else
                {
                    var stdin = Console.OpenStandardInput();

                    text = GetTextFromStream(stdin);
                }

                if (text != null)
                {
                    GenerateQRCode(text, eccLevel, outputFileName, imageFormat, pixelsPerModule, foregroundColor, backgroundColor);
                }
            }
            catch (Exception oe)
            {
                Console.Error.WriteLine(
                    $"{friendlyName}:{newLine}{oe.GetType().FullName}{newLine}{oe.Message}{newLine}{oe.StackTrace}{newLine}Try '{friendlyName} --help' for more information");
                Environment.Exit(-1);
            }
        }

        private static void GenerateQRCode(string payloadString, QRCodeGenerator.ECCLevel eccLevel, string outputFileName, SupportedImageFormat imgFormat, int pixelsPerModule, string foreground, string background)
        {
            using (var generator = new QRCodeGenerator())
            {
                using (var data = generator.CreateQrCode(payloadString, eccLevel))
                {
                    switch (imgFormat)
                    {
                        case SupportedImageFormat.Png:
                        case SupportedImageFormat.Jpg:
                        case SupportedImageFormat.Gif:
                        case SupportedImageFormat.Bmp:
                        case SupportedImageFormat.Tiff:
                            using (var code = new QRCode(data))
                            {
                                using (var bitmap = code.GetGraphic(pixelsPerModule, foreground, background, true))
                                {
                                    var actualFormat = new OptionSetter().GetImageFormat(imgFormat.ToString());
                                    bitmap.Save(outputFileName, actualFormat);
                                }
                            }
                            break;
                        case SupportedImageFormat.Svg:
                            using (var code = new SvgQRCode(data))
                            {
                                var test = code.GetGraphic(pixelsPerModule, foreground, background, true);
                                using (var f = File.CreateText(outputFileName))
                                {
                                    f.Write(test);
                                    f.Flush();
                                }
                            }
                            break;
                        case SupportedImageFormat.Xaml:
                            using (var code = new XamlQRCode(data))
                            {
                                var test = XamlWriter.Save(code.GetGraphic(pixelsPerModule, foreground, background, true));
                                using (var f = File.CreateText(outputFileName))
                                {
                                    f.Write(test);
                                    f.Flush();
                                }
                            }
                            break;
                        case SupportedImageFormat.Ps:
                        case SupportedImageFormat.Eps:
                            using (var code = new PostscriptQRCode(data))
                            {
                                var test = code.GetGraphic(pixelsPerModule, foreground, background, true, 
                                    imgFormat == SupportedImageFormat.Eps);
                                using (var f = File.CreateText(outputFileName))
                                {
                                    f.Write(test);
                                    f.Flush();
                                }
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(imgFormat), imgFormat, null);
                    }

                }
            }
        }

        private static string GetTextFromFile(FileInfo fileInfo)
        {
            var buffer = new byte[fileInfo.Length];

            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                fileStream.Read(buffer, 0, buffer.Length);
            }

            return Encoding.UTF8.GetString(buffer);
        }

        private static string GetTextFromStream(Stream stream)
        {
            var buffer = new byte[256];
            var bytesRead = 0;

            using (var memoryStream = new MemoryStream())
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, bytesRead);
                }

                var text = Encoding.UTF8.GetString(memoryStream.ToArray());

                Console.WriteLine($"text retrieved from input stream: {text}");

                return text;
            }
        }

        private static void ShowHelp(OptionSet optionSet)
        {
            optionSet.WriteOptionDescriptions(Console.Out);
            Environment.Exit(0);
        }
    }

    public class OptionSetter
    {
        public QRCodeGenerator.ECCLevel GetECCLevel(string value)
        {
            QRCodeGenerator.ECCLevel level;

            Enum.TryParse(value, out level);

            return level;
        }

        public ImageFormat GetImageFormat(string value)
        {
            switch (value.ToLower())
            {
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
                case "png":
                default:
                    return ImageFormat.Png;
            }
        }
    }
}

