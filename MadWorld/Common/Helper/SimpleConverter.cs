using System;
using System.Text;

namespace Common.Helper
{
    /// <summary>
    /// This class helps you with simple converters like Base64 and more soon.
    /// </summary>
    public static class SimpleConverter
    {
        /// <summary>
        /// From plaintext to base64.
        /// </summary>
        public static string ConvertToBase64(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return string.Empty;

            byte[] body = Encoding.ASCII.GetBytes(plainText);
            return Convert.ToBase64String(body);
        }

        /// <summary>
        /// From base64 to plaintext.
        /// </summary>
        public static string ConvertToString(string base64Text)
        {
            if (string.IsNullOrEmpty(base64Text)) return string.Empty;

            byte[] body = Convert.FromBase64String(base64Text);
            return Encoding.ASCII.GetString(body);
        }
    }
}
