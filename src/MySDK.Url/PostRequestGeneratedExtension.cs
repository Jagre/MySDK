using Flurl.Http;
using Flurl.Http.Content;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace MySDK.FlurlHttp
{
    /// <summary>
    /// Post Request Extension
    /// </summary>
    public static class PostRequestGeneratedExtension
    {
        /// <summary>
        /// request url that specified by yourself with post method. 
        /// according the package Flurl.Http.GeneratedExtensions to rewrite it in order to debug, develop.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="completionOption"></param>
        /// <returns></returns>
        public static Task<T> PostJsonAsync<T>(this Flurl.Url url, object data, CancellationToken cancellationToken = default(CancellationToken), HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            var request = new FlurlRequest(url);
            CapturedJsonContent content = new CapturedJsonContent(request.Settings.JsonSerializer.Serialize(data));
            var response = request.SendAsync(HttpMethod.Post, content, cancellationToken, completionOption);
            var result = response.ReceiveJson<T>();
            return result;
        }

        /// <summary>
        /// request url that specified by yourself with post method. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="completionOption"></param>
        /// <returns></returns>
        public static Task<T> PostJsonAsync<T>(this string url, object data, CancellationToken cancellationToken = default(CancellationToken), HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            Flurl.Url urlObj = url;
            return urlObj.PostJsonAsync<T>(data, cancellationToken, completionOption);
        }

    }
}
