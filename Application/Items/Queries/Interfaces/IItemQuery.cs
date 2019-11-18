using Application.Interfaces.Queries;
using Application.Items.Models;
using Domain.Entities.Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Items.Queries.Interfaces
{
  public interface IItemQuery : IResourceQuery<ItemDto>, IResourceWithTrackingQuery<Item>
    {
     
    }
}
