using QRCoder;
using QRCoderTests.Helpers.XUnitExtenstions;
using Shouldly;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using System.Linq;
using System.Collections;
using System.Text;

namespace QRCoderTests
{

    public class QRGeneratorTests
    {
        [Fact]
        [Category("QRGenerator/Antilog")]
        public void validate_antilogtable()
        {
            var gen = new QRCodeGenerator();

            var checkString = string.Empty;
            var gField = gen.GetType().GetField("galoisField", BindingFlags.NonPublic | BindingFlags.Static);
            foreach (var listitem in (System.Collections.IEnumerable)gField.GetValue(gen))
            {
                foreach (PropertyInfo prop in listitem.GetType().GetProperties())
                    checkString += prop.GetValue(listitem, null).ToString() + ",";

                checkString += ":";
            }
            checkString.ShouldBe("0,1,:1,2,:2,4,:3,8,:4,16,:5,32,:6,64,:7,128,:8,29,:9,58,:10,116,:11,232,:12,205,:13,135,:14,19,:15,38,:16,76,:17,152,:18,45,:19,90,:20,180,:21,117,:22,234,:23,201,:24,143,:25,3,:26,6,:27,12,:28,24,:29,48,:30,96,:31,192,:32,157,:33,39,:34,78,:35,156,:36,37,:37,74,:38,148,:39,53,:40,106,:41,212,:42,181,:43,119,:44,238,:45,193,:46,159,:47,35,:48,70,:49,140,:50,5,:51,10,:52,20,:53,40,:54,80,:55,160,:56,93,:57,186,:58,105,:59,210,:60,185,:61,111,:62,222,:63,161,:64,95,:65,190,:66,97,:67,194,:68,153,:69,47,:70,94,:71,188,:72,101,:73,202,:74,137,:75,15,:76,30,:77,60,:78,120,:79,240,:80,253,:81,231,:82,211,:83,187,:84,107,:85,214,:86,177,:87,127,:88,254,:89,225,:90,223,:91,163,:92,91,:93,182,:94,113,:95,226,:96,217,:97,175,:98,67,:99,134,:100,17,:101,34,:102,68,:103,136,:104,13,:105,26,:106,52,:107,104,:108,208,:109,189,:110,103,:111,206,:112,129,:113,31,:114,62,:115,124,:116,248,:117,237,:118,199,:119,147,:120,59,:121,118,:122,236,:123,197,:124,151,:125,51,:126,102,:127,204,:128,133,:129,23,:130,46,:131,92,:132,184,:133,109,:134,218,:135,169,:136,79,:137,158,:138,33,:139,66,:140,132,:141,21,:142,42,:143,84,:144,168,:145,77,:146,154,:147,41,:148,82,:149,164,:150,85,:151,170,:152,73,:153,146,:154,57,:155,114,:156,228,:157,213,:158,183,:159,115,:160,230,:161,209,:162,191,:163,99,:164,198,:165,145,:166,63,:167,126,:168,252,:169,229,:170,215,:171,179,:172,123,:173,246,:174,241,:175,255,:176,227,:177,219,:178,171,:179,75,:180,150,:181,49,:182,98,:183,196,:184,149,:185,55,:186,110,:187,220,:188,165,:189,87,:190,174,:191,65,:192,130,:193,25,:194,50,:195,100,:196,200,:197,141,:198,7,:199,14,:200,28,:201,56,:202,112,:203,224,:204,221,:205,167,:206,83,:207,166,:208,81,:209,162,:210,89,:211,178,:212,121,:213,242,:214,249,:215,239,:216,195,:217,155,:218,43,:219,86,:220,172,:221,69,:222,138,:223,9,:224,18,:225,36,:226,72,:227,144,:228,61,:229,122,:230,244,:231,245,:232,247,:233,243,:234,251,:235,235,:236,203,:237,139,:238,11,:239,22,:240,44,:241,88,:242,176,:243,125,:244,250,:245,233,:246,207,:247,131,:248,27,:249,54,:250,108,:251,216,:252,173,:253,71,:254,142,:255,1,:");
        }

        [Fact]
        [Category("QRGenerator/AlphanumDict")]
        public void validate_alphanumencdict()
        {
            var gen = new QRCodeGenerator();

            var checkString = string.Empty;
            var gField = gen.GetType().GetField("alphanumEncDict", BindingFlags.NonPublic | BindingFlags.Static);
            foreach (var listitem in (Dictionary<char, int>)gField.GetValue(gen))
            {
                checkString += $"{listitem.Key},{listitem.Value}:";
            }
            checkString.ShouldBe("0,0:1,1:2,2:3,3:4,4:5,5:6,6:7,7:8,8:9,9:A,10:B,11:C,12:D,13:E,14:F,15:G,16:H,17:I,18:J,19:K,20:L,21:M,22:N,23:O,24:P,25:Q,26:R,27:S,28:T,29:U,30:V,31:W,32:X,33:Y,34:Z,35: ,36:$,37:%,38:*,39:+,40:-,41:.,42:/,43::,44:");
        }

