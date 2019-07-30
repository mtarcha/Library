using System;

namespace Library.Domain
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
    {
        protected Entity(TId id)
        {
            if (object.Equals(id, default(TId)))
            {
                throw new ArgumentException("The ID cannot be the type's default value.", nameof(id));
            }

            Id = id;
        }

        public TId Id { get; protected set; }

        public override bool Equals(object other)
        {
            return
                other is Entity<TId> entity
                    ? Equals(entity)
                    : base.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public bool Equals(Entity<TId> other)
        {
            return other != null && Id.Equals(other.Id);
        }
    }
}