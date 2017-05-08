using System;

namespace QRCoder
{
    internal static class String40Methods
    {
        /// <summary>
        /// The IsNullOrWhiteSpace method from Framework4.0
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the <paramref name="value"/> is null or white space; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrWhiteSpace(String value)
        {
            if (value == null) return true;

            for (int i = 0; i < value.Length; i++)
            {
                if (!Char.IsWhiteSpace(value[i])) return false;
            }

            return true;
        }

        public static string ReverseString(string str)
        {
            char[] chars = str.ToCharArray();
            char[] result = new char[chars.Length];
            for (int i = 0, j = str.Length - 1; i < str.Length; i++, j--)
            {
                result[i] = chars[j];
            }
            return new string(result);
        }

        public static bool IsAllDigit(string str)
        {
            foreach (var c in str)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}