using Microsoft.AspNetCore.Http;

namespace Gmtl.HandyLib.AspNetCore
{
    public static class HLHttpContext
    {
        public static string ToStringExtended(this HttpContext ctx)
        {
            string url = $"{ctx.Request.Scheme}://{ctx.Request.Host}{ctx.Request.Path}";

            if (ctx.Request.QueryString.HasValue)
            {
                if (!ctx.Request.Path.Value.EndsWith("/"))
                {
                    url += "/";
                }
                return $"{url}{ctx.Request.QueryString}";
            }

            return url;
        }
    }
}
