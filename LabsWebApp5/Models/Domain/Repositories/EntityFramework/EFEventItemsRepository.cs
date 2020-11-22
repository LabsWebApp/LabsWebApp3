using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LabsWebApp5.Models.Domain.Entities;
using LabsWebApp5.Models.Domain.Repositories.Abstract;
using System.Runtime.CompilerServices;

namespace LabsWebApp5.Models.Domain.Repositories.EntityFramework
{
    public class EFEventItemsRepository : IEventItemsRepository
    {
        private readonly EFAppDbContext context;
        public EFEventItemsRepository(EFAppDbContext context) => this.context = context;
        public IQueryable<EventItem> Items => context.EventItems;

        public EventItem GetItemById(Guid id) => context.EventItems.FirstOrDefault(x => x.Id == id);

        public void SaveItem(EventItem entity)
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteItem(Guid id)
        {
            context.EventItems.Remove(new EventItem() { Id = id });
            context.SaveChanges();
        }
    }
}
