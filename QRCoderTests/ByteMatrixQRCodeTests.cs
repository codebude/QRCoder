using Xunit;
using QRCoder;
using Shouldly;
using QRCoderTests.Helpers.XUnitExtenstions;
using System;
using System.Collections.Generic;

namespace QRCoderTests
{
    public class ByteMatrixQRCodeTests
    {                        

        [Fact]
        [Category("QRRenderer/ByteMatrixQRCode")]
        public void can_render_byte_matrix_qrcode_string()
        {
            var expectedString =
                "00000000000000000000000000000\n" +
                "00000000000000000000000000000\n" +
                "00000000000000000000000000000\n" +
                "00000000000000000000000000000\n" +
                "00001111111001010011111110000\n" +
                "00001000001010011010000010000\n" +
                "00001011101010101010111010000\n" +
                "00001011101000100010111010000\n" +
                "00001011101010000010111010000\n" +
                "00001000001001111010000010000\n" +
                "00001111111010101011111110000\n" +
                "00000000000010110000000000000\n" +
                "00001111101100011110101100000\n" +
                "00001100100100110001111010000\n" +
                "00000010101111101010101100000\n" +
                "00001000100110110110000000000\n" +
                "00000111101110000001101010000\n" +
                "00000000000000000111100000000\n" +
                "00001111111011011010110110000\n" +
                "00001000001000000111100000000\n" +
                "00001011101010101001001010000\n" +
                "00001011101011100101000000000\n" +
                "00001011101010101010110110000\n" +
                "00001000001011011000010100000\n" +
                "00001111111011100000101110000\n" +
                "00000000000000000000000000000\n" +
                "00000000000000000000000000000\n" +
                "00000000000000000000000000000\n" +
                "00000000000000000000000000000";

            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
            var byteMatrixString = new ByteMatrixQRCode(data).GetGraphic(1);

            byteMatrixString.ShouldBe(expectedString);
        }

        [Fact]
        [Category("QRRenderer/ByteMatrixQRCode")]
        public void can_render_byte_matrix_qrcode_string_without_quietzones()
        {
            var expectedString =
                "111111100101001111111\n" +
                "100000101001101000001\n" +
                "101110101010101011101\n" +
                "101110100010001011101\n" +
                "101110101000001011101\n" +
                "100000100111101000001\n" +
                "111111101010101111111\n" +
                "000000001011000000000\n" +
                "111110110001111010110\n" +
                "110010010011000111101\n" +
                "001010111110101010110\n" +
                "100010011011011000000\n" +
                "011110111000000110101\n" +
                "000000000000011110000\n" +
                "111111101101101011011\n" +
                "100000100000011110000\n" +
                "101110101010100100101\n" +
                "101110101110010100000\n" +
                "101110101010101011011\n" +
                "100000101101100001010\n" +
                "111111101110000010111";

            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
            var byteMatrixString = new ByteMatrixQRCode(data).GetGraphic(1, drawQuietZones: false);

            byteMatrixString.ShouldBe(expectedString);
        }

        [Fact]
        [Category("QRRenderer/ByteMatrixQRCode")]
        public void getgraphic_returns_null_if_no_data()
        {
            var byteMatrixString = new ByteMatrixQRCode(null).GetGraphic();

            byteMatrixString.ShouldBeNull();
        }

        [Fact]
        [Category("QRRenderer/ByteMatrixQRCode")]
        public void getbytematrix_returns_null_if_no_data()
        {
            var byteMatrix = new ByteMatrixQRCode(null).GetByteMatrix();
            byteMatrix.ShouldBeNull();
        }

        [Fact]
        [Category("QRRenderer/ByteMatrixQRCode")]
        public void can_render_byte_matrix_qrcode_string_with_specified_pixel_modules()
        {
            var expectedString =
                "111111111111110000110011000011111111111111\n" +
                "111111111111110000110011000011111111111111\n" +
                "110000000000110011000011110011000000000011\n" +
                "110000000000110011000011110011000000000011\n" +
                "110011111100110011001100110011001111110011\n" +
                "110011111100110011001100110011001111110011\n" +
                "110011111100110000001100000011001111110011\n" +
                "110011111100110000001100000011001111110011\n" +
                "110011111100110011000000000011001111110011\n" +
                "110011111100110011000000000011001111110011\n" +
                "110000000000110000111111110011000000000011\n" +
                "110000000000110000111111110011000000000011\n" +
                "111111111111110011001100110011111111111111\n" +
                "111111111111110011001100110011111111111111\n" +
                "000000000000000011001111000000000000000000\n" +
                "000000000000000011001111000000000000000000\n" +
                "111111111100111100000011111111001100111100\n" +
                "111111111100111100000011111111001100111100\n" +
                "111100001100001100001111000000111111110011\n" +
                "111100001100001100001111000000111111110011\n" +
                "000011001100111111111100110011001100111100\n" +
                "000011001100111111111100110011001100111100\n" +
                "110000001100001111001111001111000000000000\n" +
                "110000001100001111001111001111000000000000\n" +
                "001111111100111111000000000000111100110011\n" +
                "001111111100111111000000000000111100110011\n" +
                "000000000000000000000000001111111100000000\n" +
                "000000000000000000000000001111111100000000\n" +
                "111111111111110011110011110011001111001111\n" +
                "111111111111110011110011110011001111001111\n" +
                "110000000000110000000000001111111100000000\n" +
                "110000000000110000000000001111111100000000\n" +
                "110011111100110011001100110000110000110011\n" +
                "110011111100110011001100110000110000110011\n" +
                "110011111100110011111100001100110000000000\n" +
                "110011111100110011111100001100110000000000\n" +
                "110011111100110011001100110011001111001111\n" +
                "110011111100110011001100110011001111001111\n" +
                "110000000000110011110011110000000011001100\n" +
                "110000000000110011110011110000000011001100\n" +
                "111111111111110011111100000000001100111111\n" +
                "111111111111110011111100000000001100111111";

            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A05", QRCodeGenerator.ECCLevel.Q);
            var byteMatrixString = new ByteMatrixQRCode(data).GetGraphic(2, drawQuietZones: false);

            byteMatrixString.ShouldBe(expectedString);
        }

