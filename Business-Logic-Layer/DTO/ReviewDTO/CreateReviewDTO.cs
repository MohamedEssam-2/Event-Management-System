using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.DTO.ReviewDTO
{
    public class CreateReviewDTO
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        [MaxLength(1000, ErrorMessage = "Comment must be less than 1000 characters.")]
        public string? Comment { get; set; }

        [Required(ErrorMessage = "EventId must be Enterd here .")]
        [Range(1, int.MaxValue, ErrorMessage = "EventId must be greater than 0.")]
        public int EventId { get; set; }

    }
}
