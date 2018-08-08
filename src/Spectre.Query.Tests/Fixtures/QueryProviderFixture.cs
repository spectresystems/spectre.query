using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Spectre.Query.Tests.Data;

namespace Spectre.Query.Tests.Fixtures
{
    public sealed class QueryProviderFixture : IDisposable
    {
        private readonly DbContextOptionsBuilder<DataContext> _builder;
        private readonly SqliteConnection _connection;
        private IQueryProvider<DataContext> _provider;

        public QueryProviderFixture(Action<DataContext> seeder = null)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _builder = new DbContextOptionsBuilder<DataContext>().UseSqlite(_connection);

            using (var context = new DataContext(_builder.Options))
            {
                context.Database.EnsureCreated();

                if (seeder != null)
                {
                    // Seed the database.
                    seeder(context);
                    context.SaveChanges();
                }
            }
        }

        public void Dispose()
        {
            _connection.Close();
        }

        public void Initialize(Action<IQueryConfigurator<DataContext>> configurator)
        {
            using (var context = new DataContext(_builder.Options))
            {
                _provider = QueryProviderBuilder.Build(context, configurator);
            }
        }

        public string ToSql<TEntity>(string query)
            where TEntity : class
        {
            if (_provider == null)
            {
                throw new InvalidOperationException("Query provider fixture has not been initialized.");
            }

            using (var context = new DataContext(_builder.Options))
            {
                return _provider.ToSql<TEntity>(query);
            }
        }

        public List<TEntity> Query<TEntity>(string query)
            where TEntity : class
        {
            if (_provider == null)
            {
                throw new InvalidOperationException("Query provider fixture has not been initialized.");
            }

            using (var context = new DataContext(_builder.Options))
            {
                return _provider.Query<TEntity>(context, query).ToList();
            }
        }
    }
}
