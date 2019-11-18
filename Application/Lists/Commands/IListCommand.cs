using Application.Interfaces.Commands;
using Application.Lists.Models;
using Domain.Entities.Lists;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Lists.Commands
{
   public interface IListCommand : IResourceCommand<ListManipulationDto,ListDto>,IResourceUpdateCommand<List,ListManipulationDto>
    {
    }
}
