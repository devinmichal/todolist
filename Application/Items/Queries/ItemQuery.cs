using Application.Common.Helper;
using Application.Interfaces.Persistance;
using Application.Interfaces.Persistance.Items;
using Application.Interfaces.Queries;
using Application.Items.Models;
using Application.Items.Queries.Interfaces;
using Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Items.Queries
{
    public class ItemQuery : IItemQuery
    {
        private readonly IReadItemsRepository _context;
        public ItemQuery(IReadItemsRepository context)
        {
            _context = context;
        }


        public Task<IEnumerable<ItemDto>> ExecuteGetResourcesAsync(Guid listId)
        {
            return _context.GetAllAsync(listId);
        }

      
        public Task<ItemDto> ExecuteGetResourceByIdAsync(Guid listId, Guid id)
        {
            return _context.GetResourceByIdAsync(listId, id);
        }

        public async Task<Item> ExecuteGetResourceByIdWithTracking(Guid id)
        {
            return await _context.GetResourceByIdWithTracking(id);
        }
    }
}
