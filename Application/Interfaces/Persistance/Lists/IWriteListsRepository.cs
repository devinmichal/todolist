using Application.Interfaces.Persistance.Shared;
using Application.Lists.Models;
using Domain.Entities.Lists;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Persistance.Lists
{
   public interface IWriteListsRepository : IAddRepository<ListManipulationDto, ListDto>, IUpdateRepository<List,ListManipulationDto> , ISaveRepository , IDeleteRepository
    {
    }
}
