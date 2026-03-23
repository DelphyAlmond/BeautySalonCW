using BSUcontrmodels.DataModels;
using BSUcontrmodels.Enums;
using System.ComponentModel.DataAnnotations;

namespace BSUcontractmodels.BindingModels;

public class ProductBM
{
    [Required(ErrorMessage = "> ID обязателен")]
    public string? ID { get; set; }
    public string? ProductNaming { get; set; }
    public ProductType ProductType { get; set; }
    public string? ManufacturerID { get; set; }
    public double Price { get; set; }
    public string? Description { get; set; }

    // IsDeleted - не учитывается со стороны клиента
    // т.к. мы (система) проставляем его сами
}
