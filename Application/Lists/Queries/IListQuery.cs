using Application.Interfaces.Queries;
using Application.Lists.Models;
using Domain.Entities.Lists;
using System;
using System.Threading.Tasks;

namespace Application.Lists.Queries
{
    public interface IListQuery : IResourceQuery<ListDto>
    {
        Task<List>ExecuteGetResourceByIdAsync(Guid id);
    }
}
