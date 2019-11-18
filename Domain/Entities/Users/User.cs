using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using Domain.Entities.Lists;
namespace Domain.Entities.Users
{
    public class User : IEntityId
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<List> Lists { get; set; } = new List<List>();
    }
}
