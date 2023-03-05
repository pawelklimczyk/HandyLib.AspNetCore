using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Gmtl.HandyLib.AspNetCore.Api.Models
{
    public class HLApiResponse
    {
        public string Message { get; set; }

        public Dictionary<string, string[]>? Errors { get; set; }
        public string TracerIdentifier { get; private set; }

        public static HLApiResponse ErrorApiResponse(ActionContext actionContext)
        {
            var message = new HLApiResponse();
            message.ConstructErrorMessages(actionContext);
            message.TracerIdentifier = actionContext.HttpContext.TraceIdentifier;

            return message;
        }

        private void ConstructErrorMessages(ActionContext context)
        {
            if (context.ModelState.IsValid) return;

            Errors = new Dictionary<string, string[]>();

            foreach (var keyModelStatePair in context.ModelState)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;

                if (errors != null && errors.Count > 0)
                {
                    var errorMessages = new string[errors.Count];
                    for (var i = 0; i < errors.Count; i++)
                    {
                        errorMessages[i] = GetErrorMessage(errors[i]);
                    }
                    Errors.Add(key, errorMessages);
                }
            }
        }

        static string GetErrorMessage(ModelError error)
        {
            return string.IsNullOrEmpty(error.ErrorMessage) ?
                "The input was not valid." :
                error.ErrorMessage;
        }
    }

    public class HLApiResponse<T> : HLApiResponse
    {
        public T Data { get; set; }
    }
}
