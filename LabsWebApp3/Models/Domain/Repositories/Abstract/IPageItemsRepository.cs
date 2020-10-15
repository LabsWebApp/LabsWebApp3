using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabsWebApp3.Models.Domain.Repositories.Abstract
{
    public interface IPageItemsRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Items { get; }
        TEntity GetItemById(Guid id);
        void SaveItem(TEntity entity);
        void DeleteItem(Guid id);
    }
}
