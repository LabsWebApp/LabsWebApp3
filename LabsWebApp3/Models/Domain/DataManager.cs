using System.Collections.Generic;
using LabsWebApp3.Models.Domain.Repositories.Abstract;

namespace LabsWebApp3.Models.Domain
{
    public class DataManager
    {
        public ITextFieldsRepository TextFields { get; set; }
        public IEventItemsRepository EventItems { get; set; }

        public IFunctionsRepository Functions { get; set; }

        public DataManager(
            ITextFieldsRepository textFieldsRepository, 
            IEventItemsRepository eventItemsRepository,
            IFunctionsRepository functions)
        {
            TextFields = textFieldsRepository;
            EventItems = eventItemsRepository;
            Functions = functions;
        }
    }
}
