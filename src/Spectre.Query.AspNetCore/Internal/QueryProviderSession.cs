using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Spectre.Query.AspNetCore
{
    internal sealed class QueryProviderSession<TContext> : IQueryProviderSession<TContext>
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly IQueryProvider<TContext> _provider;

        public QueryProviderSession(TContext context, IQueryProvider<TContext> provider)
        {
            _context = context;
            _provider = provider;
        }

        public IQueryable<TEntity> Query<TEntity>(string query)
            where TEntity : class
        {
            return _provider.Query<TEntity>(_context, query);
        }
    }
}
