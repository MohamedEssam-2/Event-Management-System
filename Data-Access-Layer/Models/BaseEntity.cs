using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class BaseEntity<TKey>
    {
        public TKey Id { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
