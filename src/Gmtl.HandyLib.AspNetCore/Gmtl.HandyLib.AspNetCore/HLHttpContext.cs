using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

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

        public static string Serialize(this HttpContext ctx)
        {
            Dictionary<string, object> obj = new Dictionary<string, object>();

            var builder = new StringBuilder();


            if (ctx.Request.QueryString.HasValue)
            {
                obj["QueryString"] = ctx.Request.QueryString.Value;
            }

            if (ctx.Request.HasFormContentType)
            {
                obj["Form"] = ctx.Request.Form;
            }

            if (!string.IsNullOrWhiteSpace(ctx.Request.ContentType))
            {
                obj["ContentType"] = ctx.Request.ContentType;
            }

            if (ctx.Request.ContentLength.HasValue)
            {
                obj["ContentLength"] = ctx.Request.ContentLength.Value;
            }
            obj["Cookies"] = ctx.Request.Cookies;
            obj["User"] = ctx.User;
            obj["Headers"] = ctx.Request.Headers;

            if (ctx.Request.Host.HasValue)
            {
                obj["Host"] = ctx.Request.Host.Value;
            }

            obj["Scheme"] = ctx.Request.Scheme;

            if (ctx.Request.Path.HasValue)
            {
                obj["Path"] = ctx.Request.Path.Value;
            }

            return JsonSerializer.Serialize(obj);
        }
    }
}
