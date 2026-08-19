using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Enum;

namespace Data_Access_Layer.Models
{
    public class Order :BaseEntity<int>
    {
        public decimal Amount { get; set; }

        public OrderStatus Status { get; set; }

        //public DateTime OrderDate { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
    }
}
