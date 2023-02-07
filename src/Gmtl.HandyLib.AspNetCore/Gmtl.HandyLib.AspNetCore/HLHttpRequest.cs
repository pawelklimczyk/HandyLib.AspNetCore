using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Gmtl.HandyLib.AspNetCore
{
    public static class HLHttpRequest
    {
        /// <summary>
        /// Check if request contains expected key
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key">case insensitive key</param>
        /// <returns></returns>
        public static bool HasQueryString(this HttpRequest request, string key)
        {
            return request.Query.ContainsKey(key);
        }

        /// <summary>
        /// Returns value for querystring key or String.Empty
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key">case insensitive key</param>
        /// <returns></returns>
        public static string HasQueryStringValue(this HttpRequest request, string key)
        {
            return HasQueryString(request, key) ? request.Query[key] : string.Empty;
        }

        public static async Task<string> GetRawBodyAsync(this HttpRequest request, Encoding encoding = null)
        {
            if (!request.Body.CanSeek)
            {
                // We only do this if the stream isn't *already* seekable,
                // as EnableBuffering will create a new stream instance
                // each time it's called
                request.EnableBuffering();
            }

            request.Body.Position = 0;

            var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8);

            var body = await reader.ReadToEndAsync().ConfigureAwait(false);

            request.Body.Position = 0;

            return body;
        }
    }
}
