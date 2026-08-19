using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.WishlistDTO;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IWishlistService
    {
        public Task<ServiceResponse<int>> CreateWishlist(int eventId);
        public Task<ServiceResponse<List<ReadWishlistDTO>>> GetWishlist();
        public Task<ServiceResponse<bool>> DeleteWishlist(int id);
        //public Task<ServiceResponse<List<WishlistByEventDTO>>> GetWishlistByEventId(int EventId);

    }
}
