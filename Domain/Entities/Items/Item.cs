using System;
using Domain.Entities.Common;
using Domain.Entities.Lists;

namespace Domain.Entities.Items
{
  public class Item : IEntityId, IEntityName
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Completed { get; set; }
        public bool isCompleted { get; set; }

        public Guid ListId { get; set; }
        public List List { get; set; }
    }
}
