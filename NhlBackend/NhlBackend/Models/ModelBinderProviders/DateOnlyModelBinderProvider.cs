using Microsoft.AspNetCore.Mvc.ModelBinding;
using NhlBackend.Models.ModelBinders;

namespace NhlBackend.Models.ModelBinderProviders;

public class DateOnlyModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        return context.Metadata.ModelType == typeof(DateOnly) ? new DateOnlyModelBinder() : null;
    }
}