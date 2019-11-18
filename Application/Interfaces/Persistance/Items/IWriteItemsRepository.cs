using Application.Interfaces.Persistance.Shared;
using Application.Items.Models;
using Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Persistance.Items
{
  public interface IWriteItemsRepository : IAddRepository<ItemManipulationDto,ItemDto>, IUpdateRepository<Item,ItemToUpdateDto>, IDeleteRepository, ISaveRepository
    {
    }
}
