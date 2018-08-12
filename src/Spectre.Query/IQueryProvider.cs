using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Spectre.Query
{
    public interface IQueryProvider<TContext>
        where TContext : DbContext
    {
        IQueryable<TEntity> Query<TEntity>(TContext context, string query) where TEntity : class;
    }
}
