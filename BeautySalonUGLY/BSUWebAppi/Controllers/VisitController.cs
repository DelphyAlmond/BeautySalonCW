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
    public class VisitController(IVisitAdapter adapter) : ControllerBase
    {
        private readonly IVisitAdapter _adapter = adapter;

        // CRUD
        [HttpGet("{id}")]
        public IActionResult GetVisit(string id)
        {
            return _adapter.GetElement(id).GetResponse(Request, Response);
        }

        [HttpPost]
        public IActionResult CreateVisit([FromBody] VisitBM model)
        {
            return _adapter.RegisterVisit(model).GetResponse(Request, Response);
        }

        [HttpPut]
        public IActionResult UpdateVisit([FromBody] VisitBM model)
        {
            return _adapter.UpdateVisit(model).GetResponse(Request, Response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVisit(string id)
        {
            return _adapter.RemoveVisit(id).GetResponse(Request, Response);
        }

        // Фильтрация
        [HttpGet("by-date")]
        public IActionResult GetVisitsByDateGap([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            return _adapter.GetVisitsByDateGap(from, to).GetResponse(Request, Response);
        }

        [HttpGet("by-master/{workerId}")]
        public IActionResult GetVisitsByMaster(string workerId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            return _adapter.GetVisitsByMaster(workerId, from, to).GetResponse(Request, Response);
        }

        [HttpGet("by-customer/{customerId}")]
        public IActionResult GetVisitsByCustomer(string customerId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            return _adapter.GetVisitsByCustomer(customerId, from, to).GetResponse(Request, Response);
        }

        [HttpGet("by-service/{serviceId}")]
        public IActionResult GetVisitsByService(string serviceId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            return _adapter.GetVisitsByService(serviceId, from, to).GetResponse(Request, Response);
        }
    }
}