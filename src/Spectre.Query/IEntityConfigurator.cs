using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Spectre.Query
{
    public interface IRootEntityConfigurator<TEntity> : IEntityConfigurator<TEntity>
    {
        void Map<TDerivedEntity>(Action<IEntityConfigurator<TDerivedEntity>> options) where TDerivedEntity : TEntity;
    }

    public interface IEntityConfigurator<TEntity>
    {
        void Map<TResult>(string name, Expression<Func<TEntity, TResult>> expression);
        void Map<TItem, TResult>(string name, Expression<Func<TEntity, ICollection<TItem>>> expression, Expression<Func<TItem, TResult>> getter);
    }
}
