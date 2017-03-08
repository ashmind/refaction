using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace Refactored.Models.Repositories
{
    public abstract class RepositoryBase<TModel> : IRepository<TModel>
        where TModel: class
    {
        private static readonly string TableName = typeof(TModel).Name;
        protected Func<IDbConnection> ConnectionFactory { get; }

        protected RepositoryBase(Func<IDbConnection> connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public IReadOnlyCollection<TModel> GetAll()
        {
            using (var connection = ConnectionFactory())
            {
                return connection.Query<TModel>($"select * from {TableName}").ToArray();
            }
        }

        // This can be made more interesting by accepting IFormattableString,
        // but for the given task that would be an overkill.
        public IReadOnlyCollection<TModel> GetAllBy<T>(string columnName, T value)
        {
            Argument.NotNullOrEmpty(nameof(columnName), columnName);
            using (var connection = ConnectionFactory())
            {
                return connection.Query<TModel>($"select * from {TableName} where {columnName} = @value", new { value }).ToArray();
            }
        }

        public void SaveNew(TModel model)
        {
            Argument.NotNull(nameof(model), model);
            using (var connection = ConnectionFactory())
            {
                connection.Execute(InsertSql, model);
            }
        }

        public void Save(TModel model)
        {
            Argument.NotNull(nameof(model), model);
            using (var connection = ConnectionFactory())
            {
                var rowCount = connection.Execute(UpdateSql, model);
                if (rowCount == 0)
                    connection.Execute(InsertSql, model);
            }
        }

        // Obviously in the current case those two properties can be generated as well (reflection),
        // however I don't feel it's worth going that path.
        protected abstract string UpdateSql { get; }
        protected abstract string InsertSql { get; }

        public void Delete(Guid id)
        {
            // In real app I would recommend soft-delete her.
            using (var connection = ConnectionFactory())
            {
                connection.Execute($"delete from {TableName} where id = @id", new { id });
            }
        }
    }
}