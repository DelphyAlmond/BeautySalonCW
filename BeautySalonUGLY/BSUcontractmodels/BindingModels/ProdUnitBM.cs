using BSUcontrmodels.DataModels;
using System.ComponentModel.DataAnnotations;

namespace BSUcontractmodels.BindingModels;

public class ProdUnitBM
{
    // [ x ] OrderID, т.к. определяется родительским заказом
    public string? ProductID { get; set; }
    public int Count { get; set; }
}
