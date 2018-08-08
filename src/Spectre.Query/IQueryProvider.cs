using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Spectre.Query
{
    public interface IQueryProvider<TContext>
        where TContext : DbContext
    {
        string ToSql<TEntity>(string query) where TEntity : class;
        IQueryable<TEntity> Query<TEntity>(TContext context, string query) where TEntity : class;
    }
}
