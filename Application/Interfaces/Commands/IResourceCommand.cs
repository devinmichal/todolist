using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Commands
{
    public interface IResourceCommand<in T, out R>
    {
      
        R ExecuteAddResource(T resource, Guid id);
        Task<int> ExecuteDeleteResource(Guid id);
        Task<bool> ExecuteSaveAsync();
    }
}
