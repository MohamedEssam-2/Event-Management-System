using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO.WishlistDTO
{
    public class ReadWishlistDTO
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }= null!;
        public string EventDescription { get; set; } = null!;
        public DateTime EventDate { get; set; } 
        public string EventLocation { get; set; } = null!;

    }
}
