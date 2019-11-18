using Domain.Entities.Items;
using Persistance.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Application.Items.Models;
using Domain.Entities.Extensions;
using Application.Common.Helper;
using Application.Interfaces.Persistance.Items;


namespace Persistance.Services.Items
{

    // To Do : 
    // Use Select method for more performat queries transforming entities into  the outer facing contracts/ Dto
    public class ItemsRepository : IItemsRepository
    {
        private readonly ToDoListContext _context;

        public ItemsRepository(ToDoListContext context)
        {
            _context = context;
        }

        public ItemDto AddResource(ItemManipulationDto resource)
        {
            throw new NotImplementedException();
        }

        public ItemDto AddResource(ItemManipulationDto resource, Guid id)
        {
            var createdItem = new Item()
            {
                Id = Guid.NewGuid(),
                Name = resource.Name,
                ListId = id,
                isCompleted = false,
                Completed = DateTimeOffset.MinValue
           
            };

            _context.Add(createdItem);

            return new ItemDto() { Id = createdItem.Id, Name = createdItem.Name, Status = createdItem.GetStatus() };
        }

        public async Task<int> DeleteResource(Guid id)
        {
           return  await _context.Database.ExecuteSqlCommandAsync($"DELETE FROM Items WHERE Id = {id}");
        }

        public IEnumerable<ItemDto> GetAll()
        {
            return _context.Items
                .Select(i => new ItemDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Status = i.GetStatus()

                })
                .AsNoTracking()
                .ToList();
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            return await _context.Items
                .Select(i => new ItemDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Status = i.GetStatus()

                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync(Guid listId)
        {
            return await _context.Items
                .Where(i => i.ListId == listId)
                .Select(i => new ItemDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Status = i.GetStatus()

                })
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<IEnumerable<ItemDto>> GetAllAsync(QueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ItemDto GetById(Guid id)
        {
            return _context.Items
                .Select(i => new ItemDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Status = i.GetStatus()

                })
                .AsNoTracking()
                .FirstOrDefault(i => i.Id == id);
        }

        public async Task<ItemDto> GetByIdAsync(Guid id)
        {
            return await _context.Items
                .Select(i => new ItemDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Status = i.GetStatus()

                })
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public  async Task<ItemDto> GetResourceByIdAsync(Guid listId, Guid id)
        {
            return await _context.Items
               .Where(i => i.ListId == listId)
               .Select(i => new ItemDto()
               {
                   Id = i.Id,
                   Name = i.Name,
                   Status = i.GetStatus()

               })
               .AsNoTracking()
               .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Item> GetResourceByIdWithTracking(Guid id)
        {
            return await _context.Items.SingleOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void UpdateResource(Item resource, ItemToUpdateDto dto)
        {
            resource.Name = string.IsNullOrWhiteSpace(dto.Name) ? resource.Name : dto.Name;

            resource.isCompleted = dto.Status.Trim().ToLower().Contains("completed") ? true : resource.isCompleted;

            resource.isCompleted = dto.Status.Trim().ToLower().Contains("incomplete") ? false : resource.isCompleted;

            resource.Completed = resource.isCompleted && (resource.Completed == DateTimeOffset.MinValue) ? DateTimeOffset.UtcNow : resource.Completed;

        }
    }
}
