using System;
using System.Threading.Tasks;
namespace Application.Interfaces.Commands
{
    public interface IResourceUpdateCommand<T,R>
    {
       void ExecuteUpdateResource(T resource, R dto);
    }
}
