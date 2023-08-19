using BaseConfig.EntityObject.Entity;
using MediatR;

namespace BaseConfig.Infrashtructure.Interface.Events
{
    public class EntityChangedEvent<T> : INotification where T : Entity
    {
        public T Data { get; set; }

        public EntityChangedEvent(T entity)
        {
            Data = entity;
        }
    }
}
