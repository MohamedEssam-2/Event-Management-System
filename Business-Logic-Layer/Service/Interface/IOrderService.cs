using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Logic_Layer.DTO.OrderDTO;

namespace Business_Logic_Layer.Service.Interface
{
    public interface IOrderService
    {
        public Task <ServiceResponse<int>> CreateOrder(int EventId);
        public Task<ServiceResponse<List<ReadOrderDTO>>> GetMyOrders();
        public Task<ServiceResponse<bool>> DeleteOrder(int OrderId);
        public Task<ServiceResponse<ReadOrderDTO>> GetOrderById(int OrderId);
        public Task<ServiceResponse<List<ReadOrderDTO>>> GetAllOrders();

    }
}
