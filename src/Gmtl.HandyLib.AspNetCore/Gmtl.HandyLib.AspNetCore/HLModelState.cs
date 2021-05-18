using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Gmtl.HandyLib.AspNetCore
{
    public static class HLModelState
    {
        public static Dictionary<string, string> GetErrors(this ModelStateDictionary modelState)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            if (modelState.IsValid)
            {
                return errors;
            }

            foreach (var item in modelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    if (!errors.ContainsKey(item.Key))
                    {
                        //errors.Add(item.Key, error.ErrorMessage);
                    }
                }

                string errorsForKey = item.Value.Errors
                    .Select(e => e.ErrorMessage)
                    .Aggregate("", //handle empty list case.
                        (current, next) => current + ". " + next);

                errors.Add(item.Key, errorsForKey);
            }

            return errors;
        }
    }
}
