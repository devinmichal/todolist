using Application.Common.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Queries
{
   public interface IResourceQuery<T>
    {
        Task<IEnumerable<T>> ExecuteGetResourcesAsync(Guid parentId);
        Task<T> ExecuteGetResourceByIdAsync(Guid parentId, Guid id);
       
      
    }
}
