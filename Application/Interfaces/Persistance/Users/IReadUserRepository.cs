using Application.Common.Helper;
using Application.Users.Models;
using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Persistance.Users
{
   public interface IReadUserRepository : IGetReposityWithTracking<User>
    {
        IEnumerable<UserDto> GetAll();
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<IEnumerable<UserDto>> GetAllAsync(QueryParameters parameters);

        UserDto GetById(Guid id);
        Task<UserDto> GetByIdAsync(Guid id);
        Task<IEnumerable<UserDto>> GetByIdsAsync(IEnumerable<Guid> id);
    }
}
