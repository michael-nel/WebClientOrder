using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WebClientOrder.Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        protected Entity()
        {
            Id = Guid.NewGuid();
        }


    }
}
