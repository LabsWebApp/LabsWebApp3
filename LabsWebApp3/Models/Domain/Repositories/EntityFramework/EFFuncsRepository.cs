using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabsWebApp3.Models.Domain.Repositories.Abstract;

namespace LabsWebApp3.Models.Domain.Repositories.EntityFramework
{
    public class EFFunctionsRepository : IFunctionsRepository

    {
        private readonly EFAppDbContext context;
        public EFFunctionsRepository(EFAppDbContext context) => 
            this.context = context;

        public async Task AddBlockAsync(string id, DateTime upto)
        {
            await context.AddBlockAsync(id, upto);
        }

        public async Task<DateTime> GetBlockAsync(string id) =>
            await context.GetBlockAsync(id);
    }
}
