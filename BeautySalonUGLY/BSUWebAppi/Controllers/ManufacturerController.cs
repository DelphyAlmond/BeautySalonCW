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
    public class ManufacturerController(IManufacturerAdapter adapter) : ControllerBase
    {
        private readonly IManufacturerAdapter _adapter = adapter;

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
        public IActionResult Register([FromBody] ManufacturerBM model)
        {
            return _adapter.RegisterManufacturer(model).GetResponse(Request, Response);
        }

        [HttpPut] // ~
        public IActionResult ChangeInfo([FromBody] ManufacturerBM model)
        {
            return _adapter.ChangeManufacturerInfo(model).GetResponse(Request, Response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return _adapter.RemoveManufacturer(id).GetResponse(Request, Response);
        }
    }
}