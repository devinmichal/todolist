using Domain.Entities.Lists;
using Persistance.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Application.Interfaces.Persistance;
using Application.Lists.Models;
using Application.Items.Models;
using Domain.Entities.Extensions;
using Application.Common.Helper;
using Application.Interfaces.Persistance.Lists;

namespace Persistance.Services.Lists
{


    // To Do : 
    // Use Select method for more performat queries transforming entities into  the outer facing contracts/ Dto
    public class ListRepository : IListRepository
    {
        private readonly ToDoListContext _context;
        public ListRepository(ToDoListContext context)
        {
            _context = context;
        }

        
        public ListDto AddResource(ListManipulationDto resource, Guid id)
        {
            var createdList = new List()
            {
                Id = Guid.NewGuid(),
                Name = resource.Name,
                UserId = id,
                Created = DateTimeOffset.UtcNow
            };
            
            _context.Add(createdList);

            return new ListDto()
            {
                Id = createdList.Id,
                Name = createdList.Name,
                AuthorId = createdList.UserId,
                Author = _context.Users.Where(u => u.Id == createdList.UserId).Select(name => name.FirstName + " " + name.LastName).FirstOrDefault(),
                ItemsLeftToComplete = 0,
                Items = new List<ItemDto>()
            };
        }

        public async Task<int> DeleteResource(Guid id)
        {
            return await _context.Database.ExecuteSqlCommandAsync($"DELETE FROM List WHERE Id == {id}"); 
        }

  
        public async Task<IEnumerable<ListDto>> GetAllAsync(Guid id)
        {
            return await _context.Lists
                .Where(l => l.UserId == id)
                 .Select(l => new ListDto()
                 {
                     Id = l.Id,
                     Name = l.Name,
                     AuthorId = id,
                     Author = l.User.FirstName + " " + l.User.LastName,
                     ItemsLeftToComplete = l.Items.Where(i => i.isCompleted == true).Count(),
                     Items = l.Items.Select(i => new ItemDto()
                     {
                         Id = i.Id,
                         Name = i.Name,
                         Status = i.GetStatus()

                     })
                 })
                 .AsNoTracking()
                 .ToListAsync();
        }

        public async Task<ListDto> GetResourceByIdAsync(Guid parentId, Guid id)
        {
            return await _context.Lists
             .Where(l => l.UserId == parentId)
             .Select(l => new ListDto()
             {
                 Id = l.Id,
                 Name = l.Name,
                 AuthorId = parentId,
                 Author = l.User.FirstName + " " + l.User.LastName,
                 ItemsLeftToComplete = l.Items.Where(i => i.isCompleted == true).Count(),
                 Items = l.Items.Select(i => new ItemDto()
                 {
                     Id = i.Id,
                     Name = i.Name,
                     Status = i.GetStatus()

                 })
             })
             .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<List> GetResourceByIdAsync(Guid id)
        {
            return await _context.Lists
                .SingleOrDefaultAsync(l => l.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void UpdateResource(List resource, ListManipulationDto dto)
        {
            resource.Name = string.IsNullOrWhiteSpace(dto.Name) ? resource.Name : dto.Name;
        }
    }
}
