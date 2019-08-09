using System;

namespace Library.Infrastructure.Data.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}