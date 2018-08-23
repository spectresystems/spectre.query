using System.Collections.Generic;
using Spectre.Query.Tests.Data;

namespace Spectre.Query.Tests.Infrastructure
{
    public sealed class TestQueryRunnerResult
    {
        public string Query { get; set; }
        public List<Document> Result { get; set; }
    }
}
