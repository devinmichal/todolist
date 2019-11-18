using Application.Interfaces.Commands;
using Application.Users.Models;
using Domain.Entities.Users;

namespace Application.Users.Commands
{
    public interface IUserCommand : IResourceCommand<UserManipulationDto,UserDto>, IResourceUpdateCommand<User,UserManipulationDto>
    {
        UserDto ExecuteAddResource(UserManipulationDto resource);
    }
}
