using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Spectre.Query
{
    public interface IQueryProvider<TContext>
        where TContext : DbContext
    {
        Expression<Func<TEntity, bool>> Compile<TEntity>(string query) where TEntity : class;
        IQueryable<TEntity> Query<TEntity>(TContext context, string query) where TEntity : class;
    }
}
