using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using QRCoder;
using Shouldly;
using Xunit;
using ECCLevel = QRCoder.QRCodeGenerator.ECCLevel;


namespace QRCoderTests;


public class QRGeneratorTests
{
    [Fact]
    public void micro()
    {
        var input = "00000000";
        var expectedSize = 13;

        var qrData = QRCodeGenerator.GenerateMicroQrCode(input, ECCLevel.M, -2);
        (qrData.ModuleMatrix.Count - 8).ShouldBe(expectedSize); // exclude padding
        var encoder = new AsciiQRCode(qrData);
        var txt = encoder.GetGraphicSmall(false, true, Environment.NewLine);
        Debug.WriteLine(txt);
        txt = encoder.GetGraphic(1, drawQuietZones: false, endOfLine: Environment.NewLine);
        Debug.WriteLine(txt);

        //var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
        //var hash = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(result));
        //var hashString = Convert.ToBase64String(hash);
        //hashString.TrimEnd('=').ShouldBe(expectedHash);
    }

    [Fact]
    public void validate_antilogtable()
    {
        var gen = new QRCodeGenerator();

        var checkString = string.Empty;
        var tablesType = Type.GetType("QRCoder.QRCodeGenerator+GaloisField, QRCoder");
        var gField = tablesType.GetField("_galoisFieldByExponentAlpha", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null).ShouldBeOfType<int[]>();
        gField.Length.ShouldBe(256);
        for (int i = 0; i < gField.Length; i++)
        {
            checkString += i + "," + gField[i] + ",:";
        }
        checkString.ShouldBe("0,1,:1,2,:2,4,:3,8,:4,16,:5,32,:6,64,:7,128,:8,29,:9,58,:10,116,:11,232,:12,205,:13,135,:14,19,:15,38,:16,76,:17,152,:18,45,:19,90,:20,180,:21,117,:22,234,:23,201,:24,143,:25,3,:26,6,:27,12,:28,24,:29,48,:30,96,:31,192,:32,157,:33,39,:34,78,:35,156,:36,37,:37,74,:38,148,:39,53,:40,106,:41,212,:42,181,:43,119,:44,238,:45,193,:46,159,:47,35,:48,70,:49,140,:50,5,:51,10,:52,20,:53,40,:54,80,:55,160,:56,93,:57,186,:58,105,:59,210,:60,185,:61,111,:62,222,:63,161,:64,95,:65,190,:66,97,:67,194,:68,153,:69,47,:70,94,:71,188,:72,101,:73,202,:74,137,:75,15,:76,30,:77,60,:78,120,:79,240,:80,253,:81,231,:82,211,:83,187,:84,107,:85,214,:86,177,:87,127,:88,254,:89,225,:90,223,:91,163,:92,91,:93,182,:94,113,:95,226,:96,217,:97,175,:98,67,:99,134,:100,17,:101,34,:102,68,:103,136,:104,13,:105,26,:106,52,:107,104,:108,208,:109,189,:110,103,:111,206,:112,129,:113,31,:114,62,:115,124,:116,248,:117,237,:118,199,:119,147,:120,59,:121,118,:122,236,:123,197,:124,151,:125,51,:126,102,:127,204,:128,133,:129,23,:130,46,:131,92,:132,184,:133,109,:134,218,:135,169,:136,79,:137,158,:138,33,:139,66,:140,132,:141,21,:142,42,:143,84,:144,168,:145,77,:146,154,:147,41,:148,82,:149,164,:150,85,:151,170,:152,73,:153,146,:154,57,:155,114,:156,228,:157,213,:158,183,:159,115,:160,230,:161,209,:162,191,:163,99,:164,198,:165,145,:166,63,:167,126,:168,252,:169,229,:170,215,:171,179,:172,123,:173,246,:174,241,:175,255,:176,227,:177,219,:178,171,:179,75,:180,150,:181,49,:182,98,:183,196,:184,149,:185,55,:186,110,:187,220,:188,165,:189,87,:190,174,:191,65,:192,130,:193,25,:194,50,:195,100,:196,200,:197,141,:198,7,:199,14,:200,28,:201,56,:202,112,:203,224,:204,221,:205,167,:206,83,:207,166,:208,81,:209,162,:210,89,:211,178,:212,121,:213,242,:214,249,:215,239,:216,195,:217,155,:218,43,:219,86,:220,172,:221,69,:222,138,:223,9,:224,18,:225,36,:226,72,:227,144,:228,61,:229,122,:230,244,:231,245,:232,247,:233,243,:234,251,:235,235,:236,203,:237,139,:238,11,:239,22,:240,44,:241,88,:242,176,:243,125,:244,250,:245,233,:246,207,:247,131,:248,27,:249,54,:250,108,:251,216,:252,173,:253,71,:254,142,:255,1,:");

        var gField2 = tablesType.GetField("_galoisFieldByIntegerValue", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null).ShouldBeOfType<int[]>();
        gField2.Length.ShouldBe(256);
        var checkString2 = string.Empty;
        for (int i = 0; i < gField2.Length; i++)
        {
            checkString2 += i + "," + gField2[i] + ",:";
        }
        checkString2.ShouldBe("0,0,:1,0,:2,1,:3,25,:4,2,:5,50,:6,26,:7,198,:8,3,:9,223,:10,51,:11,238,:12,27,:13,104,:14,199,:15,75,:16,4,:17,100,:18,224,:19,14,:20,52,:21,141,:22,239,:23,129,:24,28,:25,193,:26,105,:27,248,:28,200,:29,8,:30,76,:31,113,:32,5,:33,138,:34,101,:35,47,:36,225,:37,36,:38,15,:39,33,:40,53,:41,147,:42,142,:43,218,:44,240,:45,18,:46,130,:47,69,:48,29,:49,181,:50,194,:51,125,:52,106,:53,39,:54,249,:55,185,:56,201,:57,154,:58,9,:59,120,:60,77,:61,228,:62,114,:63,166,:64,6,:65,191,:66,139,:67,98,:68,102,:69,221,:70,48,:71,253,:72,226,:73,152,:74,37,:75,179,:76,16,:77,145,:78,34,:79,136,:80,54,:81,208,:82,148,:83,206,:84,143,:85,150,:86,219,:87,189,:88,241,:89,210,:90,19,:91,92,:92,131,:93,56,:94,70,:95,64,:96,30,:97,66,:98,182,:99,163,:100,195,:101,72,:102,126,:103,110,:104,107,:105,58,:106,40,:107,84,:108,250,:109,133,:110,186,:111,61,:112,202,:113,94,:114,155,:115,159,:116,10,:117,21,:118,121,:119,43,:120,78,:121,212,:122,229,:123,172,:124,115,:125,243,:126,167,:127,87,:128,7,:129,112,:130,192,:131,247,:132,140,:133,128,:134,99,:135,13,:136,103,:137,74,:138,222,:139,237,:140,49,:141,197,:142,254,:143,24,:144,227,:145,165,:146,153,:147,119,:148,38,:149,184,:150,180,:151,124,:152,17,:153,68,:154,146,:155,217,:156,35,:157,32,:158,137,:159,46,:160,55,:161,63,:162,209,:163,91,:164,149,:165,188,:166,207,:167,205,:168,144,:169,135,:170,151,:171,178,:172,220,:173,252,:174,190,:175,97,:176,242,:177,86,:178,211,:179,171,:180,20,:181,42,:182,93,:183,158,:184,132,:185,60,:186,57,:187,83,:188,71,:189,109,:190,65,:191,162,:192,31,:193,45,:194,67,:195,216,:196,183,:197,123,:198,164,:199,118,:200,196,:201,23,:202,73,:203,236,:204,127,:205,12,:206,111,:207,246,:208,108,:209,161,:210,59,:211,82,:212,41,:213,157,:214,85,:215,170,:216,251,:217,96,:218,134,:219,177,:220,187,:221,204,:222,62,:223,90,:224,203,:225,89,:226,95,:227,176,:228,156,:229,169,:230,160,:231,81,:232,11,:233,245,:234,22,:235,235,:236,122,:237,117,:238,44,:239,215,:240,79,:241,174,:242,213,:243,233,:244,230,:245,231,:246,173,:247,232,:248,116,:249,214,:250,244,:251,234,:252,168,:253,80,:254,88,:255,175,:");
    }

#if !NETFRAMEWORK // [Theory] is not supported in xunit < 2.0.0
    [Theory]
    // version 1 numeric
    [InlineData("1", "KWw84nkWZLMh5LqAJ/4s/4mW/08", 21)]
    [InlineData("12", "+MdvzzZYQNF3d+6NuZGSmqmCmXY", 21)]
    [InlineData("123", "meNWffAoC6ozzXEdDpEjixvBAME", 21)]
    [InlineData("1234", "rOI2dmjilbVXsk4m2sJjAWybMto", 21)]
    [InlineData("12345", "gVrbNyJNTkLCoXhLA1g1vGUlQvI", 21)]
    [InlineData("123456", "TsdKS6PDgtq1b2stRT1C90DiGik", 21)]
    [InlineData("1234567", "pbpVWmVQPjeRSPk/8GIlAbtPPlY", 21)]
    [InlineData("12345678", "ng29QoMxhqMsygeU7t2Ic9RB2hk", 21)]
    [InlineData("123456789", "Xb/EHaUUUU+22a4Hm/2Sr+O1zv0", 21)]
    [InlineData("1234567890", "X0kmbmnqpAFjTuS0SQAEkphAaok", 21)]
    [InlineData("1234567890123456", "afOstf4rTgaLUaHL/Vb23vzjQFM", 21)]
    [InlineData("12345678901234567", "S0BOgmRblr9Bb6Lpkf62WfYIj58", 21)]
    // version 2 numeric
    [InlineData("123456789012345678", "qs5j+bBK3fdRgoQg1N00vUF7f0g", 25)]
    [InlineData("1234567890123456789012345678901234", "mu6wUZp+uXqXGyYFduQZt38Jbu0", 25)]
    // version 3 numeric
    [InlineData("12345678901234567890123456789012345", "AiWiTB6xreLc514aHw4StDsomvk", 29)]
    [InlineData("1234567890123456789012345678901234567890123456789012345678", "WNLD0ved5WdysFG1uqNBBV7ItKI", 29)]
    // version 4 numeric
    [InlineData("12345678901234567890123456789012345678901234567890123456789", "cV6Rijj6q3f/dUlDVOZD3DafrMM", 33)]

