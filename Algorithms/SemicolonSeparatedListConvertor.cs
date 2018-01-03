using System.Collections;
using System.Collections.Generic;

namespace Algorithms
{
    public sealed class SemicolonSeparatedListConvertor<T> : CustomConvertorBase<T>
    {
        protected override ICollection ToCollection(IList<T> data)
        {
            return new List<T>(data);
        }
    }
}
