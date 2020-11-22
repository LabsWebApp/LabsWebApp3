using LabsWebApp5.Models.Domain.Repositories.Abstract;

namespace LabsWebApp5.Models.Domain
{
    public class DataManager
    {
        public ITextFieldsRepository TextFields { get; set; }
        public IEventItemsRepository EventItems { get; set; }

        public DataManager(ITextFieldsRepository textFieldsRepository, IEventItemsRepository eventItemsRepository)
        {
            TextFields = textFieldsRepository;
            EventItems = eventItemsRepository;
        }
    }
}
