using System.Linq;

namespace Spectre.Query.AspNetCore
{
    public interface IQueryProviderSession<TContext>
    {
        IQueryable<TEntity> Query<TEntity>(string query) where TEntity : class;
    }
}
