using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AlgoPuzzles.Helpers
{
    public static class PropertyInfoExtensions
    {
        public static string GetDisplay (this PropertyInfo prop)
        {
            var attrDisplay = (DisplayAttribute)Attribute.GetCustomAttribute(prop, typeof(DisplayAttribute));
            return attrDisplay?.Name ?? prop.Name;
        }
    }
}