    // version 1 alphanumeric
    [InlineData("A", "YUpoycThbE3FwkkHaO6GYqe9V+c", 21)]
    [InlineData("AB", "UnUHZDgLdnYIy0iN31sguw2qbh8", 21)]
    [InlineData("ABC", "GVB3xcSMAawwOZlq0hiF9hqVldg", 21)]
    [InlineData("ABCD", "jATOwpwGVWpou3WtKiq4DX4jWkk", 21)]
    [InlineData("ABCDE", "m/LrK4iP22OW9RmC2r2dnDFd4wE", 21)]
    [InlineData("ABCDEF", "p8acVHkm3z751oh5yK4mBBRMUuE", 21)]
    [InlineData("ABCDEFG", "md1jFcZSqDmQ2KeFTwKJVFrfZko", 21)]
    [InlineData("ABCDEFGH", "XvL+fpHNqQQ2FHUCXraQw77DGns", 21)]
    [InlineData("ABCDEFGHI", "k+DTXI3yht473k9lvYLMdHf0V/0", 21)]
    [InlineData("ABCDEFGHIJ", "f9uful+85iSlJVJAFc5zEk04eMc", 21)]
    // version 2 alphanumeric
    [InlineData("ABCDEFGHIJK", "qiXut4Jz2zX8Tl9DSXxIo+bqjZY", 25)]
    [InlineData("ABCDEFGHIJKL", "wSpjZmpo9CEjlxlYF18xEa6BMYM", 25)]
    [InlineData("ABCDEFGHIJKLM", "utCYtAtJp+GdKS6y6A7jQuES6kA", 25)]
    [InlineData("ABCDEFGHIJKLMN", "jhzNewJNcC875mlYI31BkVgx0G0", 25)]
    [InlineData("ABCDEFGHIJKLMNO", "eWCSdyn3EH3uFDig1a0NUYZFlO0", 25)]
    [InlineData("ABCDEFGHIJKLMNOP", "glV9FE+UQPDkplgOXFhk3Ll29pI", 25)]
    [InlineData("ABCDEFGHIJKLMNOPQ", "Crlq92Pqiw8X9EIG6KFCvStNQuI", 25)]
    [InlineData("ABCDEFGHIJKLMNOPQR", "lIG8/YBsD0uK+Dop6QfvD7IFdAY", 25)]
    [InlineData("ABCDEFGHIJKLMNOPQRS", "rsDPSYotVP65kLAxP3fzbGqt6wc", 25)]
    [InlineData("ABCDEFGHIJKLMNOPQRST", "yOb1jCKlj3wqdczHEPRdWxafvNU", 25)]
    // version 3 alphanumeric
    [InlineData("ABCDEFGHIJKLMNOPQRSTU", "U4YNhfjgJbprsTurHs4E7Mi2sS8", 29)]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUV", "s9WCvPzYhXrwzNoVbDocQtse1w8", 29)]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVW", "6vZ9rDMy1GUVEcnL2ErJFOxqWI0", 29)]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWX", "C9LlmjRV+TPDkR03MlgDvo/DP+U", 29)]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXY", "+EtALGm0mrDrnZVW54WdXG612P0", 29)]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ789012345", "3nFUvZ/Aa2wUdAj1zlMmSu9x4kU", 29)]
    // version 4 alphanumeric
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ7890123456", "9K6EinxynS2KRum46iQsVoPgM7k", 33)]

    // version 1 binary
    [InlineData("a", "zd6kApf0BQSE5W8fhCijkz6wzKA", 21)]
    [InlineData("ab", "mXAUC/dqcqqj6SzC+Us6NiYzzCM", 21)]
    [InlineData("abc", "6Y3HGyOFxhZUYINks/hzE2DjulM", 21)]
    [InlineData("abcd", "ssvlMmEub85t1d0R/aZG+Qgpa+0", 21)]
    [InlineData("abcde", "At93DjDyAtIkTCpOwD3p/lSqFz4", 21)]
    [InlineData("abcdef", "Q8BU1lCJJA/UesKGvszQTptskSk", 21)]
    [InlineData("abcdefg", "2AceZvcjIpEBCh5FIc1esXsaEY4", 21)]
    // version 2 binary
    [InlineData("abcdefgh", "kl8bx15B4ApauURa0nlD71NPk+I", 25)]
    [InlineData("abcdefghi", "v9rI0/2a8nYxM9MerzxSCwT6EKs", 25)]
    [InlineData("abcdefghij", "yE59+LfDLQCt2VXCuhnz9aFIoMk", 25)]
    [InlineData("abcdefghijk", "nVne+lyjPV5XDMKqa0+oNfxZTgI", 25)]
    [InlineData("abcdefghijkl", "QUeDHmjQDyHvbe5r8tViXxBcHv0", 25)]
    [InlineData("abcdefghijklm", "WtN1tTti8hV4vvH5vX6obIPdjpM", 25)]
    [InlineData("abcdefghijklmn", "AT5SPNUPL3wG0r4XXPBzSAK2sIE", 25)]
    // version 3 binary
    [InlineData("abcdefghijklmno", "N04AFOJlXQRjeXijWoy4rsBNZGg", 29)]
    [InlineData("abcdefghijklmnop", "a4tgwBApGX0+P4yiwR/wUtLAxQA", 29)]
    [InlineData("abcdefghijklmnopq", "MFLd+exCRrUZkqfw5UTqY2QZ1n0", 29)]
    [InlineData("abcdefghijklmnopqr", "aSYOJXfFAjxrtBWnBQqHWrC8Zv0", 29)]
    [InlineData("abcdefghijklmnopqrs", "K9Uic6+NO2rPy/Hfo4fEhXkUw2Q", 29)]
    [InlineData("abcdefghijklmnopqrst", "eKVJvIH8J1waEb3UHRdXYAWLezc", 29)]
    [InlineData("abcdefghijklmnopqrstu", "ylmFLWV1grM2MoTFpdngo05fdyI", 29)]
    [InlineData("abcdefghijklmnopqrstuv", "Z0IETxnf8x+pTU2nuj1hxg2G/pQ", 29)]
    [InlineData("abcdefghijklmnopqrstuvw", "oHzGRWtkI+a30AF5JILT6HON7Zc", 29)]
    [InlineData("abcdefghijklmnopqrstuvwx", "NRIoT6rGd3HWrBq4JhBWvbwYp9g", 29)]
    // version 4 binary
    [InlineData("abcdefghijklmnopqrstuvwxy", "RMSyMOBpdBYphJPkXR/xA/ekPoo", 33)]
    [InlineData("abcdefghijklmnopqrstuvwxyz", "BAMiY251UapecfI+v2C3cX2EBH4", 33)]
    // version 5 binary
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghi", "yV9Cd3xiW2HRzSIMq3eLTIrdqVQ", 37)]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghij", "mV/R+gMAwN+lO8ByXhU5IyZp39Y", 37)]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijk", "sIb5hBRamy+MIgaFakCCGnDM9yU", 37)]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijkl", "2/PZLsxe4c/R/tStrn9pcB8EUOQ", 37)]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklm", "KFReyVpr4rq5c+ELZBt/ZuhQkYM", 37)]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmn", "IwlWmCnXp0FSr+WUp/igMuQKHQo", 37)]
    // version 7 binary
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefg", "bisFBjhANRxoF9JDCBSODvsSKqk", 45)]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefgh", "MwRnhkqr5CM17xtcQycytd+d+Fs", 45)]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghi", "PFlhVI0La4/qOweduCP2WfedoCQ", 45)]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghij", "ZwMdK51id9A99IxefE01o5ZtkN4", 45)]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijk", "HM6MMwWDmJ0PTLLBWzIo7Q0YvmA", 45)]
    public void can_encode_various_strings_ecc_h(string input, string expectedHash, int expectedSize)
    {
        var gen = new QRCodeGenerator();
        var qrData = gen.CreateQrCode(input, ECCLevel.H);
        (qrData.ModuleMatrix.Count - 8).ShouldBe(expectedSize); // exclude padding
        var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
        var hash = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(result));
        var hashString = Convert.ToBase64String(hash);
        hashString.TrimEnd('=').ShouldBe(expectedHash);
    }

    [Theory]
    // Version 1
    [InlineData(17, ECCLevel.L, "iOaoY7YsHAYGNRn+Tpnt74IQoVw=", 21)]
    [InlineData(14, ECCLevel.M, "JV2XYoq8nt/lWipVkwvvSbNvFVQ=", 21)]
    [InlineData(11, ECCLevel.Q, "44vd54SCPFEevWN9PKC5swEpVmU=", 21)]
    [InlineData(7, ECCLevel.H, "FvR2FAU+4sltHMS969/Y1FAHZRA=", 21)]
    // Version 2
    [InlineData(32, ECCLevel.L, "vM4eIrKbner3NxjRznd6kZLbyck=", 25)]
    [InlineData(26, ECCLevel.M, "mesaTID5N92ar2fyElorp7zcSVg=", 25)]
    [InlineData(20, ECCLevel.Q, "mg1Z+VPVuxMoGwvgRzJrW4NHehA=", 25)]
    [InlineData(14, ECCLevel.H, "6T0I6Z9AmN9yNIvan82NQqAMATc=", 25)]
    // Version 3
    [InlineData(53, ECCLevel.L, "O8Wkal/iDmCnENBubqR0HXOo/RY=", 29)]
    [InlineData(42, ECCLevel.M, "NCjzwIm3l5urwU4EcFGK5DD1y9U=", 29)]
    [InlineData(32, ECCLevel.Q, "kR+4FNybKyAiGDLPDnzIslvjypQ=", 29)]
    [InlineData(24, ECCLevel.H, "ZamyoGRJG7mGMY6nzz8r3+q0z18=", 29)]
    // Version 4
    [InlineData(78, ECCLevel.L, "P3Dx7gDjD2L94wPyL23AO/z5+Yk=", 33)]
    [InlineData(62, ECCLevel.M, "wDUcQmVTCcTx6sStDlPG4Wsn5FU=", 33)]
    [InlineData(46, ECCLevel.Q, "3mYP2cHqQysy93UC4NGUnNwfj10=", 33)]
    [InlineData(34, ECCLevel.H, "5ovh+NFiGh6soAuNNTWqxenM8cw=", 33)]
    // Version 5
    [InlineData(106, ECCLevel.L, "0vZswNhwdcCcj2GpwucfkcnlG/M=", 37)]
    [InlineData(84, ECCLevel.M, "AWlV4NsbRtkmH2/vfBxTahIiG7U=", 37)]
    [InlineData(60, ECCLevel.Q, "8w35kxFqvcMajba9IvRhjbOn0Js=", 37)]
    [InlineData(44, ECCLevel.H, "+d0gLH3v9FG+w/hhv+zDm2Y3IVw=", 37)]
    // Version 6
    [InlineData(134, ECCLevel.L, "CNyyNDIylrMi97DwuNh6JAgHlw8=", 41)]
    [InlineData(106, ECCLevel.M, "z4LUkv75O26FLaVo823TMLv9Owg=", 41)]
    [InlineData(74, ECCLevel.Q, "NgCKIxbeuSt24C9M067nDGopKgU=", 41)]
    [InlineData(58, ECCLevel.H, "Bzp923oooHYWoQfETFENmb5wup0=", 41)]
    // Version 7
    [InlineData(154, ECCLevel.L, "ftMeEbWj6D0lyOBVsAnTCq0UV0s=", 45)]
    [InlineData(122, ECCLevel.M, "zif9uHXnPgo+OeIN95xU3iqcexk=", 45)]
    [InlineData(86, ECCLevel.Q, "wApf2GzMYIQlYw4ws3k6Wi1DqMU=", 45)]
    [InlineData(64, ECCLevel.H, "i8llCv2L4dwlW5E8+mswsAa+Zo4=", 45)]
    // Version 8
    [InlineData(192, ECCLevel.L, "DSph/W1Nq2VAKFxgRq0VeqTP54g=", 49)]
    [InlineData(152, ECCLevel.M, "j06phyT/k6pXqf935BuaMxUjckk=", 49)]
    [InlineData(108, ECCLevel.Q, "RufVav4xUUuL/K5ELnH3/qUrEf8=", 49)]
    [InlineData(84, ECCLevel.H, "oG538pE6ac81I2of3LzIHQ6+Dxg=", 49)]
    // Version 9
    [InlineData(230, ECCLevel.L, "LBs30yL9Rec1qFdPwKz4nBrDraY=", 53)]
    [InlineData(180, ECCLevel.M, "c0DN8hFoX6SEkVKr/yVA79SZE4g=", 53)]
    [InlineData(130, ECCLevel.Q, "XdZ3zNyz14Sq0fv9KjonZK7ok04=", 53)]
    [InlineData(98, ECCLevel.H, "9NE1egSXCdGY8AiY4LhHM6sO/jA=", 53)]
    // Version 10
    [InlineData(271, ECCLevel.L, "gyox9Nk2DCPzVeL2E1V/P5XsuNY=", 57)]
    [InlineData(213, ECCLevel.M, "iqS7CNYuwZpw47/SnM8JAcWkhCE=", 57)]
    [InlineData(151, ECCLevel.Q, "vVdh2R+yWmSeDc7iCKonTTcs4ok=", 57)]
    [InlineData(119, ECCLevel.H, "n79CbR/JZZC30sDIDjdFAgurzR4=", 57)]
    // Version 11
    [InlineData(321, ECCLevel.L, "R77LLVhO+/YE8WKmn3CV9f/I9ZY=", 61)]
    [InlineData(251, ECCLevel.M, "l/IFWD6Pkm1TZHFc4ZuFLWDrfdc=", 61)]
    [InlineData(177, ECCLevel.Q, "SxVElF8qBWe0oXXGn57CoI6iglo=", 61)]
    [InlineData(137, ECCLevel.H, "GrHJ2EiDMJ/cXpjcITvypJZZGrY=", 61)]
    // Version 12
    [InlineData(367, ECCLevel.L, "rCv4hIrv0obcHALDSvzN/5zwCfg=", 65)]
    [InlineData(287, ECCLevel.M, "mBC3lYhpNuCa2TbD/h+F6gFH8f4=", 65)]
    [InlineData(203, ECCLevel.Q, "2Gpr+HihG8dDshcf96n2lNopsiM=", 65)]
    [InlineData(155, ECCLevel.H, "eMjatzihLLJH1KZ56GAmaXyf/os=", 65)]
    // Version 13
    [InlineData(425, ECCLevel.L, "mmPOWpjyRZWVC+JRJFDpufEqbUk=", 69)]
    [InlineData(331, ECCLevel.M, "nujBdyCZyO4HoHL4uLYIucd/MA4=", 69)]
    [InlineData(241, ECCLevel.Q, "IT+VAECAcwuZqJdQft5fWo/UTMs=", 69)]
    [InlineData(177, ECCLevel.H, "7JczSXSWYg5XXPhdqLx4Lb411lU=", 69)]
    // Version 14
    [InlineData(458, ECCLevel.L, "a/hMcMmEajVBC3kj8ILzRdGR4t0=", 73)]
    [InlineData(362, ECCLevel.M, "M7D1Cm0FeVbNeiZd+yUPp/8lDfU=", 73)]
    [InlineData(258, ECCLevel.Q, "KAnpA9g4esUdsXHLBpIVvbJ/Dsw=", 73)]
    [InlineData(194, ECCLevel.H, "gs48PFtIXdBTsNB5CIDK4IopcMU=", 73)]
    // Version 15
    [InlineData(520, ECCLevel.L, "L1/Q9lMcmGeNwY4RbBTfFKk2CAQ=", 77)]
    [InlineData(412, ECCLevel.M, "7Z4o8qbi+HXAh5wSBlg/KO8VWl8=", 77)]
    [InlineData(292, ECCLevel.Q, "v52Vt1lJpaEOWkfIsmRMeF2VkZ4=", 77)]
    [InlineData(220, ECCLevel.H, "0c8GsO3CIWhcJYcQDE92+l+w7rQ=", 77)]
    // Version 16
    [InlineData(586, ECCLevel.L, "M66FY6iMOawr2JoEInl0KBKQ1nI=", 81)]
    [InlineData(450, ECCLevel.M, "5u8qfYfrBGxzyesU/xepVqwaWWw=", 81)]
    [InlineData(322, ECCLevel.Q, "QnNtWtlQDmt6cu535YqceOZAyVY=", 81)]
    [InlineData(250, ECCLevel.H, "w2O4eKcEL43ibEH/dDzbNqGDFaM=", 81)]
    // Version 17
    [InlineData(644, ECCLevel.L, "7uG763m0mGPJdY9nwquzdiR4Yu8=", 85)]
    [InlineData(504, ECCLevel.M, "FJuxPTwkgTFHIiFOKfdMMjins2Y=", 85)]
    [InlineData(364, ECCLevel.Q, "JPuf2oD8xEeJSY/bhIO7VCbDxfI=", 85)]
    [InlineData(280, ECCLevel.H, "iepqbSMD9KO0jdaBHdDD3CN/ELA=", 85)]
    // Version 18
    [InlineData(718, ECCLevel.L, "tg9fRcelrfpz1muMC3bp9Rd+d+Q=", 89)]
    [InlineData(560, ECCLevel.M, "ZiJ3ALPxKefddqcbFsaLVtaqu4M=", 89)]
    [InlineData(394, ECCLevel.Q, "TetEWsqYm2DnePzsBN2n2TZI1qw=", 89)]
    [InlineData(310, ECCLevel.H, "g3SqiNtegQKKWz0fphMJNbMnauI=", 89)]
    // Version 19
    [InlineData(792, ECCLevel.L, "G3wlYoJhuxgOMAhwlBSlenIPzQE=", 93)]
    [InlineData(624, ECCLevel.M, "zlQfupN9mxSqabm5IH0Au5UltHA=", 93)]
    [InlineData(442, ECCLevel.Q, "ZTqF6EL1yCtaRxB2/fwuOUQVyDo=", 93)]
    [InlineData(338, ECCLevel.H, "R2zVkSuv/xkSn5tz4RW/8z1Tu+U=", 93)]
    // Version 20
    [InlineData(858, ECCLevel.L, "picn/dsww2hy+2gWQCFJfWRryvM=", 97)]
    [InlineData(666, ECCLevel.M, "65jjl86TbBbuzJk6n42DYMGWVtI=", 97)]
    [InlineData(482, ECCLevel.Q, "78k1kZhh228wPC1HNLuHR2E2rXI=", 97)]
    [InlineData(382, ECCLevel.H, "cjCcbhd7GuNpePeTVeEXS11ZrXk=", 97)]
    // Version 21
    [InlineData(929, ECCLevel.L, "88t3Y6RQA+g+6r8mp9RUuMkZGP0=", 101)]
    [InlineData(711, ECCLevel.M, "Xlq1Xfk881mzrCO+Iu8kK8brGBg=", 101)]
    [InlineData(509, ECCLevel.Q, "KkN2utAu40CZuKCUN0jLWJ+Vd6o=", 101)]
    [InlineData(403, ECCLevel.H, "L44GKy1dAMakEhA4UO3rZscrjys=", 101)]
    // Version 22
    [InlineData(1003, ECCLevel.L, "QN5/o8D8VuuH8hDP+DIItgKwPb0=", 105)]
    [InlineData(779, ECCLevel.M, "K6ss7doERSJbYd2EAWWgB8q/tjQ=", 105)]
    [InlineData(565, ECCLevel.Q, "uO4I8RZ1QBgOdlV36LGFCHeNOzk=", 105)]
    [InlineData(439, ECCLevel.H, "wdAxJat/xdz5D/A53Twe7LijnHg=", 105)]
    // Version 23
    [InlineData(1091, ECCLevel.L, "ZIkAmmyIUotcDSfA4APqqBrb1WY=", 109)]
    [InlineData(857, ECCLevel.M, "7h/MVR1ognfK4SVDRmfgyy7UJVA=", 109)]
    [InlineData(611, ECCLevel.Q, "NYW8AFfV48ojWSAiZjnMYR98o78=", 109)]
    [InlineData(461, ECCLevel.H, "7rQgH3LE/YyOjpqH3XficevFSaU=", 109)]
    // Version 24
    [InlineData(1171, ECCLevel.L, "0pNvS9HfwsAOLG6/U1PXKMyoc6I=", 113)]
    [InlineData(911, ECCLevel.M, "555aDmkhsRGE2li1z81j6mVMh7s=", 113)]
    [InlineData(661, ECCLevel.Q, "UJQmA6QoWlN3r8BEF+zIFvWsoJ4=", 113)]
    [InlineData(511, ECCLevel.H, "emTvRmvoSWFRHLljWXOzvUjpLX0=", 113)]
    // Version 25
    [InlineData(1273, ECCLevel.L, "vf+HWAPl5vJXIKHrZCPHjAInuo4=", 117)]
    [InlineData(997, ECCLevel.M, "v+H4HXOL3tO5/QIsK1IYPGu+zA0=", 117)]
    [InlineData(715, ECCLevel.Q, "rc+LXSs8ILA84TabFJ5b45gX0n8=", 117)]
    [InlineData(535, ECCLevel.H, "ZidXJ+kT23SwS9+xbLZ755ATt4g=", 117)]
    // Version 26
    [InlineData(1367, ECCLevel.L, "QPNW+iYKQdJClPaZsuTUju6dsQE=", 121)]
    [InlineData(1059, ECCLevel.M, "NLTDTTGBmvzR6TOUSxTF/EY4oLI=", 121)]
    [InlineData(751, ECCLevel.Q, "MoOh9EA/7kESiuzy6YHJWcjujMM=", 121)]
    [InlineData(593, ECCLevel.H, "16reau4y5ukinTo0YfM1ToP+/nE=", 121)]
    // Version 27
    [InlineData(1465, ECCLevel.L, "NeSmv01ZOFMeRCxIu6AshMdonOM=", 125)]
    [InlineData(1125, ECCLevel.M, "4xKzxZ7KvzpFAlFmEXYXDHuKh2A=", 125)]
    [InlineData(805, ECCLevel.Q, "LWbwDKIZKwqnfJQncZM+SXJ2qmg=", 125)]
    [InlineData(625, ECCLevel.H, "ks+3sSJU4VFavu6ILO7zFVQZVqc=", 125)]
    // Version 28
    [InlineData(1528, ECCLevel.L, "h64k62PMqcq+hpKSUEFbfNwOlxY=", 129)]
    [InlineData(1190, ECCLevel.M, "H8mNI2+l1xp6LAJKc54t6BcTgXE=", 129)]
    [InlineData(868, ECCLevel.Q, "s+IrQoE960X/7R6mFQyw6w3d0C4=", 129)]
    [InlineData(658, ECCLevel.H, "ugGZT7++E0kKqEi1lOwpgGIuilU=", 129)]
    // Version 29
    [InlineData(1628, ECCLevel.L, "ZJ2nILLA7z8tcMK/NUayp0gd+Ws=", 133)]
    [InlineData(1264, ECCLevel.M, "oVnYNf73ATNLaLNV+Kcsk/kkJ5g=", 133)]
    [InlineData(908, ECCLevel.Q, "G/4ZQB/lgaaP8cfzy6tyQkA4LCk=", 133)]
    [InlineData(698, ECCLevel.H, "snR+O40DDNFn6tGio9X3Qx7Yj6I=", 133)]
    // Version 30
    [InlineData(1732, ECCLevel.L, "QnOSXQy6rgnX3mOEFilBMYagxd4=", 137)]
    [InlineData(1370, ECCLevel.M, "JV+jGwxg94rTaQB1pYEbyznyBg4=", 137)]
    [InlineData(982, ECCLevel.Q, "nI2Xd3XP9ozg+YUns5VN1JvyLqg=", 137)]
    [InlineData(742, ECCLevel.H, "YZKS3IH1kVNvndDJHxQGdRk6WNY=", 137)]
    // Version 31
    [InlineData(1840, ECCLevel.L, "jjvNCmxgZzQnl0gtfx8AXGDzr64=", 141)]
    [InlineData(1452, ECCLevel.M, "8P5rLwbI3czsK65jSnWYHK540ps=", 141)]
    [InlineData(1030, ECCLevel.Q, "Z/+XPRRYe4AL90fdeD2pd9BtEJo=", 141)]
    [InlineData(790, ECCLevel.H, "cauLAUrqzrUlDce6QvLc0QDVb8o=", 141)]
    // Version 32
    [InlineData(1952, ECCLevel.L, "heo6yDq1jE3lrJBGQRWFq6Yxw4Y=", 145)]
    [InlineData(1538, ECCLevel.M, "Owl+uFMlPWFUDa14YKPHl6Iz2rw=", 145)]
    [InlineData(1112, ECCLevel.Q, "iH25k4mUj1LZFfH9RlRU7w1mvgU=", 145)]
    [InlineData(842, ECCLevel.H, "u6fkuEiZbHyROePmzV22/fi4BUQ=", 145)]
    // Version 33
    [InlineData(2068, ECCLevel.L, "d9ATU4zloW1TWbDyFUQRnstXX80=", 149)]
    [InlineData(1628, ECCLevel.M, "jJxIdcbC4JK6ilHF3tCLUN2z12Q=", 149)]
    [InlineData(1168, ECCLevel.Q, "ODr9TeLZGwhJOkKiFTNfzxGeD5E=", 149)]
    [InlineData(898, ECCLevel.H, "FxTOLRMB5YQw1y4Z2mxTOv+5p3g=", 149)]
    // Version 34
    [InlineData(2188, ECCLevel.L, "hOS3RBFGQAIBqVxI1dJD3DMbPiM=", 153)]
    [InlineData(1722, ECCLevel.M, "6ljLKSekU2f3IZSVYt2SmkCwxcQ=", 153)]
    [InlineData(1228, ECCLevel.Q, "CMPPw3e2cUtxuPp6vyW4FLgltdg=", 153)]
    [InlineData(958, ECCLevel.H, "ZHTja2LTLTgrd9Ha0awn5HiB9Pk=", 153)]
    // Version 35
    [InlineData(2303, ECCLevel.L, "pmSX3DjwSMpia+KAM+MYi+jAyN4=", 157)]
    [InlineData(1809, ECCLevel.M, "Uk6fudf3ij96QeYbDKMqyuC7ccY=", 157)]
    [InlineData(1283, ECCLevel.Q, "Sss6YeZZjl/eA019B0vHOMAivG0=", 157)]
    [InlineData(983, ECCLevel.H, "NXJCn5THFStoGeJdQ3gqVfpSQic=", 157)]
    // Version 36
    [InlineData(2431, ECCLevel.L, "3aSKsgEZlsJ6PisZN9f55NPecHg=", 161)]
    [InlineData(1911, ECCLevel.M, "OmKYn++0akH80oMbs/CAUCoHFsY=", 161)]
    [InlineData(1351, ECCLevel.Q, "WWyI18sPU1e4z7/D6/tND0oJhps=", 161)]
    [InlineData(1051, ECCLevel.H, "m38wtnevvqtYxDfS4dM7xDMaI/M=", 161)]
    // Version 37
    [InlineData(2563, ECCLevel.L, "YV6EWRw9/HGfVO0COUoQ94uGRRg=", 165)]
    [InlineData(1989, ECCLevel.M, "/KjlKPlECYqj/pX/Dz3wl9lLKsE=", 165)]
    [InlineData(1423, ECCLevel.Q, "mI+9aaDdXQHxir65csXwE487Zvg=", 165)]
    [InlineData(1093, ECCLevel.H, "f+oGwtL745K4S3x25qNBY8XCGyA=", 165)]
    // Version 38
    [InlineData(2699, ECCLevel.L, "jFAE8Cw77UN/uf8rndzymM7Idwc=", 169)]
    [InlineData(2099, ECCLevel.M, "roNsVHUcMngjE4/GyJop++9eeRs=", 169)]
    [InlineData(1499, ECCLevel.Q, "G2/TnwE1HSnoTytk0mr4552oIos=", 169)]
    [InlineData(1139, ECCLevel.H, "fou4BP/tpD9oxj7KeXyzZ6Fo+xc=", 169)]
    // Version 39
    [InlineData(2809, ECCLevel.L, "IZ2L6FrpTQdjqls020r8YEWiFU8=", 173)]
    [InlineData(2213, ECCLevel.M, "iybO3rmtGmgVRcOYkDnfrH7Scpo=", 173)]
    [InlineData(1579, ECCLevel.Q, "O8jQuoPVybJuJy9ULv/xfJKA29Y=", 173)]
    [InlineData(1219, ECCLevel.H, "FD2FN7bSh8ispau30YQRyNN5LL0=", 173)]
    // Version 40
    [InlineData(2953, ECCLevel.L, "a+80y4pPAhCywuraFrnMSTFRRmo=", 177)]
    [InlineData(2331, ECCLevel.M, "jLKuYZ7beIij+5j9Ko6GRxZVzaA=", 177)]
    [InlineData(1663, ECCLevel.Q, "G1vhiI8anCkTOgeQPQAVH3xcSk8=", 177)]
    [InlineData(1273, ECCLevel.H, "A0HAgMWn4TvnFfSnnEhQ0cVXcNU=", 177)]
    public void can_encode_various_strings_various_ecc(int inputChars, ECCLevel eccLevel, string expectedHash, int expectedSize)
    {
        var input = new string('a', inputChars);
        var gen = new QRCodeGenerator();
        var qrData = gen.CreateQrCode(input, eccLevel);
        (qrData.ModuleMatrix.Count - 8).ShouldBe(expectedSize); // exclude padding
        var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
        var hash = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(result));
        var hashString = Convert.ToBase64String(hash);
        hashString.ShouldBe(expectedHash);
    }
