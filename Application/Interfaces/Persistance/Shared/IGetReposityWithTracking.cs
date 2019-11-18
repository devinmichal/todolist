using System;
using System.Threading.Tasks;

namespace Application.Interfaces.Persistance
{
    public interface IGetReposityWithTracking<T>
    {
        Task<T> GetResourceByIdWithTracking(Guid id);
    }
}
