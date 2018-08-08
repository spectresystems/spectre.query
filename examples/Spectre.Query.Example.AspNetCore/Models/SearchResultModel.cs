using System.Collections.Generic;

namespace Spectre.Query.AspNetCore.Example.Models
{
    public sealed class SearchResultModel<T>
    {
        public string Query { get; set; }
        public List<T> Movies { get; set; }
    }
}
