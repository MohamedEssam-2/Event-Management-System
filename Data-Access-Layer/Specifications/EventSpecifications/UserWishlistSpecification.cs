using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.Specifications.Base;

namespace Data_Access_Layer.Specifications.EventSpecifications
{
    public class UserWishlistSpecification :BaseSpecification<Wishlist, int>
    {
        public UserWishlistSpecification(string userId):base(u=>u.UserId == userId)
        {
            AddInclude(e => e.Event);
            AddInclude(u=>u.User);
        }
    }
}
