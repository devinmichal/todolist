using System;
using System.Threading.Tasks;

namespace Application.Interfaces.Queries
{
   public interface IResourceWithTrackingQuery<T>
    {
        Task<T> ExecuteGetResourceByIdWithTracking(Guid id);
    }
}
