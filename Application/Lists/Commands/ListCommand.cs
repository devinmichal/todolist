using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Persistance.Lists;
using Application.Lists.Models;
using Domain.Entities.Lists;

namespace Application.Lists.Commands
{
    public class ListCommand : IListCommand
    {
        private readonly IWriteListsRepository _context;
        public ListCommand(IWriteListsRepository context)
        {
            _context = context;
        }
       

        public ListDto ExecuteAddResource(ListManipulationDto resource, Guid id)
        {
           return _context.AddResource(resource, id);
        }

        public async Task<int> ExecuteDeleteResource(Guid id)
        {
            return  await _context.DeleteResource(id);
        }

        public async Task<bool> ExecuteSaveAsync()
        {
            return await _context.SaveAsync();
        }

        public void ExecuteUpdateResource(List resource, ListManipulationDto dto)
        {
            _context.UpdateResource(resource, dto);
        }
    }
}
