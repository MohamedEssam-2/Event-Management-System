using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Enum;

namespace Business_Logic_Layer.DTO.OrderDTO
{
    public class ReadOrderDTO
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public decimal Amount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        //public DateTime OrderDate { get; set; }
    }
}
