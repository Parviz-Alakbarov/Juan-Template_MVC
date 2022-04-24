using System;

namespace JuanShoesStore.Models
{
    public class TimeLoggingEntity : BaseEntity
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
