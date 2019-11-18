using Application.Interfaces.Commands;
using Application.Items.Models;
using Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Items.Commands
{
   public interface IItemsCommand : IResourceCommand<ItemManipulationDto,ItemDto>, IResourceUpdateCommand<Item,ItemToUpdateDto>
    {
    }
}
