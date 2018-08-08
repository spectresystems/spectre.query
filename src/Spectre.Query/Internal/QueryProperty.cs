using System;

namespace Spectre.Query
{
    internal sealed class QueryProperty
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
    }
}
