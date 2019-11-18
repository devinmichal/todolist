using Persistance.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Application.Users.Models;
using Application.Interfaces.Persistance.Users;
using Domain.Entities.Users;
using Application.Common.Helper;

namespace Persistance.Services.Users
{
   public class UsersRepository : IUserRepository
    {
        private readonly ToDoListContext _context;
        public UsersRepository(ToDoListContext context)
        {
            _context = context;
        }

        public UserDto AddResource(UserManipulationDto resource)
        {
            var userToCreate = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = resource.FirstName,
                LastName = resource.LastName
            };
            _context.Add(userToCreate);

            return new UserDto()
            {
                Id = userToCreate.Id,
                FullName = userToCreate.FirstName + " " + userToCreate.LastName
            };
        }

        public UserDto AddResource(UserManipulationDto resource, Guid id)
        {
            var userToCreate = new User()
            {
                Id = id,
                FirstName = resource.FirstName,
                LastName = resource.LastName
            };
            _context.Add(userToCreate);

            return new UserDto()
            {
                Id = userToCreate.Id,
                FullName = userToCreate.FirstName + " " + userToCreate.LastName
            };
        }

        public IEnumerable<UserDto> GetAll()
        {
            return _context.Users
                .Select(u => new UserDto()
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName

                })
                .AsNoTracking()
                .ToList();
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _context.Users
                  .Select(u => new UserDto()
                  {
                      Id = u.Id,
                      FullName = u.FirstName + " " + u.LastName

                  })
               .AsNoTracking()
               .ToListAsync();
        }

        public UserDto GetById(Guid id)
        {
            return _context.Users
                  .Select(u => new UserDto()
                  {
                      Id = u.Id,
                      FullName = u.FirstName + " " + u.LastName

                  })
               .AsNoTracking()
               .SingleOrDefault(u => u.Id == id);
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            return await _context.Users
                  .Select(u => new UserDto()
                  {
                      Id = u.Id,
                      FullName = u.FirstName + " " + u.LastName

                  })
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public IEnumerable<UserDto> GetByIds(IEnumerable<Guid> id)
        {
            return _context.Users
                 .Where(u => id.Contains(u.Id))
                  .Select(u => new UserDto()
                  {
                      Id = u.Id,
                      FullName = u.FirstName + " " + u.LastName

                  })
                 .AsNoTracking()
                 .ToList();
        }

        public async Task<IEnumerable<UserDto>> GetByIdsAsync(IEnumerable<Guid> id)
        {
            return await _context.Users
                 .Where(u => id.Contains(u.Id))
                  .Select(u => new UserDto()
                  {
                      Id = u.Id,
                      FullName = u.FirstName + " " + u.LastName

                  })
                 .AsNoTracking()
                 .ToListAsync();
        }

        public async Task<User> GetResourceByIdWithTracking(Guid id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return ( await _context.SaveChangesAsync() >= 0);
        }

        public void UpdateResource(User resource, UserManipulationDto dto)
        {
            resource.FirstName = dto.FirstName ?? resource.FirstName;
            resource.LastName = dto.LastName ?? resource.LastName;
        }

        public async Task<int> DeleteResource(Guid id)
        {
            return  await _context.Database.ExecuteSqlCommandAsync($"DELETE FROM Users WHERE Id = {id}");

        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(QueryParameters parameters)
        {
            var users = _context.Users.AsNoTracking();




            //Searching
            if (parameters.SearchQuery != null)
            {
                var search = parameters.SearchQuery.Trim().ToLower();
              users = users.Where(u => 
              u.FirstName.ToLower().Contains(search) 
              || u.LastName.ToLower().Contains(search)
              );
            }

          
            return await users.Skip(parameters.PageNumber - 1)
                .Take(parameters.PageSize)
                .Select(u => new UserDto()
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName

                })
                .ToListAsync();
        }
    }
}
