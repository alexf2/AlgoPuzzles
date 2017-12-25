using System.Collections;
using System.Collections.Generic;

namespace Algorithms
{    
    public sealed class SemicolonSeparatedArrayConvertor<T> : CustomConvertorBase<T>
    {
        protected override ICollection ToCollection(IList<T> data)
        {
            T[] arr = new T[data.Count];
            if (data.Count > 0)
                data.CopyTo(arr, 0);
            return arr;
        }
    }
}
