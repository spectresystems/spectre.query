using System;
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
    }
}