        [Fact]
        [Category("QRGenerator/TextEncoding")]
        public void can_recognize_enconding_numeric()
        {
            var gen = new QRCodeGenerator();
            MethodInfo method = gen.GetType().GetMethod("GetEncodingFromPlaintext", BindingFlags.NonPublic | BindingFlags.Static);
            var result = (int)method.Invoke(gen, new object[] { "0123456789", false });

            result.ShouldBe(1);
        }


        [Fact]
        [Category("QRGenerator/TextEncoding")]
        public void can_recognize_enconding_alphanumeric()
        {
            var gen = new QRCodeGenerator();
            MethodInfo method = gen.GetType().GetMethod("GetEncodingFromPlaintext", BindingFlags.NonPublic | BindingFlags.Static);
            var result = (int)method.Invoke(gen, new object[] { "0123456789ABC", false });

            result.ShouldBe(2);
        }


        [Fact]
        [Category("QRGenerator/TextEncoding")]
        public void can_recognize_enconding_forced_bytemode()
        {
            var gen = new QRCodeGenerator();
            MethodInfo method = gen.GetType().GetMethod("GetEncodingFromPlaintext", BindingFlags.NonPublic | BindingFlags.Static);
            var result = (int)method.Invoke(gen, new object[] { "0123456789", true });

            result.ShouldBe(4);
        }


        [Fact]
        [Category("QRGenerator/TextEncoding")]
        public void can_recognize_enconding_byte()
        {
            var gen = new QRCodeGenerator();
            MethodInfo method = gen.GetType().GetMethod("GetEncodingFromPlaintext", BindingFlags.NonPublic | BindingFlags.Static);
            var result = (int)method.Invoke(gen, new object[] { "0123456789äöüß", false });

            result.ShouldBe(4);
        }

        [Fact]
        [Category("QRGenerator/TextEncoding")]
        public void can_encode_numeric()
        {
            var gen = new QRCodeGenerator();
            var qrData = gen.CreateQrCode("123", QRCodeGenerator.ECCLevel.L);
            var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
            result.ShouldBe("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001111111011111011111110000000010000010010100100000100000000101110100110001011101000000001011101001110010111010000000010111010001010101110100000000100000100001101000001000000001111111010101011111110000000000000000111110000000000000000110110100110101000001000000001110110000001010101100000000000110111100001101110000000000101111010011000001111000000000011101111100010011010000000000000000111110010101100000000111111100010111110001000000001000001000011101110010000000010111010101110110110100000000101110101011100011100000000001011101001100010001110000000010000010101001101010100000000111111101101000001110000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
        }

        [Fact]
        [Category("QRGenerator/TextEncoding")]
        public void can_encode_alphanumeric()
        {
            var gen = new QRCodeGenerator();
            var qrData = gen.CreateQrCode("123ABC", QRCodeGenerator.ECCLevel.L);
            var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
            result.ShouldBe("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001111111010111011111110000000010000010001100100000100000000101110101101001011101000000001011101011001010111010000000010111010100100101110100000000100000100111101000001000000001111111010101011111110000000000000000000110000000000000000111100101111110011101000000000111100010011110001110000000000100010100100000001000000000011110011111001110011000000001111101110101001000000000000000000000111100100100100000000111111100001100100110000000001000001000100001111110000000010111010010011111010100000000101110101111001011110000000001011101010101011000000000000010000010111001000010000000000111111101010010010010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
        }

        [Fact]
        [Category("QRGenerator/TextEncoding")]
        public void can_encode_byte()
        {
            var gen = new QRCodeGenerator();
            var qrData = gen.CreateQrCode("äöü", QRCodeGenerator.ECCLevel.L);
            var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
            result.ShouldBe("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001111111001011011111110000000010000010011100100000100000000101110101101101011101000000001011101001010010111010000000010111010001010101110100000000100000100000101000001000000001111111010101011111110000000000000000110110000000000000000111011111111011000100000000001001110001100010000010000000010011110001010001001000000000110011010000001000110000000001110001111001010110110000000000000000111101010011100000000111111101111011100110000000001000001010011101110010000000010111010110101110010100000000101110100110001000110000000001011101011001000100010000000010000010100000100011000000000111111101110101010111000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
        }
    }

    public static class ExtensionMethods
    {
        public static string ToBitString(this BitArray bits)
        {
            var sb = new StringBuilder();
            int bitLength = 0;
#if !NETCOREAPP1_1
            bitLength = bits.Count;
#else
            bitLength = bits.Length;
#endif
            for (int i = 0; i < bitLength; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}



