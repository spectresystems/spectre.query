using System;
using System.Linq.Expressions;

namespace Spectre.Query
{
    public interface IEntityConfigurator<TEntity>
    {
        void Map<TResult>(string name, Expression<Func<TEntity, TResult>> expression);
    }
}
