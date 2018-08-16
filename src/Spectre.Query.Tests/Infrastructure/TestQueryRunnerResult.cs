using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Query.Tests.Data;

namespace Spectre.Query.Tests.Infrastructure
{
    public sealed class TestQueryRunnerResult
    {
        public string Query { get; set; }
        public List<Invoice> Result { get; set; }
    }
}
