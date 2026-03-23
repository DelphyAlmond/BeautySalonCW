using BSUcontractmodels.AdapterContracts;
using BSUcontractmodels.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BSUWebAppi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")] // - указывает путь по контроллеру,
                                         // где [controller] > это имя самого контроллера,
                                         // но без приписки "-Controller". Эту "ссылку"
                                         // можно дополнить, в частности атрибутом
                                         // [action], указвывющим, что учитывается
                                         // не только название к.-ра, но и метода.
                                         // => Множественность меток [Http-...]
    [ApiController]
    [Produces("application/json")]
    public class ProductController(IProductAdapter adapter) : ControllerBase
    {
        private readonly IProductAdapter _adapter = adapter;

        [HttpGet]
        public IActionResult GetAllRecords(bool includeDeleted) // < +
        {
            return _adapter.GetList(includeDeleted).GetResponse(Request, Response);
        }

        [HttpGet("{data}")] // instead of -Post("{data}")] *
        public IActionResult GetRecord(string data)
        {
            return _adapter.GetElement(data).GetResponse(Request, Response);
        }

        [HttpGet] // instead of -(/by-manufacturer/"{manufacturerId}")] *
        public IActionResult GetByManufacturer(string manufacturerId, bool includeDeleted) // < +
        {
            return _adapter.GetByManufacturer(manufacturerId, includeDeleted).GetResponse(Request, Response);
        }

        [HttpPost]
        public IActionResult Register([FromBody] ProductBM model)
        {
            return _adapter.RegisterProduct(model).GetResponse(Request, Response);
        }

        [HttpPut] // не сильно принципиально, можно было бы оставить и post во всех, но для разнообразия ~
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