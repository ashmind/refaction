using System;
using System.Collections.Generic;

namespace Refactored.Models.Repositories
{
    public interface IRepository<TModel>
    {
        IReadOnlyCollection<TModel> GetAll();
        IReadOnlyCollection<TModel> GetAllBy<T>(string columnName, T value);
        void SaveNew(TModel product);
        void Save(TModel product);
        void Delete(Guid id);
    }
}