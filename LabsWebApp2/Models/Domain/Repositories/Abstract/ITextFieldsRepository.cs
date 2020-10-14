using LabsWebApp2.Models.Domain.Entities;

namespace LabsWebApp2.Models.Domain.Repositories.Abstracts
{
    public interface ITextFieldsRepository : IPageItemsRepository<TextField> {
        TextField GetItemByCodeWord(string codeWord);
    }
}
