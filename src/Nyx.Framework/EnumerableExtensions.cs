using System;
using System.Collections;
using System.Collections.Generic;

namespace Nyx
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<int, T> action)
        {
            var i = 0;
            foreach (var item in enumerable)
            {
                action(i++, item);
            }
        }
    }
}