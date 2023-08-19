using BaseConfig.EntityObject.Entity;
using MediatR;

namespace BaseConfig.Infrashtructure.Interface.Events
{
    public class EntityCreatedEvent<T> : INotification where T : Entity
    {
        public T Data { get; set; }

        public EntityCreatedEvent(T entity)
        {
            Data = entity;
        }
    }
}
