using Application.Common.Helper;
using Application.Interfaces.Queries;
using Application.Users.Models;
using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries.Interfaces
{
    public interface IUserQuery : IResourceWithTrackingQuery<User>
    {
        Task<IEnumerable<UserDto>> ExecuteGetResourcesById(IEnumerable<Guid> ids);
        IEnumerable<UserDto> ExecuteGetResources();
        Task<IEnumerable<UserDto>> ExecuteGetResourcesAsync(QueryParameters parameters);
        Task<IEnumerable<UserDto>> ExecuteGetResourcesAsync();
        UserDto ExecuteGetResourceById(Guid id);
        Task<UserDto> ExecuteGetResourceByIdAsync(Guid id);
    }
}
