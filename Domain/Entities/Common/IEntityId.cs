using System;


namespace Domain.Entities.Common
{
    interface IEntityId
    {
        Guid Id { get; set; }
    }
}
