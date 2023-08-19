using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MediatR;
using MongoDB.Bson.Serialization.Attributes;
using BaseConfig.Extentions.Datetime;

namespace BaseConfig.EntityObject.Entity
{
    public class Entity : ValidationObject
    {
        private int? _requestedHashCode;

        private List<INotification> _domainEvents;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", Order = 0)]
        [BsonId]
        public virtual long Id { get; set; }
        [Column("guid", Order = 1)]
        public virtual Guid Guid { get; set; } = Guid.NewGuid();

        [Column("created_user_guid", Order = 101)]
        [Required(AllowEmptyStrings = true)]
        public Guid? CreatedUserGuid { get; set; } = Guid.NewGuid();

        [Column("updated_user_guid", Order = 102)]
        public Guid? UpdatedUserGuid { get; set; } = Guid.NewGuid();

        [Column("deleted_user_guid", Order = 103)]
        public Guid? DeletedUserGuid { get; set; } = Guid.NewGuid();

        [Column("created_username", Order = 104)]
        [MaxLength(100)]
        public string? CreatedUserName { get; set; }

        [Column("updated_username", Order = 105)]
        [MaxLength(100)]
        public string? UpdatedUserName { get; set; }

        [Column("deleted_username", Order = 106)]
        [MaxLength(100)]
        public string? DeletedUserName { get; set; }

        [Column("created_date_ts", Order = 107)]
        public double? CreatedDateTS { get; set; }

        [Column("updated_date_ts", Order = 108)]
        public double? UpdatedDateTS { get; set; }

        [Column("deleted_date_ts", Order = 109)]
        public double? DeletedDateTS { get; set; }

        [Column("is_deleted", Order = 110)]
        [DefaultValue("false")]
        public bool IsDeleted { get; set; }

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        protected Entity()
        {
            DateTime utcNow = DateTime.UtcNow;
            CreatedDateTS = utcNow.GetTimeStamp(includedTimeValue: true);
            Id = 0;
        }

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public bool IsTransient()
        {
            return Id == 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
            {
                return false;
            }

            if (this == obj)
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            Entity entity = (Entity)obj;
            if (entity.IsTransient() || IsTransient())
            {
                return false;
            }

            return entity.Id == Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                {
                    _requestedHashCode = Id.GetHashCode() ^ 0x1F;
                }

                return _requestedHashCode.Value;
            }

            return base.GetHashCode();
        }
    }
}
