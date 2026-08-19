using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class UserEventWishlistSpecification:BaseSpecification<Wishlist,int>
    {
        public UserEventWishlistSpecification(string userId,int EventId):base(w => w.UserId == userId && w.EventId == EventId)
        {  
        }
    }
}
