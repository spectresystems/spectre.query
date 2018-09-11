using Spectre.Query.Internal.Expressions;

namespace Spectre.Query.Internal
{
    internal interface IQueryMapping
    {
        string Alias { get; set; }

        QueryExpression CreateExpression();
    }
}
