using System;
using System.Diagnostics;

namespace Spectre.Query.Tests
{
    public static class ShouldlyExtensions
    {
        [DebuggerStepThrough]
        public static void And<T>(this T obj, Action<T> action)
        {
            action(obj);
        }

        [DebuggerStepThrough]
        public static void As<T>(this T obj, Action<T> action)
        {
            action(obj);
        }
    }
}
