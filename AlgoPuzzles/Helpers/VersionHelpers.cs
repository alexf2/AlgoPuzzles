using System.Linq;
using System.Reflection;

namespace WebApiHelpers
{
    public class VersionHelpers
    {
        public static string GetProductVersion(Assembly ass = null)
        {            
            var attribute = (ass ?? typeof(VersionHelpers).GetTypeInfo().Assembly)
              .GetCustomAttributes<AssemblyFileVersionAttribute>()
              .Single();

            return attribute.Version;            
        }
    }
}
