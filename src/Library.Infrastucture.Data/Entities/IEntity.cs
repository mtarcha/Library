using System;

namespace Library.Infrastucture.Data.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}