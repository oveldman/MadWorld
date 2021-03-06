using System;
namespace API.Models
{
    public static class ApiSettings
    {
        public static string Issuer { get; private set; }
        public static string Url { get; private set; }

        private static bool Initialized = false;

        public static void SetSettings(string apiUrl, string issuer)
        {
            if (Initialized) return;
            Initialized = true;

            Issuer = issuer;
            Url = apiUrl;
        }
    }
}
