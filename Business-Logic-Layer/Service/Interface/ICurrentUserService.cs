using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service.Interface
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? FullName { get; }
        string? Email { get; }
        string? Role { get; }
    }
}
