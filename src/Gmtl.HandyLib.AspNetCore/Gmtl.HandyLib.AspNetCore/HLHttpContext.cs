using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
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
            var obj = new Dictionary<string, object>();

            var builder = new StringBuilder();

            if (ctx.Request.Host.HasValue)
            {
                obj["Host"] = ctx.Request.Host.Value;
            }

            obj["Scheme"] = ctx.Request.Scheme;

            if (ctx.Request.Path.HasValue)
            {
                obj["Path"] = ctx.Request.Path.Value;
            }
            
            if (!string.IsNullOrWhiteSpace(ctx.Request.ContentType))
            {
                obj["ContentType"] = ctx.Request.ContentType;
            }

            if (ctx.Request.ContentLength.HasValue)
            {
                obj["ContentLength"] = ctx.Request.ContentLength.Value;
            }

            if (ctx.Request.QueryString.HasValue)
            {
                obj["QueryString"] = ctx.Request.QueryString.Value;
            }

            foreach (var key in ctx.Request.Headers.Keys)
            {
                obj[$"Header_{key}"] = ctx.Request.Headers[key];
            }

            if (ctx.Request.HasFormContentType)
            {
                foreach (var key in ctx.Request.Form.Keys)
                    obj[$"Form_{key}"] = ctx.Request.Form[key];
            }

            foreach (var key in ctx.Request.Cookies.Keys)
            {
                obj[$"Cookie_{key}"] = ctx.Request.Cookies[key];
            }

            if (ctx.User != null)
            {
                foreach (var identity in ctx.User.Identities)
                {
                    obj[$"User_Identity_{identity.Name}_isAuthenticated"] = identity.IsAuthenticated;
                    obj[$"User_Identity_{identity.Name}_name"] = identity.Name;
                    obj[$"User_Identity_{identity.Name}_claims"] = string.Join(",", identity.Claims.Select(c => $"{c.Type} = {c.Value}"));
                }
            }

            return JsonSerializer.Serialize(obj);
        }
    }
}
