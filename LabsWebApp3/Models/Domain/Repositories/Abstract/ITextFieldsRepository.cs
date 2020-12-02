using LabsWebApp3.Models.Domain.Entities;

namespace LabsWebApp3.Models.Domain.Repositories.Abstract
{
    public interface ITextFieldsRepository : IPageItemsRepository<TextField> {
        TextField GetItemByCodeWord(string codeWord);
    }
}
