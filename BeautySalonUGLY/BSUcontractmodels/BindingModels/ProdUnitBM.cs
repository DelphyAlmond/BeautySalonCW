using BSUcontrmodels.DataModels;
using System.ComponentModel.DataAnnotations;

namespace BSUcontractmodels.BindingModels;

public class ProdUnitBM
{
    // [ ? ] OrderID - определяется родительским заказом
    public string? OrderID { get; set; }

    public string? ProductID { get; set; }
    public int Count { get; set; }

    public double Price { get; set; }
}
