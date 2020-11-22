using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LabsWebApp5.Models.Domain.Entities;
using LabsWebApp5.Models.Domain.Repositories.Abstract;

namespace LabsWebApp5.Models.Domain.Repositories.EntityFramework
{
    public class EFTextFieldsRepository : ITextFieldsRepository
    {
        private readonly EFAppDbContext context;
        public EFTextFieldsRepository(EFAppDbContext context) => this.context = context;

        public IQueryable<TextField> Items => context.TextFields;

        public TextField GetItemById(Guid id) => context.TextFields.FirstOrDefault(x => x.Id == id);

        public TextField GetItemByCodeWord(string codeWord) => context.TextFields.FirstOrDefault(x => x.CodeWord == codeWord);

        public void SaveItem(TextField entity)
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteItem(Guid id)
        {
            context.TextFields.Remove(new TextField() { Id = id });
            context.SaveChanges();
        }
    }
}
