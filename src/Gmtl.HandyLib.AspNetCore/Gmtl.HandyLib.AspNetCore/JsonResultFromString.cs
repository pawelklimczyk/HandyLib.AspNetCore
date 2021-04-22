using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Gmtl.HandyLib.AspNetCore
{
    public class JsonResultFromString : ObjectResult
    {
        private static MediaTypeCollection contentTypes = new MediaTypeCollection { System.Net.Mime.MediaTypeNames.Application.Json };

        public JsonResultFromString(object value) : base(value)
        {
            StatusCode = 200;
            ContentTypes = contentTypes;
        }
    }
}
