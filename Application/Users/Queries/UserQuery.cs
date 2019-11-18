using Application.Common.Helper;
using Application.Interfaces.Persistance.Users;
using Application.Users.Models;
using Application.Users.Queries.Interfaces;
using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Users.Queries
{
   public class UserQuery : IUserQuery
    {
        private readonly IReadUserRepository _userContext;
        public UserQuery(IReadUserRepository userContext)
        {
            _userContext = userContext;
        }
        public UserDto ExecuteGetResourceById(Guid id)
        {
            return _userContext.GetById(id);
        }

        public async Task<UserDto> ExecuteGetResourceByIdAsync(Guid id)
        {
            return await _userContext.GetByIdAsync(id);
        }

        public IEnumerable<UserDto> ExecuteGetResources()
        {
            return _userContext.GetAll();
        }

        public async Task<IEnumerable<UserDto>> ExecuteGetResourcesAsync()
        {
            return await _userContext.GetAllAsync();
        }

        public async Task<IEnumerable<UserDto>> ExecuteGetResourcesById(IEnumerable<Guid> ids)
        {
            return await _userContext.GetByIdsAsync(ids);
        }

        public async Task<User> ExecuteGetResourceByIdWithTracking(Guid id)
        {
            return await _userContext.GetResourceByIdWithTracking(id);
        }

        public async Task<IEnumerable<UserDto>> ExecuteGetResourcesAsync(QueryParameters parameters)
        {
            return await _userContext.GetAllAsync(parameters);
        }
    }
}
