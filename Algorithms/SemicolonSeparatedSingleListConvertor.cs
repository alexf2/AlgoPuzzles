using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Algorithms
{
    public sealed class SemicolonSeparatedSingleListConvertor<T> : CustomConvertorBase<T>
    {
        protected override ICollection ToCollection(IList<T> array)
        {
            throw new NotImplementedException();
        }

        public override object ConvertFrom (ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var str = (string)value;

                if (string.IsNullOrEmpty(str))
                    return null;

                ListNode<T> prevInstance = null, head = null;
                foreach (var v in str.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Select(it => it.Trim()))
                {
                    T item;
                    if (v == string.Empty)
                        continue;
                    else
                        item = (T)_typeConverter.ConvertFromInvariantString(v);

                    if (prevInstance == null)
                        head = prevInstance = new ListNode<T>() { Value = item };
                    else                    
                        prevInstance = prevInstance.Next = new ListNode<T>() { Value = item };
                }

                return head;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
