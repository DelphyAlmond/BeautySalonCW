using BSUcontractmodels.AdapterContracts;
using BSUcontractmodels.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BSUWebAppi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrderController(IOrderAdapter adapter) : ControllerBase
    {
        private readonly IOrderAdapter _adapter = adapter;

        // CRUD
        [HttpGet("{id}")]
        public IActionResult GetOrder(string id)
        {
            return _adapter.GetElement(id).GetResponse(Request, Response);
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderBM model)
        {
            return _adapter.RegisterOrder(model).GetResponse(Request, Response);
        }

        [HttpPut]
        public IActionResult UpdateOrder([FromBody] OrderBM model)
        {
            return _adapter.UpdateOrder(model).GetResponse(Request, Response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(string id)
        {
            return _adapter.RemoveOrder(id).GetResponse(Request, Response);
        }

        // Фильтрация

        [HttpGet("by-date")]
        public IActionResult GetOrdersByDateGap([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            return _adapter.GetOrdersByDateGap(from, to).GetResponse(Request, Response);
        }

        [HttpGet("by-worker/{workerId}")]
        public IActionResult GetOrdersByWorker(string workerId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            return _adapter.GetOrdersByWorker(workerId, from, to).GetResponse(Request, Response);
        }

        [HttpGet("by-customer/{customerId}")]
        public IActionResult GetOrdersByCustomer(string customerId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            return _adapter.GetOrdersByCustomer(customerId, from, to).GetResponse(Request, Response);
        }

        [HttpGet("by-product/{productId}")]
        public IActionResult GetOrdersByProduct(string productId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            return _adapter.GetOrdersByProduct(productId, from, to).GetResponse(Request, Response);
        }
    }
}