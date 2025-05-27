using Microsoft.AspNetCore.Mvc.ModelBinding;
using NhlBackend.Models.ModelBinders;
using NhlBackend.Models.Types;

namespace NhlBackend.Models.ModelBinderProviders;

public class YearOnlyModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        return context.Metadata.ModelType == typeof(YearOnly) ? new YearOnlyModelBinder() : null;
    }
}