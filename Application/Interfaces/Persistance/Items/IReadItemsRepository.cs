using Application.Items.Models;
using Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Persistance.Items
{
    public interface IReadItemsRepository : IGetRepository<ItemDto>, IGetReposityWithTracking<Item>
    {
       
    }
}