#endif

    [Fact]
    public void validate_alphanumencdict()
    {
        var gen = new QRCodeGenerator();

        var checkString = string.Empty;
        var encoderType = Type.GetType("QRCoder.QRCodeGenerator+AlphanumericEncoder, QRCoder");
        var gField = encoderType.GetField("_alphanumEncDict", BindingFlags.NonPublic | BindingFlags.Static);
        foreach (var listitem in (Dictionary<char, int>)gField.GetValue(gen))
        {
            checkString += $"{listitem.Key},{listitem.Value}:";
        }
        checkString.ShouldBe("0,0:1,1:2,2:3,3:4,4:5,5:6,6:7,7:8,8:9,9:A,10:B,11:C,12:D,13:E,14:F,15:G,16:H,17:I,18:J,19:K,20:L,21:M,22:N,23:O,24:P,25:Q,26:R,27:S,28:T,29:U,30:V,31:W,32:X,33:Y,34:Z,35: ,36:$,37:%,38:*,39:+,40:-,41:.,42:/,43::,44:");
    }

    [Fact]
    public void can_recognize_enconding_numeric()
    {
        var gen = new QRCodeGenerator();
        var method = gen.GetType().GetMethod("GetEncodingFromPlaintext", BindingFlags.NonPublic | BindingFlags.Static);
        var result = (int)method.Invoke(gen, new object[] { "0123456789", false });

        result.ShouldBe(1);
    }


    [Fact]
    public void can_recognize_enconding_alphanumeric()
    {
        var gen = new QRCodeGenerator();
        var method = gen.GetType().GetMethod("GetEncodingFromPlaintext", BindingFlags.NonPublic | BindingFlags.Static);
        var result = (int)method.Invoke(gen, new object[] { "0123456789ABC", false });

        result.ShouldBe(2);
    }


    [Fact]
    public void can_recognize_enconding_forced_bytemode()
    {
        var gen = new QRCodeGenerator();
        var method = gen.GetType().GetMethod("GetEncodingFromPlaintext", BindingFlags.NonPublic | BindingFlags.Static);
        var result = (int)method.Invoke(gen, new object[] { "0123456789", true });

        result.ShouldBe(4);
    }


    [Fact]
    public void can_recognize_enconding_byte()
    {
        var gen = new QRCodeGenerator();
        var method = gen.GetType().GetMethod("GetEncodingFromPlaintext", BindingFlags.NonPublic | BindingFlags.Static);
        var result = (int)method.Invoke(gen, new object[] { "0123456789äöüß", false });

        result.ShouldBe(4);
    }

    [Fact]
    public void can_encode_numeric()
    {
        var gen = new QRCodeGenerator();
        var qrData = gen.CreateQrCode("123", ECCLevel.L);
        var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
        result.ShouldBe("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001111111011111011111110000000010000010010100100000100000000101110100110001011101000000001011101001110010111010000000010111010001010101110100000000100000100001101000001000000001111111010101011111110000000000000000111110000000000000000110110100110101000001000000001110110000001010101100000000000110111100001101110000000000101111010011000001111000000000011101111100010011010000000000000000111110010101100000000111111100010111110001000000001000001000011101110010000000010111010101110110110100000000101110101011100011100000000001011101001100010001110000000010000010101001101010100000000111111101101000001110000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
    }

    [Fact]
    public void can_encode_numeric_2()
    {
        var gen = new QRCodeGenerator();
        var qrData = gen.CreateQrCode("1234567", ECCLevel.L);
        var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
        result.ShouldBe("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001111111011111011111110000000010000010010100100000100000000101110100110001011101000000001011101001110010111010000000010111010001010101110100000000100000100001101000001000000001111111010101011111110000000000000000111110000000000000000110110100110101000001000000000100000000101010111100000000010110110100001101000000000000101110001101000001111000000001110111111000010010010000000000000000100110010011100000000111111100100111111101000000001000001000111101110110000000010111010110110110110100000000101110101101100011100000000001011101000100010011110000000010000010100001101010100000000111111101111000000110000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
    }

    [Fact]
    public void can_encode_numeric_3()
    {
        var gen = new QRCodeGenerator();
        var qrData = gen.CreateQrCode("12345678901", ECCLevel.L);
        var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
        result.ShouldBe("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001111111010111011111110000000010000010001100100000100000000101110101101001011101000000001011101011001010111010000000010111010100100101110100000000100000100111101000001000000001111111010101011111110000000000000000000110000000000000000111100101111110011101000000001110010101011110011110000000010011010000100000010000000000010010010111001110001000000000101101011001001000100000000000000000111100100100100000000111111100111100101101000000001000001001100001101010000000010111010001011111000100000000101110101011001011010000000001011101011001011011000000000010000010111001001101100000000111111101000010010010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
    }

    [Fact]
    public void can_encode_alphanumeric()
    {
        var gen = new QRCodeGenerator();
        var qrData = gen.CreateQrCode("123ABC", ECCLevel.L);
        var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
        result.ShouldBe("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001111111010111011111110000000010000010001100100000100000000101110101101001011101000000001011101011001010111010000000010111010100100101110100000000100000100111101000001000000001111111010101011111110000000000000000000110000000000000000111100101111110011101000000000111100010011110001110000000000100010100100000001000000000011110011111001110011000000001111101110101001000000000000000000000111100100100100000000111111100001100100110000000001000001000100001111110000000010111010010011111010100000000101110101111001011110000000001011101010101011000000000000010000010111001000010000000000111111101010010010010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
    }

    [Fact]
    public void can_encode_byte_long()
    {
        var gen = new QRCodeGenerator();
        var qrData = gen.CreateQrCode("https://github.com/codebude/QRCoder/blob/f89aa90081f369983a9ba114e49cc6ebf0b2a7b1/QRCoder/Framework4.0Methods/Stream4Methods.cs", ECCLevel.H);
        var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
        result.ShouldBe("000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000011111110111011110110000101001110011101111011110001011011111110000000010000010111110111110001010010001100010100000011000011010000010000000010111010110010100110110000100110001010110100101010111010111010000000010111010000110010111011000101111110011010101000100101010111010000000010111010011010111000011000101111100001101111110001110010111010000000010000010110101101010101100011000100100100001011101100010000010000000011111110101010101010101010101010101010101010101010101011111110000000000000000111011011010010111001000100010111011010101000000000000000000000111010111110001111111100111111110110000111011110011111001110000000011111101111010101011111100011110101010001011000111000110100000000000000101111000110111101010010011011000010000110010110101000000110000000000000001111100010111100110100010100011010111100000001000100010000000000101010011011100110011111101000111110001100011111011011011110000000010010001110011110000101111001100100011001111110010010011010100000000001010011010001011011011010111011001110110000001001100101011110000000010110000111100111100010110011101110000001101111001000010110010000000010101010100011010111011110111010010100010100000110111111011010000000000110101111110101110100001101100010101110110010111000011010000000000001011111111001010110101000111001001011011000101011111101010110000000000010000000110100110010011100110100000100110111101001011110000000000010101110011111100011001000110101101111001011000011001001001100000000000100000101111001101001100001000010110010111000001000010010100000000010000011000010110111001110111011010010101000111011110100111110000000001100000100110001100100010101111011011100010110101000010000010000000000001111101011110111110100010100101010101111000010000000011110000000011010100010000111011000110000110001011000111110011010011100100000000010010011011110011110011001101110011001110001111110000000011010000000011101000110001100001110010010001011000000010101101110010110100000000010111111101100100010001000111111100111011101000010101111101000000000011001000110101111011001011001000101101011111100001101000100000000000010001010100000111101011101111010101010000000011110011010110110000000010111000100001011000010010011000100000100110101000011000100100000000000001111101111110100000010101111111100001001001110001111111100000000010111001011011001100001101010001000100111111100101011011000000000000000000011111101001100110000110100010001111001111101100111010110000000010101101101010110010011111101010000100100010000110001011010000000000010100011111001110010000010000111111110110001110111111011111010000000000001000000111110111001100111011011010100110100111011110010000000000000110111001011011100010111100000000010010011111010100110111110000000001110000110101101001100001001000111010011100110001111001100110000000001100011100110101100100101001010001100110101000111111101111000000000011101100100001000100011101010100111110101110010101001001010100000000011111110000111010111100001011011101000110100100101000101001010000000010111101100010010110000011101011011101101101101100001011000110000000010101111001111111101011010100000001000100101001010110110101000000000000011101100001001011001110010110111100101110010100000011001100000000010001111010011100000100100110111011011010111011010111110001110000000010000001111101010111111110001110110001011111101110000100000110000000000100010110000000101000111001011000011100011001011111011111110000000010110101101100011101011100001111101011111111000110010000011000000000000111111100010001000110100000001101111110101011011110011010110000000011101000111010011011110100001010110011111101010001001000000010000000011110011110100001110111110011111101110100001001111011111101110000000000000000100110000001011111101000100101001101100110001000110100000000011111110011101111000110110011010110001110011111101111010111110000000010000010010101100101110011001000111101111010111001011000100110000000010111010100001010110001101111111101101000111010111111111101000000000010111010101001111101101011110000100011100011110011011111101010000000010111010100101010001000010100001001101000100001110100010001010000000010000010001010111111111001100110111100101011111000010110000010000000011111110001011000110010001101010000101110110000110101000011110000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
    }

    [Fact]
    public void can_encode_byte()
    {
        var gen = new QRCodeGenerator();
        var qrData = gen.CreateQrCode("äöü", ECCLevel.L);
        var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
        result.ShouldBe("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001111111001011011111110000000010000010011100100000100000000101110101101101011101000000001011101001010010111010000000010111010001010101110100000000100000100000101000001000000001111111010101011111110000000000000000110110000000000000000111011111111011000100000000001001110001100010000010000000010011110001010001001000000000110011010000001000110000000001110001111001010110110000000000000000111101010011100000000111111101111011100110000000001000001010011101110010000000010111010110101110010100000000101110100110001000110000000001011101011001000100010000000010000010100000100011000000000111111101110101010111000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
    }

    [Fact]
    public void can_encode_utf8()
    {
        var gen = new QRCodeGenerator();
        var qrData = gen.CreateQrCode("https://en.wikipedia.org/wiki/🍕", ECCLevel.L, true, false, QRCodeGenerator.EciMode.Utf8);
        var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
        result.ShouldBe("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000011111110101011010011101111111000000001000001001111100001110100000100000000101110101110000011000010111010000000010111010111010111100101011101000000001011101010011010111010101110100000000100000100011010001110010000010000000011111110101010101010101111111000000000000000000101000011100000000000000000111100101011110101011100111010000000001011000101011111010011101010000000001010011101111101001111011101000000000111011011110000010001100000100000000000000010011010101100000000000000000001100110101011011111001101110000000000000011100001010101010110101000000000000111001011100110111111110011000000001110101011001011001000100011000000000000101010100001010111111000000000000010111010101001111100000001110000000000010110100010111111100100010100000000011101111010011101111111101010000000000000000110000001000100010010000000001111111001100011001010101101000000000100000100111111111011000111000000000010111010010100011010111110111000000001011101010110100011100101011000000000101110101100101111100101111010000000010000010111011001111000001101000000001111111011110000100000110101000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
    }

    [Fact]
    public void can_encode_utf8_bom()
    {
        var gen = new QRCodeGenerator();
        var qrData = gen.CreateQrCode("https://en.wikipedia.org/wiki/🍕", ECCLevel.L, true, true, QRCodeGenerator.EciMode.Utf8);
        var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
        result.ShouldBe("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000011111110010001101010101111111000000001000001011011000110000100000100000000101110100111010101111010111010000000010111010110100100010101011101000000001011101000101111000010101110100000000100000101010000111000010000010000000011111110101010101010101111111000000000000000000001010101110000000000000000111110111110101010100101010100000000000100000110000101000001100101000000000001001001011000011010000111100000000100010001111000001111110111010000000010110111010100011100100101111000000000001010001101101001000010100100000000100001101110011001010000001010000000001011001100011001111111010111000000000010001010101011110010100000100000000100100010000000000010110010000000000010110110010110000101010101100000000001001100100010010100111101101100000000101010110011000111101111100100000000000000000111011110011100011010000000001111111011100110010010101110000000000100000100100110010101000110110000000010111010110010111101111110011000000001011101010100000100010110100000000000101110101001100111110110111100000000010000010111100101111100100001000000001111111011110001110100111000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
    }

    [Fact]
    public void can_generate_from_bytes()
    {
        byte[] test_data = { 49, 50, 51, 65, 66, 67 }; //123ABC
        var gen = new QRCodeGenerator();
        var qrData = gen.CreateQrCode(test_data, ECCLevel.L);
        var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
        result.ShouldBe("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001111111011001011111110000000010000010010010100000100000000101110101010101011101000000001011101010010010111010000000010111010111000101110100000000100000100000001000001000000001111111010101011111110000000000000000011000000000000000000111100101010010011101000000001011100001001001001110000000010101011111011111110100000000000101000000110000000000000001011001001010100110000000000000000000110001000101000000000111111100110011011110000000001000001001111110111010000000010111010011100100101100000000101110101110010010010000000001011101011010100011000000000010000010110110101000100000000111111101011100010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
    }

    [Fact]
    public void trim_leading_zeros_works()
    {
        var gen = new QRCodeGenerator();
        var qrData = gen.CreateQrCode("this is a test", ECCLevel.M);
        var result = string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
        result.ShouldBe("0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001111111001101011111110000000010000010010000100000100000000101110101101101011101000000001011101010001010111010000000010111010101010101110100000000100000101010101000001000000001111111010101011111110000000000000000110010000000000000000101111100011101111100000000001110100011110001100010000000001100011010110010011000000000100111000011010011100000000001001011001101011000100000000000000000100100001001100000000111111100111110001110000000001000001010011000011010000000010111010101110111101100000000101110101000000110100000000001011101011111000010000000000010000010010011010010000000000111111101101111100010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
    }

    [Fact]
    public void isValidIso_works()
    {
        // see private method: QRCodeGenerator.IsValidISO

        var _iso88591ExceptionFallback = Encoding.GetEncoding(28591, new EncoderExceptionFallback(), new DecoderExceptionFallback()); // ISO-8859-1

        IsValidISO("abc").ShouldBeTrue();
        IsValidISO("äöü").ShouldBeTrue();
        IsValidISO("🍕").ShouldBeFalse();

        bool IsValidISO(string input)
        {
            try
            {
                _ = _iso88591ExceptionFallback.GetByteCount(input);
                return true;
            }
            catch (EncoderFallbackException)
            {
                return false;
            }
        }
    }

    [Fact]
    public void ecc_level_from_payload_works()
    {
        var stringValue = "this is a test";

        // set up baselines
        var expectedL = Encode(QRCodeGenerator.GenerateQrCode(stringValue, QRCodeGenerator.ECCLevel.L));
        var expectedM = Encode(QRCodeGenerator.GenerateQrCode(stringValue, QRCodeGenerator.ECCLevel.M));
        var expectedQ = Encode(QRCodeGenerator.GenerateQrCode(stringValue, QRCodeGenerator.ECCLevel.Q));
        var expectedH = Encode(QRCodeGenerator.GenerateQrCode(stringValue, QRCodeGenerator.ECCLevel.H));

        // ensure that the baselines are different from each other
        expectedL.ShouldNotBe(expectedM);
        expectedL.ShouldNotBe(expectedQ);
        expectedL.ShouldNotBe(expectedH);
        expectedM.ShouldNotBe(expectedQ);
        expectedM.ShouldNotBe(expectedH);
        expectedQ.ShouldNotBe(expectedH);

        // validate that any ECC level can be used when the payload specifies a default ECC level
        var payloadDefault = new SamplePayload(stringValue, QRCodeGenerator.ECCLevel.Default);
        Encode(QRCodeGenerator.GenerateQrCode(payloadDefault)).ShouldBe(expectedM);
        Encode(QRCodeGenerator.GenerateQrCode(payloadDefault, QRCodeGenerator.ECCLevel.Default)).ShouldBe(expectedM);
        Encode(QRCodeGenerator.GenerateQrCode(payloadDefault, QRCodeGenerator.ECCLevel.L)).ShouldBe(expectedL);
        Encode(QRCodeGenerator.GenerateQrCode(payloadDefault, QRCodeGenerator.ECCLevel.M)).ShouldBe(expectedM);
        Encode(QRCodeGenerator.GenerateQrCode(payloadDefault, QRCodeGenerator.ECCLevel.Q)).ShouldBe(expectedQ);
        Encode(QRCodeGenerator.GenerateQrCode(payloadDefault, QRCodeGenerator.ECCLevel.H)).ShouldBe(expectedH);

        // validate that the ECC level specified in the payload is used when default is specified,
        //   or checks that the selected ECC level matches the payload ECC level, throwing an exception otherwise
        Verify(QRCodeGenerator.ECCLevel.L, expectedL);
        Verify(QRCodeGenerator.ECCLevel.M, expectedM);
        Verify(QRCodeGenerator.ECCLevel.Q, expectedQ);
        Verify(QRCodeGenerator.ECCLevel.H, expectedH);


        void Verify(QRCodeGenerator.ECCLevel eccLevel, string expected)
        {
            var payload = new SamplePayload(stringValue, eccLevel);
            Encode(QRCodeGenerator.GenerateQrCode(payload)).ShouldBe(expected);
            foreach (var ecc in Enum.GetValues(typeof(QRCodeGenerator.ECCLevel)).Cast<QRCodeGenerator.ECCLevel>())
            {
                if (ecc == eccLevel || ecc == QRCodeGenerator.ECCLevel.Default)
                    Encode(QRCodeGenerator.GenerateQrCode(payload, ecc)).ShouldBe(expected);
                else
                    Should.Throw<ArgumentOutOfRangeException>(() => Encode(QRCodeGenerator.GenerateQrCode(payload, ecc)));
            }
        }

        string Encode(QRCodeData qrData) => string.Join("", qrData.ModuleMatrix.Select(x => x.ToBitString()).ToArray());
    }

    private class SamplePayload : PayloadGenerator.Payload
    {
        private readonly string _data;
        private readonly QRCodeGenerator.ECCLevel _eccLevel;

        public SamplePayload(string data, QRCodeGenerator.ECCLevel eccLevel)
        {
            _data = data;
            _eccLevel = eccLevel;
        }

        public override QRCodeGenerator.ECCLevel EccLevel => _eccLevel;

        public override string ToString() => _data;
    }
}

public static class ExtensionMethods
{
    public static string ToBitString(this BitArray bits)
    {
        var sb = new StringBuilder();
        int bitLength = bits.Length;
        for (int i = 0; i < bitLength; i++)
        {
            char c = bits[i] ? '1' : '0';
            sb.Append(c);
        }

        return sb.ToString();
    }
}
