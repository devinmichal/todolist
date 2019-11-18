using Application.Interfaces.Commands;
using Application.Interfaces.Persistance.Users;
using Application.Users.Models;
using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class UserCommand : IUserCommand
    {
        private readonly IWriteUserRepository _context;
        public UserCommand(IWriteUserRepository context)
        {
            _context = context;
        }
        public UserDto ExecuteAddResource(UserManipulationDto resource)
        {
            return _context.AddResource(resource);
        }

        public UserDto ExecuteAddResource(UserManipulationDto resource, Guid id)
        {
           return _context.AddResource(resource, id);
        }

        public Task<int> ExecuteDeleteResource(Guid id)
        {
           return _context.DeleteResource(id);
        }

        public Task<bool> ExecuteSaveAsync()
        {
            return _context.SaveAsync();
        }

        public void ExecuteUpdateResource(User resource, UserManipulationDto dto)
        {
            _context.UpdateResource(resource, dto);
        }
    }
}
