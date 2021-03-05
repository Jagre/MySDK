using Flurl.Http;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MySDK.URL
{
    /// <summary>
    /// Get Request Extension
    /// </summary>
    public static class GetRequestGeneratedExtension
    {
        /// <summary>
        /// request url that specified by yourself with get method. 
        /// according the package Flurl.Http.GeneratedExtensions to rewrite it in order to debug, develop.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="completionOption"></param>
        /// <returns></returns>
        public static Task<T> GetJsonAsync<T>(this Flurl.Url url, CancellationToken cancellationToken = default(CancellationToken), HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            var request = new FlurlRequest(url);
            var result = request.GetJsonAsync<T>(cancellationToken, completionOption);
            return result;
        }

        /// <summary>
        /// request url that specified by yourself with get method. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="completionOption"></param>
        /// <returns></returns>
        public static Task<T> GetJsonAsync<T>(this string url, CancellationToken cancellationToken = default(CancellationToken), HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            Flurl.Url urlObj = url;
            return urlObj.GetJsonAsync<T>(cancellationToken, completionOption);
        }
    }
}
