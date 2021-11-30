using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace QRCoder2.Payloads
{
    public abstract class PayloadBase
    {
        public virtual int Version => -1;
        public virtual ECCLevel EccLevel => ECCLevel.M;
        public virtual EciMode EciMode => EciMode.Default;
        public abstract override string ToString();
        
        protected static string EscapeInput(string inp, bool simple = false)
        {
            char[] forbiddenChars = {'\\', ';', ',', ':'};
            if (simple)
            {
                forbiddenChars = new char[1] {':'};
            }
            foreach (var c in forbiddenChars)
            {
                inp = inp.Replace(c.ToString(), "\\" + c);
            }
            return inp;
        }
        
        protected static bool IsHexStyle(string inp)
        {
            return (System.Text.RegularExpressions.Regex.IsMatch(inp, @"\A\b[0-9a-fA-F]+\b\Z") || System.Text.RegularExpressions.Regex.IsMatch(inp, @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"));
        }
        
        protected static bool IsValidIban(string iban)
        {
            //Clean IBAN
            var ibanCleared = iban.ToUpper().Replace(" ", "").Replace("-", "");

            //Check for general structure
            var structurallyValid = Regex.IsMatch(ibanCleared, @"^[a-zA-Z]{2}[0-9]{2}([a-zA-Z0-9]?){16,30}$");

            //Check IBAN checksum
            var checksumValid = false;
            var sum = $"{ibanCleared.Substring(4)}{ibanCleared.Substring(0, 4)}".ToCharArray().Aggregate("",
                (current, c) => current + (char.IsLetter(c) ? (c - 55).ToString() : c.ToString()));
            int m = 0;
            for (int i = 0; i < (int)Math.Ceiling((sum.Length - 2) / 7d); i++)
            {
                var offset = (i == 0 ? 0 : 2);
                var start = i * 7 + offset;
                var n = (i == 0 ? "" : m.ToString()) +
                        sum.Substring(start, Math.Min(9 - offset, sum.Length - start));
                if (!int.TryParse(n, NumberStyles.Any, CultureInfo.InvariantCulture, out m))
                    break;
                m = m % 97;
            }

            checksumValid = m == 1;
            return structurallyValid && checksumValid;
        }
        
        protected static bool IsValidBic(string bic)
        {
            return Regex.IsMatch(bic.Replace(" ", ""), @"^([a-zA-Z]{4}[a-zA-Z]{2}[a-zA-Z0-9]{2}([a-zA-Z0-9]{3})?)$");
        }
    }
}