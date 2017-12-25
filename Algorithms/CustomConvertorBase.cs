using System;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

namespace Algorithms
{
    public abstract class CustomConvertorBase<T> : TypeConverter
    {
        protected const string Separator = "; ";

        protected readonly TypeConverter _typeConverter;

        protected Type ArgType { get => typeof(T); }

        public CustomConvertorBase()
        {
            var t = ArgType;
            _typeConverter = TypeDescriptor.GetConverter(t);
            if (_typeConverter == null)
                throw new InvalidOperationException($"No type converter exists for type {t.FullName}");
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) ? true : base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) ? true : base.CanConvertTo(context, destinationType);
        }

        protected abstract ICollection ToCollection(IList<T> array);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string || value == null)
            {
                var str = (string)value;

                if (string.IsNullOrEmpty(str))
                    return ToCollection(new T[] { });

                var arr = ((string)value).Split(';', StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length == 0)
                    return ToCollection(new T[] { });

                var tmp = new List<T>(arr.Length);
                for (int i = 0; i < arr.Length; ++i)
                {
                    var val = arr[i].Trim();
                    if (!string.IsNullOrEmpty(val))
                        tmp.Add((T)_typeConverter.ConvertFromInvariantString(val));
                }

                return ToCollection(tmp);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value == null)
                    return string.Empty;

                if (value is ICollection)
                {
                    var b = new StringBuilder();
                    foreach (var item in (value as ICollection))
                    {
                        if (b.Length > 0)
                            b.Append(Separator);
                        b.Append(_typeConverter.ConvertToInvariantString(item));
                    }

                    return b.ToString();
                }
                else
                    return _typeConverter.ConvertToInvariantString(value);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
