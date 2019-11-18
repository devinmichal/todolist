using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Helper;
using Application.Interfaces.Persistance;
using Application.Interfaces.Persistance.Lists;
using Application.Interfaces.Queries;
using Application.Lists.Models;
using Domain.Entities.Lists;

namespace Application.Lists.Queries
{
    public class ListQuery :IListQuery
    {
        private readonly IReadListsRepository _listContext;
        public ListQuery(IReadListsRepository listContext)
        {
            _listContext = listContext;
        }

        public async Task<ListDto> ExecuteGetResourceByIdAsync(Guid parentId, Guid id)
        {
            return  await _listContext.GetResourceByIdAsync(parentId, id);
        }

        public async Task<List> ExecuteGetResourceByIdAsync(Guid id)
        {
            return  await _listContext.GetResourceByIdAsync(id);
        }

        public async Task<IEnumerable<ListDto>> ExecuteGetResourcesAsync(Guid parentId)
        {
            return await _listContext.GetAllAsync(parentId);
        }
    }
}
