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

			String fileName = null, outputFileName = null;

			QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.L;

			var showHelp = false;


			var optionSet = new OptionSet {
				{	"e|ecc-level=",
					"error correction level",
					value => eccLevel = setter.GetECCLevel(value)
				},
				{
					"i|in=",
					"input file",
					value => fileName = value
				},
				{
					"o|out=",
					"output file",
					value => outputFileName = value
				},
				{ 	"h|help",
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

		        var fileInfo = new FileInfo(fileName);

		        if (fileInfo.Exists)
		        {
		            var buffer = new byte[fileInfo.Length];

		            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
		            {
		                fileStream.Read(buffer, 0, buffer.Length);
		            }

		            var text = Encoding.UTF8.GetString(buffer);

		            using (var generator = new QRCodeGenerator())
		            {
		                using (var data = generator.CreateQrCode(text, eccLevel))
		                {
		                    using (var code = new QRCode(data))
		                    {
		                        using (var bitmap = code.GetGraphic(20))
		                        {
		                            bitmap.Save(outputFileName, ImageFormat.Png);
		                        }
		                    }
		                }
		            }


		        }
		        else
		        {
		            Console.WriteLine($"{friendlyName}: {fileName}: No such file or directory");
		        }
		    }
		    catch (Exception oe)
		    {
		        Console.Error.WriteLine(
		            $"{friendlyName}:{newLine}{oe.Message}{newLine}Try '{friendlyName} --help' for more information");
		        Environment.Exit(-1);
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
	}
		
}
