using System;
using Microsoft.EntityFrameworkCore;

namespace Spectre.Query
{
    public interface IQueryConfigurator<TContext>
        where TContext : DbContext
    {
        void Configure<TEntity>(Action<IEntityConfigurator<TEntity>> options);
    }
}
