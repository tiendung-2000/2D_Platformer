using System;

// ReSharper disable CheckNamespace

namespace SunExternals.Shared
{
    public static class EnumHelper
    {
        public static bool HaveOption<T>(this T field, T current) where T : Enum
        {
            return field.HasFlag(current);
        }
    }
}