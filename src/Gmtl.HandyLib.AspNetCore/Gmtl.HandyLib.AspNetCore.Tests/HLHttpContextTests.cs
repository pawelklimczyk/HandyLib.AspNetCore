using Microsoft.AspNetCore.Http;
using Xunit;

namespace Gmtl.HandyLib.AspNetCore.Tests
{
    public class HLHttpContextTests
    {
        [Theory]
        [InlineData("https", "www.test.com", "/test/123", "?a=1&b=2", "https://www.test.com/test/123/?a=1&b=2")]
        [InlineData("https", "www.test.com", "/test/123/", "?a=1&b=2", "https://www.test.com/test/123/?a=1&b=2")]
        [InlineData("https", "www.test.com", "/test/123/", "", "https://www.test.com/test/123/")]
        [InlineData("https", "www.test.com", "/test/123", "", "https://www.test.com/test/123")]
        public void Should(string scheme, string host, string path, string queryString, string expected)
        {
            DefaultHttpContext ctx = new DefaultHttpContext();
            ctx.Request.Scheme = scheme;
            ctx.Request.Host = new HostString(host);
            ctx.Request.Path = path;

            if (!string.IsNullOrWhiteSpace(queryString))
                ctx.Request.QueryString = new QueryString(queryString);

            string actual = ctx.ToStringExtended();

            Assert.Equal(expected, actual);
        }
    }
}
