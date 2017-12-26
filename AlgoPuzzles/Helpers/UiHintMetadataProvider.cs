using System.Collections;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace AlgoPuzzles.Helpers
{
    public sealed class UiHintMetadataProvider : IDisplayMetadataProvider
    {
        public void CreateDisplayMetadata (DisplayMetadataProviderContext context)
        {
            var mt = context.Key.ModelType;

            if (mt.IsArray && mt.GetArrayRank() == 2)
                context.DisplayMetadata.TemplateHint = "Matrix";
            else if (typeof(ICollection).IsAssignableFrom(mt))
                context.DisplayMetadata.TemplateHint = "Collection";            
        }
    }
}
