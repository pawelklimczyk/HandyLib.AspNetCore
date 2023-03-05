using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Gmtl.HandyLib.AspNetCore
{
    public class HLJsonResult : ObjectResult
    {
        private static MediaTypeCollection _contentTypes = new MediaTypeCollection { System.Net.Mime.MediaTypeNames.Application.Json };

        public HLJsonResult(object value) : base(value)
        {
            StatusCode = 200;
            ContentTypes = _contentTypes;
        }
    }
}
