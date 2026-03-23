using BSUcontractmodels.AdapterContracts;
using BSUcontractmodels.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BSUWebAppi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    // Передача в формате json:
    [Produces("application/json")]
    public class CustomerController(ICustomerAdapter adapter) : ControllerBase
    {
        private readonly ICustomerAdapter _adapter = adapter;

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
        public IActionResult Register([FromBody] CustomerBM model)
        {
            return _adapter.RegisterCustomer(model).GetResponse(Request, Response);
        }

        [HttpPut] // ~ (see product)
        public IActionResult ChangeInfo([FromBody] CustomerBM model)
        {
            return _adapter.ChangeCustomerInfo(model).GetResponse(Request, Response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return _adapter.RemoveCustomer(id).GetResponse(Request, Response);
        }
    }
}
