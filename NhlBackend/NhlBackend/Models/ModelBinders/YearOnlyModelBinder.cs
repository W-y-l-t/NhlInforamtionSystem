using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NhlBackend.Models.Types;

namespace NhlBackend.Models.ModelBinders;

public class YearOnlyModelBinder : IModelBinder
{
    private const string YearFormat = "yyyy";

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        var str = valueProviderResult.FirstValue;
        
        if (str != null && YearOnly.TryParseExact(
                str, YearFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var yearOnly))
        {
            bindingContext.Result = ModelBindingResult.Success(yearOnly);
        }
        else
        {
            bindingContext.ModelState.TryAddModelError(
                bindingContext.ModelName,
                $"Year format must be '{YearFormat}'."
            );
        }
        
        return Task.CompletedTask;
    }
}