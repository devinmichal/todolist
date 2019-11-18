using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Persistance.Items
{
    public interface IItemsRepository : IReadItemsRepository, IWriteItemsRepository
    {
    }
}
