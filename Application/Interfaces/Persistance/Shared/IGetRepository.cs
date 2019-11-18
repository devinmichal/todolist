using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Application.Common.Helper;

namespace Application.Interfaces.Persistance
{
    public interface IGetRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(Guid id);
        Task<T> GetResourceByIdAsync(Guid parentId, Guid id);
       
    }
}
