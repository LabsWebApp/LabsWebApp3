using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabsWebApp3.Models.Domain.Repositories.Abstract
{
    public interface IFunctionsRepository
    {
        public Task AddBlockAsync(string id, DateTime upto);
        public Task<DateTime> GetBlockAsync(string id);
    }
}