        [Fact]
        [Category("QRRenderer/ByteMatrixQRCode")]
        public void can_get_byte_matrix_qrcode()
        {
            var expectedString =
                "00000000000000000000000000000\n" +
                "00000000000000000000000000000\n" +
                "00000000000000000000000000000\n" +
                "00000000000000000000000000000\n" +
                "00001111111000011011111110000\n" +
                "00001000001011101010000010000\n" +
                "00001011101001111010111010000\n" +
                "00001011101001100010111010000\n" +
                "00001011101010010010111010000\n" +
                "00001000001001000010000010000\n" +
                "00001111111010101011111110000\n" +
                "00000000000001111000000000000\n" +
                "00000010111011100100100100000\n" +
                "00001011100110111100111010000\n" +
                "00000000001010100100101000000\n" +
                "00000100000100001010111000000\n" +
                "00000101111011010011110010000\n" +
                "00000000000010011110000110000\n" +
                "00001111111000111100101110000\n" +
                "00001000001011100001111000000\n" +
                "00001011101010110000101100000\n" +
                "00001011101001100001111000000\n" +
                "00001011101010111100101110000\n" +
                "00001000001001000000110010000\n" +
                "00001111111001001110110110000\n" +
                "00000000000000000000000000000\n" +
                "00000000000000000000000000000\n" +
                "00000000000000000000000000000\n" +
                "00000000000000000000000000000";

            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A", QRCodeGenerator.ECCLevel.Q);
            var byteMatrix = new ByteMatrixQRCode(data).GetByteMatrix();
            var byteMatrixString = ByteMatrixToString(byteMatrix);

            byteMatrixString.ShouldBe(expectedString);

            Console.WriteLine();
        }

        [Fact]
        [Category("QRRenderer/ByteMatrixQRCode")]
        public void can_get_byte_matrix_qrcode_without_quietzones()
        {
            var expectedString =
                "111111100001101111111\n" +
                "100000101110101000001\n" +
                "101110100111101011101\n" +
                "101110100110001011101\n" +
                "101110101001001011101\n" +
                "100000100100001000001\n" +
                "111111101010101111111\n" +
                "000000000111100000000\n" +
                "001011101110010010010\n" +
                "101110011011110011101\n" +
                "000000101010010010100\n" +
                "010000010000101011100\n" +
                "010111101101001111001\n" +
                "000000001001111000011\n" +
                "111111100011110010111\n" +
                "100000101110000111100\n" +
                "101110101011000010110\n" +
                "101110100110000111100\n" +
                "101110101011110010111\n" +
                "100000100100000011001\n" +
                "111111100100111011011";

            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A", QRCodeGenerator.ECCLevel.Q);
            var byteMatrix = new ByteMatrixQRCode(data).GetByteMatrix(drawQuietZones: false);
            var byteMatrixString = ByteMatrixToString(byteMatrix);

            byteMatrixString.ShouldBe(expectedString);

            Console.WriteLine();
        }

        [Fact]
        [Category("QRRenderer/ByteMatrixQRCode")]
        public void can_instantate_parameterless()
        {
            var asciiCode = new ByteMatrixQRCode();
            asciiCode.ShouldNotBeNull();
            asciiCode.ShouldBeOfType<ByteMatrixQRCode>();
        }

        [Fact]
        [Category("QRRenderer/ByteMatrixQRCode")]
        public void can_render_byte_matrix_qrcode_string_from_helper()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A", QRCodeGenerator.ECCLevel.Q);
            var byteMatrixString = new ByteMatrixQRCode(data).GetGraphic(1);

            var byteMatrixHelperString = ByteMatrixQRCodeHelper.GetQRCode("A", 1, QRCodeGenerator.ECCLevel.Q);
            byteMatrixString.ShouldBe(byteMatrixHelperString);
        }

        [Fact]
        [Category("QRRenderer/ByteMatrixQRCode")]
        public void can_get_byte_matrix_qrcode_from_helper()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("A", QRCodeGenerator.ECCLevel.Q);
            var byteMatrix = new ByteMatrixQRCode(data).GetByteMatrix();
            var byteMatrixString = ByteMatrixToString(byteMatrix);

            var byteMatrixHelper = ByteMatrixQRCodeHelper.GetQRCodeByteMatrix("A", QRCodeGenerator.ECCLevel.Q);
            var byteMatrixHelperString = ByteMatrixToString(byteMatrixHelper);
            byteMatrixString.ShouldBe(byteMatrixHelperString);
        }

        private string ByteMatrixToString(byte[,] byteMatrix, string endOfLine = "\n")
        {
            List<string> byteMatrixContent = new List<string>();

            for (int i = 0; i < byteMatrix.GetLength(0); i++)
            {
                string line = string.Empty;

                for (int j = 0; j < byteMatrix.GetLength(1); j++)
                {
                    line += byteMatrix[i, j];
                }

                byteMatrixContent.Add(line);
            }

            return string.Join(endOfLine, byteMatrixContent.ToArray());
        }
    }
}



