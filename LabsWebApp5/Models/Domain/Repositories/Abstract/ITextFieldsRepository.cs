using System;
using System.Linq;
using LabsWebApp5.Models.Domain.Entities;

namespace LabsWebApp5.Models.Domain.Repositories.Abstract
{
    public interface ITextFieldsRepository : IPageItemsRepository<TextField> {
        TextField GetItemByCodeWord(string codeWord);
    }
}
