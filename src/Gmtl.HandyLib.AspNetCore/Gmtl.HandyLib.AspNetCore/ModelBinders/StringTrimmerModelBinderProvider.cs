using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Gmtl.HandyLib.AspNetCore.ModelBinders;

/// <summary>
/// Usage
//services.AddControllers(opt =>
///{
///    //registers CustomModelBinderProvider in the first place so that it is asked before other model binder providers.
///    opt.ModelBinderProviders.Insert(0, new StringTrimmerModelBinderProvider());
///});
/// </summary>
public class StringTrimmerModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));


        if (context.Metadata.ModelType == typeof(string) && context.BindingInfo.BindingSource != BindingSource.Body)
            return new StringTrimmerBinder();

        return null;
    }

    public class StringTrimmerBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext is null)
                throw new ArgumentNullException(nameof(bindingContext));

            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            bindingContext.Result = ModelBindingResult.Success(valueProviderResult.FirstValue.Trim());
            return Task.CompletedTask;
        }
    }
}

