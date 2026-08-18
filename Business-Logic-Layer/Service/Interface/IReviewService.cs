using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.ReviewDTO;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IReviewService
    {
        public Task<ServiceResponse<int>> CreateReview(CreateReviewDTO dto);
        public Task<ServiceResponse<GetReviewDTO>> GetReviewById(int id);
        public Task<ServiceResponse<GetReviewDTO>>UpdateReview(int id, UpdateReviewDTO dto);
        public Task<ServiceResponse<List<GetReviewDTO>>> MyReviews();
        public Task<ServiceResponse<bool>> DeleteReview(int id);
        public Task<ServiceResponse<List<GetReviewDTO>>> GetAllReviewsByEventId(int eventId);
    }
}
