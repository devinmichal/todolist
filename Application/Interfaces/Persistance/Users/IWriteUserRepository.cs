using Application.Interfaces.Persistance.Shared;
using Application.Users.Models;
using Domain.Entities.Users;

namespace Application.Interfaces.Persistance.Users
{
   public interface IWriteUserRepository : IAddRepository<UserManipulationDto,UserDto>, IUpdateRepository<User,UserManipulationDto>, IDeleteRepository, ISaveRepository
    {

        UserDto AddResource(UserManipulationDto resource);
    }
}
