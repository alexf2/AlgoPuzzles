using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            else if (mt.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICollection<>)))
            {
                var itemType = mt.IsArray ? mt.GetElementType() : (mt.IsGenericType ? mt.GetGenericArguments()[0]:null);                                
                context.DisplayMetadata.TemplateHint =
                    itemType != null && itemType.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICollection<>)) ? "NestedCollection" : "Collection";
            }
        }
    }
}
