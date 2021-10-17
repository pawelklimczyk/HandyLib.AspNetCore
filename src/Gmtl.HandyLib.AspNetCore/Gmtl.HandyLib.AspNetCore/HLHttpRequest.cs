using Microsoft.AspNetCore.Http;

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
    }
}
