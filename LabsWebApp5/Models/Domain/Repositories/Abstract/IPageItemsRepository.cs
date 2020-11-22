using System;
using System.Linq;

namespace LabsWebApp5.Models.Domain.Repositories.Abstract
{
    public interface IPageItemsRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Items { get; }
        TEntity GetItemById(Guid id);
        void SaveItem(TEntity entity);
        void DeleteItem(Guid id);
    }
}
