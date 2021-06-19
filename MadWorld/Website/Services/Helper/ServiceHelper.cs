using System;
using System.Collections.Generic;
using System.Linq;
using Website.Services.Models;

namespace Website.Services.Helper
{
    public static class ServiceHelper
    {
        public static string BuildUrl(string url, List<UrlParameter> urlParameters)
        {
            if (urlParameters == null || !urlParameters.Any()) return url;

            url = $"{url}?";

            for (int i = 0; i < urlParameters.Count; i++)
            {
                url += $"{urlParameters[i].Name}={urlParameters[i].Value}";

                if (i != (urlParameters.Count - 1))
                {
                    url += "&";
                }
            }

            return url;
        }
    }
}
