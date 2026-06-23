using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class Category : BaseEntity<int>
    {
       
        public string Name { get; set; } = null!;
        public ICollection<Event> Events { get; set; } = new List<Event>();

    }
}
