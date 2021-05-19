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
                var errorsForAggregation = item.Value.Errors
                    .Select(e => e.ErrorMessage)
                    .Where(s => !string.IsNullOrWhiteSpace(s));

                if (errorsForAggregation.Count() > 0)
                {
                    string errorsForKey = errorsForAggregation.Aggregate((current, next) => current + ". " + next);
                    errors.Add(item.Key, errorsForKey);
                }
            }

            return errors;
        }
    }
}
