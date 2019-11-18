using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Persistance.Items;
using Application.Interfaces.Persistance.Users;
using Application.Items.Models;
using Domain.Entities.Items;

namespace Application.Items.Commands
{
    public class ItemsCommand : IItemsCommand
    {
        private readonly IWriteItemsRepository _context;
        public ItemsCommand(IWriteItemsRepository context)
        {
            _context = context;
        }
        public ItemDto ExecuteAddResource(ItemManipulationDto resource)
        {
            throw new NotImplementedException();
        }

        public ItemDto ExecuteAddResource(ItemManipulationDto resource, Guid id)
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

        public void ExecuteUpdateResource(Item resource, ItemToUpdateDto dto)
        {
            _context.UpdateResource(resource, dto);
        }
    }
}
