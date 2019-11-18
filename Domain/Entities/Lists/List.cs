using System;
using Domain.Entities.Common;
using System.Collections.Generic;
using Domain.Entities.Items;
using Domain.Entities.Users;

namespace Domain.Entities.Lists
{
    public class List : IEntityId, IEntityName
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Created { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();
    }
}
