using System;
using System.Collections.Generic;
using System.Text;
using Clinic_DataAccess.Entities;

namespace Clinic_DataAccess.Repositories.Interfaces
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        Task<Branch?> GetByNameAsync(string name);
    }
}
