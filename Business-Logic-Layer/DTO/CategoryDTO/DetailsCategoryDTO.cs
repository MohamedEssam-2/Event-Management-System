using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.EventDTO;

namespace Business_Logic_Layer.DTO.CategoryDTO
{
    public class DetailsCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }= null!;
        public List<ReadAllEventDTO> Events { get; set; }

    }
}
