using System.Diagnostics;

namespace Spectre.Query.Tests
{
    public static class ShouldlyExtensions
    {
        [DebuggerStepThrough]
        public static T And<T>(this T obj)
        {
            return obj;
        }
    }
}
