using BaseConfig.EntityObject.Entity;
using MediatR;

namespace BaseConfig.Infrashtructure.Interface.Events
{
    public class EntityDeletedEvent<T> : INotification where T : Entity
    {
        public T Data { get; set; }

        public EntityDeletedEvent(T entity)
        {
            Data = entity;
        }
    }
}
