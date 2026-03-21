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
    public class ServiceController(IServiceAdapter adapter) : ControllerBase
    {
        private readonly IServiceAdapter _adapter = adapter;

        [HttpGet]
        public IActionResult GetAllRecords()
        {
            return _adapter.GetList().GetResponse(Request, Response);
        }

        [HttpPost("{data}")]
        public IActionResult GetRecord(string data)
        {
            return _adapter.GetElement(data).GetResponse(Request, Response);
        }

        [HttpPost]
        public IActionResult Register([FromBody] ServiceBM model)
        {
            return _adapter.RegisterService(model).GetResponse(Request, Response);
        }

        [HttpPost]
        public IActionResult ChangeInfo([FromBody] ServiceBM model)
        {
            return _adapter.ChangeServiceInfo(model).GetResponse(Request, Response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return _adapter.RemoveService(id).GetResponse(Request, Response);
        }
    }
}