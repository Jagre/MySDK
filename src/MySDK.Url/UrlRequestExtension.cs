using Microsoft.AspNetCore.Http;
using System;

namespace MySDK.URL
{
    public static class UrlRequestExtension
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return "XMLHttpRequest".Equals(request.Query["X-Requested-With"], StringComparison.Ordinal)
                || "XMLHttpRequest".Equals(request.Headers["X-Requested-With"], StringComparison.Ordinal);
        }

        public static string GetRequestScheme(this HttpRequest request)
        {
            if (!string.IsNullOrEmpty(request.Headers["X-Scheme"]))
            {
                return request.Headers["X-Scheme"];
            }
            else if (!string.IsNullOrEmpty(request.Headers["X-Http-Scheme"]))
            {
                return request.Headers["X-Http-Scheme"];
            }
            else
            {
                return request.Scheme;
            }
        }

        public static string GetRequestUrl(this HttpRequest request)
        {
            if (null == request)
                return string.Empty;

            var scheme = request.Scheme;
            if (string.IsNullOrEmpty(scheme))
                throw new ArgumentException("the argument request object hasn't specified Scheme property.");

            var host = request.Host.HasValue ? request.Host.ToString() : string.Empty;
            if (string.IsNullOrEmpty(host))
                throw new ArgumentException("the argument request object hasn't specified Host property.");

            var pathBase = request.PathBase.HasValue ? request.PathBase.ToString() : string.Empty;
            var path = request.Path.HasValue ? request.Path.ToString() : string.Empty;
            var queryString = request.QueryString.HasValue ? request.QueryString.ToString() : string.Empty;

            return $"{scheme}://{host}{pathBase}{path}{queryString}";
        }



    }
}
