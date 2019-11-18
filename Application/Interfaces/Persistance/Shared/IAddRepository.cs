using System.Threading.Tasks;
using System;

namespace Application.Interfaces.Persistance.Shared
{
   public interface IAddRepository<in T, out R>
    {
        R AddResource(T resource, Guid id);
    }
}
