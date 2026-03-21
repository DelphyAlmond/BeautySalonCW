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
    public class ProductController(IProductAdapter adapter) : ControllerBase
    {
        private readonly IProductAdapter _adapter = adapter;

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

        [HttpGet("by-manufacturer/{manufacturerId}")]
        public IActionResult GetByManufacturer(string manufacturerId)
        {
            return _adapter.GetByManufacturer(manufacturerId).GetResponse(Request, Response);
        }

        [HttpPost]
        public IActionResult Register([FromBody] ProductBM model)
        {
            return _adapter.RegisterProduct(model).GetResponse(Request, Response);
        }

        [HttpPost]
        public IActionResult ChangeInfo([FromBody] ProductBM model)
        {
            return _adapter.ChangeProductInfo(model).GetResponse(Request, Response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return _adapter.RemoveProduct(id).GetResponse(Request, Response);
        }
    }
}