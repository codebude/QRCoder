using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
#if NETSTANDARD1_3
using System.Reflection;
#endif

namespace QRCoder;

/// <summary>
/// Provides utility methods for generating QR code payloads.
/// </summary>
public static partial class PayloadGenerator
{
    /// <summary>
    /// Validates the structure and checksum of an IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    private static bool IsValidIban(string iban)
    {
        //Clean IBAN
        var ibanCleared = iban.ToUpper().Replace(" ", "").Replace("-", "");

        //Check for general structure
        var structurallyValid = Regex.IsMatch(ibanCleared, @"^[a-zA-Z]{2}[0-9]{2}([a-zA-Z0-9]?){16,30}$");

        //Check IBAN checksum
        var checksumValid = false;
        var sum = $"{ibanCleared.Substring(4)}{ibanCleared.Substring(0, 4)}".ToCharArray().Aggregate("", (current, c) => current + (char.IsLetter(c) ? (c - 55).ToString() : c.ToString()));
        int m = 0;
        for (int i = 0; i < (int)Math.Ceiling((sum.Length - 2) / 7d); i++)
        {
            var offset = (i == 0 ? 0 : 2);
            var start = i * 7 + offset;
            var n = (i == 0 ? "" : m.ToString()) + sum.Substring(start, Math.Min(9 - offset, sum.Length - start));
            if (!int.TryParse(n, NumberStyles.Any, CultureInfo.InvariantCulture, out m))
                break;
            m %= 97;
        }
        checksumValid = m == 1;
        return structurallyValid && checksumValid;
    }

    /// <summary>
    /// Validates the structure and checksum of a QR IBAN.
    /// </summary>
    /// <param name="iban">The QR IBAN to validate.</param>
    /// <returns>True if the QR IBAN is valid; otherwise, false.</returns>
    private static bool IsValidQRIban(string iban)
    {
        var foundQrIid = false;
        try
        {
            var ibanCleared = iban.ToUpper().Replace(" ", "").Replace("-", "");
            var possibleQrIid = Convert.ToInt32(ibanCleared.Substring(4, 5));
            foundQrIid = possibleQrIid >= 30000 && possibleQrIid <= 31999;
        }
        catch { }
        return IsValidIban(iban) && foundQrIid;
    }

    /// <summary>
    /// Validates the structure of a BIC.
    /// </summary>
    /// <param name="bic">The BIC to validate.</param>
    /// <returns>True if the BIC is valid; otherwise, false.</returns>
    private static bool IsValidBic(string bic)
        => Regex.IsMatch(bic.Replace(" ", ""), @"^([a-zA-Z]{4}[a-zA-Z]{2}[a-zA-Z0-9]{2}([a-zA-Z0-9]{3})?)$");


    /// <summary>
    /// Converts a string to a specified encoding.
    /// </summary>
    /// <param name="message">The string to convert.</param>
    /// <param name="encoding">The encoding to use.</param>
    /// <returns>The converted string.</returns>
    private static string ConvertStringToEncoding(string message, string encoding)
    {
        var iso = Encoding.GetEncoding(encoding);
        var utf8 = Encoding.UTF8;
        byte[] utfBytes = utf8.GetBytes(message);
        byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
        return iso.GetString(isoBytes);
    }

    /// <summary>
    /// Escapes forbidden characters in a string.
    /// </summary>
    /// <param name="inp">The input string.</param>
    /// <param name="simple">Indicates whether to use a simple escape mode.</param>
    /// <returns>The escaped string.</returns>
    private static string EscapeInput(string inp, bool simple = false)
    {
        char[] forbiddenChars = { '\\', ';', ',', ':' };
        if (simple)
        {
            forbiddenChars = new char[1] { ':' };
        }
        foreach (var c in forbiddenChars)
        {
            inp = inp.Replace(c.ToString(), "\\" + c);
        }
        return inp;
    }



    /// <summary>
    /// Validates a string using the Mod10 checksum algorithm.
    /// </summary>
    /// <param name="digits">The string to validate.</param>
    /// <returns>True if the string is valid; otherwise, false.</returns>
    public static bool ChecksumMod10(string digits)
    {
        if (string.IsNullOrEmpty(digits) || digits.Length < 2)
            return false;
        int[] mods = new int[] { 0, 9, 4, 6, 8, 2, 7, 1, 3, 5 };

        int remainder = 0;
        for (int i = 0; i < digits.Length - 1; i++)
        {
            var num = Convert.ToInt32(digits[i]) - 48;
            remainder = mods[(num + remainder) % 10];
        }
        var checksum = (10 - remainder) % 10;
        return checksum == Convert.ToInt32(digits[digits.Length - 1]) - 48;
    }

    /// <summary>
    /// Checks if a string is in hexadecimal format.
    /// </summary>
    /// <param name="inp">The input string.</param>
    /// <returns>True if the string is in hexadecimal format; otherwise, false.</returns>
    private static bool isHexStyle(string inp)
        => System.Text.RegularExpressions.Regex.IsMatch(inp, @"\A\b[0-9a-fA-F]+\b\Z") || System.Text.RegularExpressions.Regex.IsMatch(inp, @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z");
}
