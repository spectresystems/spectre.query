using System;
using System.Reflection;

namespace Spectre.Query
{
    internal sealed class QueryProperty
    {
        public Type EntityType { get; set; }
        public string Alias { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
    }
}
