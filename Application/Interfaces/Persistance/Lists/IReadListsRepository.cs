using Application.Lists.Models;
using Domain.Entities.Lists;
using System;
using System.Threading.Tasks;

namespace Application.Interfaces.Persistance.Lists
{
   public interface IReadListsRepository : IGetRepository<ListDto>
    {
        Task<List> GetResourceByIdAsync(Guid id);
    }
}
