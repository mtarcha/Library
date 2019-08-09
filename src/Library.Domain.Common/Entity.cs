using System;

namespace Library.Domain.Common
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
    {
        private readonly IEventDispatcher _eventDispatcher;

        protected Entity(TId id, IEventDispatcher eventDispatcher)
        {
            if (object.Equals(id, default(TId)))
            {
                throw new ArgumentException("The ID cannot be the type's default value.", nameof(id));
            }

            Id = id;
            _eventDispatcher = eventDispatcher;
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

        protected void RaiseEventImmediately(IDomainEvent domainEvent)
        {
            _eventDispatcher.DispatchImmediately(domainEvent);
        }

        protected void RaiseEventDeferred(IDomainEvent domainEvent)
        {
            _eventDispatcher.DispatchDeferred(domainEvent);
        }
    }
}