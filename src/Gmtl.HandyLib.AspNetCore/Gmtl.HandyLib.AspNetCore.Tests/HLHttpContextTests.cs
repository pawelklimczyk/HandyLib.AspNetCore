using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Serialize_ReturnsValidJson(bool withUser)
        {
            var context = CreateHttpContext(withUser);
            var json = context.Serialize();

            var obj = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            Assert.NotNull(obj);
            Assert.Equal("?key=value", obj["QueryString"].ToString());
            Assert.Equal("application/x-www-form-urlencoded", obj["ContentType"].ToString());
            Assert.Equal("localhost", obj["Host"].ToString());
            Assert.Equal("https", obj["Scheme"].ToString());
            Assert.Equal("[\"Bearer token\"]", obj["Header_Authorization"].ToString());
            Assert.Equal("/test", obj["Path"].ToString());

            Assert.Equal("[\"formValue\"]", obj["Form_formKey"].ToString());
            Assert.Equal("[\"formValue2\"]", obj["Form_formKey2"].ToString());

            if (withUser)
            {
                Assert.NotNull(obj["User_Identity_TestUser_name"]);
            }
        }

        private HttpContext CreateHttpContext(bool withUser)
        {
            var context = A.Fake<HttpContext>();
            var request = A.Fake<HttpRequest>();
            var response = A.Fake<HttpResponse>();
            var headers = new HeaderDictionary { { "Authorization", "Bearer token" } };
            var cookies = A.Fake<IRequestCookieCollection>();
            var user = withUser ? new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "TestUser") })) : null;
            var formCollection = new FormCollection(new Dictionary<string, StringValues> { { "formKey", "formValue" }, { "formKey2", "formValue2" } });

            A.CallTo(() => request.Method).Returns("POST");
            A.CallTo(() => request.QueryString).Returns(new QueryString("?key=value"));
            //A.CallTo(() => request.ContentType).Returns("application/json");

            A.CallTo(() => request.ContentType).Returns("application/x-www-form-urlencoded");
            A.CallTo(() => request.Host).Returns(new HostString("localhost"));
            A.CallTo(() => request.Scheme).Returns("https");
            A.CallTo(() => request.Path).Returns(new PathString("/test"));
            A.CallTo(() => request.Headers).Returns(headers);
            A.CallTo(() => request.Cookies).Returns(cookies);
            A.CallTo(() => request.Form).Returns(formCollection);
            A.CallTo(() => request.HasFormContentType).Returns(true);
            A.CallTo(() => context.Request).Returns(request);
            A.CallTo(() => context.Response).Returns(response);
            A.CallTo(() => context.User).Returns(user);

            return context;
        }
    }
}
